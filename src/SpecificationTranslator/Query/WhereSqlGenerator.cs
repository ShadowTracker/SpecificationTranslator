using SpecificationTranslator.Query.ExpressionTranslators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SpecificationTranslator.Query.Expressions;
using JetBrains.Annotations;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace SpecificationTranslator.Query
{
    public abstract class WhereSqlGenerator : ExpressionVisitor, ISqlExpressionVisitor
    {
        private readonly ISqlGenerationHelper _sqlGenerationHelper;
        private readonly IReadOnlyDictionary<string, object> _parametersValues;
        private readonly IMethodCallTranslator _methodCallTranslator;
        private Expression _expression;
        private StringBuilder _sqlBuilder;

        private const string ALWAYS_TRUE_SQL = "1 = 1";
        private const string ALWAYS_FALSE_SQL = "0 = 1";

        protected WhereSqlGenerator(ISqlGenerationHelper sqlGenerationHelper, Expression expression)
        {
            _sqlGenerationHelper = sqlGenerationHelper;
            _parametersValues = new Dictionary<string, object>();
            _methodCallTranslator = new RelationalCompositeMethodCallTranslator();
            _expression = expression;
        }

        protected virtual bool TryGenerateBinaryOperator(ExpressionType op, [NotNull] out string result)
            => SqlBinaryOperators.TryGetValue(op, out result);

        protected virtual string ConcatOperator => "+";

        public string Generate()
        {
            _expression = PartialEvaluator.Eval(_expression);

            this._sqlBuilder = new StringBuilder();

            this.Visit(_expression);

            string result = this._sqlBuilder.ToString();
            string trueSqlValue = _sqlGenerationHelper.GenerateLiteral(true);
            string falseSqlValue = _sqlGenerationHelper.GenerateLiteral(false);
            if (result.Equals(trueSqlValue))
            {
                return ALWAYS_TRUE_SQL;
            }
            else if (result.Equals(falseSqlValue))
            {
                return ALWAYS_FALSE_SQL;
            }
            else
            {
                return result;
            }
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }

            return e;
        }

        protected virtual string GenerateBinaryOperator(ExpressionType op)
        {
            return SqlBinaryOperators.Get(op);
        }


        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Method.DeclaringType == typeof(Queryable)
                && methodCallExpression.Method.Name == "Where")
                return methodCallExpression;

            var expression = _methodCallTranslator.Translate(methodCallExpression);
            if (expression != null)
            {
                this.Visit(expression);

                return expression;
            }

            var inValuesExpression = TranslateInExpression(methodCallExpression);
            if (inValuesExpression != null)
            {
                this.Visit(inValuesExpression);
                return inValuesExpression;
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", methodCallExpression.Method.Name));
        }

        private InExpression TranslateInExpression(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression != null)
            {
                var inValuesExpression = methodCallExpression.Arguments[0] as ConstantExpression;
                if (inValuesExpression != null && "Contains" == methodCallExpression.Method.Name
                    /*&& inValuesExpression.Type.IsAssignableFrom(typeof(object[]))*/)
                {
                    var inExpression = new InExpression(methodCallExpression.Arguments[1], new[] { inValuesExpression });

                    return inExpression;
                }

                inValuesExpression = methodCallExpression.Object as ConstantExpression;
                if (inValuesExpression != null && "Contains" == methodCallExpression.Method.Name
                    /*&& inValuesExpression.Type.IsAssignableFrom(typeof(object[]))*/)
                {
                    var inExpression = new InExpression(methodCallExpression.Arguments[0], new[] { inValuesExpression });

                    return inExpression;
                }
            }

            return null;
        }

        protected override Expression VisitUnary(UnaryExpression unaryExpression)
        {

            switch (unaryExpression.NodeType)
            {

                //case ExpressionType.Not:

                //    sb.Append(" NOT ");

                //    this.Visit(unaryExpression.Operand);

                //    break;
                case ExpressionType.Not:

                    var inExpression = unaryExpression.Operand as InExpression;
                    if (inExpression != null)
                    {
                        return VisitNotIn(inExpression);
                    }

                    var methodCallExpression = unaryExpression.Operand as MethodCallExpression;
                    var inValuesExpression = TranslateInExpression(methodCallExpression);
                    if (inValuesExpression != null)
                    {
                        this.VisitNotIn(inValuesExpression);
                        return inValuesExpression;
                    }

                    var isNullExpression = unaryExpression.Operand as IsNullExpression;
                    if (isNullExpression != null)
                    {
                        return VisitIsNotNull(isNullExpression);
                    }

                    //if (unaryExpression.Operand is ExistsExpression)
                    //{
                    //    sb.Append("NOT ");

                    //    Visit(unaryExpression.Operand);

                    //}

                    _sqlBuilder.Append("NOT (");

                    Visit(unaryExpression.Operand);

                    _sqlBuilder.Append(")");
                    break;
                case ExpressionType.Convert:
                    Visit(unaryExpression.Operand);
                    break;

                default:

                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", unaryExpression.NodeType));

            }

            return unaryExpression;
        }

        protected override Expression VisitBinary(BinaryExpression binaryExpression)
        {
            if (ShouldBeOptimizedForBooleanExpression(binaryExpression))
            {

                if (IsBooleanConstantExpression(binaryExpression.Left) && IsBooleanConstantExpression(binaryExpression.Right))
                {
                    var leftBoolean = binaryExpression.Left as ConstantExpression;
                    var rightBoolean = binaryExpression.Right as ConstantExpression;
                    bool leftBooleanValue = (bool)leftBoolean.Value;
                    bool rightBooleanValue = (bool)rightBoolean.Value;

                    //return leftBooleanValue && leftBooleanValue ? this.VisitAlways(alwaysTrueExpression) : this.VisitAlways(alwaysFalseExpression);
                    if (leftBooleanValue && leftBooleanValue)
                    {
                        _sqlBuilder.Append(ALWAYS_TRUE_SQL);
                    }
                    else
                    {
                        _sqlBuilder.Append(ALWAYS_FALSE_SQL);
                    }

                    return binaryExpression;
                }
                else if (IsBooleanConstantExpression(binaryExpression.Left))
                {
                    var leftBoolean = binaryExpression.Left as ConstantExpression;
                    bool leftBooleanValue = (bool)leftBoolean.Value;

                    if (leftBooleanValue)
                    {
                        _sqlBuilder.Append(ALWAYS_TRUE_SQL);
                    }
                    else
                    {
                        _sqlBuilder.Append(ALWAYS_FALSE_SQL);
                    }
                    _sqlBuilder.Append(SqlBinaryOperators.Get(binaryExpression.NodeType));

                    return this.Visit(binaryExpression.Right);
                }
                else if (IsBooleanConstantExpression(binaryExpression.Right))
                {
                    var rightBoolean = binaryExpression.Right as ConstantExpression;
                    bool rightBooleanValue = (bool)rightBoolean.Value;

                    if (rightBooleanValue)
                    {
                        _sqlBuilder.Append(ALWAYS_TRUE_SQL);
                    }
                    else
                    {
                        _sqlBuilder.Append(ALWAYS_FALSE_SQL);
                    }
                    _sqlBuilder.Append(SqlBinaryOperators.Get(binaryExpression.NodeType));

                    return this.Visit(binaryExpression.Left);
                }
            }

            _sqlBuilder.Append("(");

            this.Visit(binaryExpression.Left);

            string op;
            if (!TryGenerateBinaryOperator(binaryExpression.NodeType, out op))
            {
                switch (binaryExpression.NodeType)
                {
                    case ExpressionType.Add:
                        {
                            op = binaryExpression.Type == typeof(string)
                               ? " " + ConcatOperator + " "
                               : " + ";
                            break;
                        }
                    default:
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                }
            }
            else
            {
                if (binaryExpression.NodeType == ExpressionType.Equal
                    || binaryExpression.NodeType == ExpressionType.NotEqual)
                {
                    var left = binaryExpression.Left as ConstantExpression;
                    var right = binaryExpression.Right as ConstantExpression;
                    if ((left != null && left.Value == null)
                        || (right != null && right.Value == null))
                    {
                        switch (binaryExpression.NodeType)
                        {
                            case ExpressionType.Equal:
                                {
                                    op = " IS ";
                                    break;
                                }
                            case ExpressionType.NotEqual:
                                {
                                    op = " IS NOT ";
                                    break;
                                }
                            default:
                                {
                                    throw new ArgumentOutOfRangeException();
                                }
                        }
                    }
                }
                else
                {
                    op = GenerateBinaryOperator(binaryExpression.NodeType);
                }
            }

            _sqlBuilder.Append(op);

            this.Visit(binaryExpression.Right);

            _sqlBuilder.Append(")");

            return binaryExpression;

        }

        private static bool IsBooleanConstantExpression(Expression expression)
        {
            return expression.NodeType == ExpressionType.Constant && expression.Type == typeof(bool);
        }

        private bool ShouldBeOptimizedForBooleanExpression(BinaryExpression binaryExpression)
        {
            if (binaryExpression.NodeType != ExpressionType.AndAlso && binaryExpression.NodeType != ExpressionType.OrElse)
                return false;

            if ((binaryExpression.Left.NodeType == ExpressionType.Constant && binaryExpression.Left.Type == typeof(bool)) ||
                (binaryExpression.Right.NodeType == ExpressionType.Constant && binaryExpression.Right.Type == typeof(bool)))
                return true;

            return false;
        }

        protected override Expression VisitConstant(ConstantExpression constantExpression)
        {
            IQueryable q = constantExpression.Value as IQueryable;
            if (q != null)
            {
                // assume constant nodes w/ IQueryables are table references

                _sqlBuilder.Append("SELECT * FROM ");

                _sqlBuilder.Append(q.ElementType.Name);

            }
            else if (constantExpression.Value == null)
            {
                _sqlBuilder.Append("NULL");
            }
            else
            {
                _sqlBuilder.Append(_sqlGenerationHelper.GenerateLiteral(constantExpression.Value));
            }

            return constantExpression;

        }


        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            if (memberExpression.Expression != null && memberExpression.Expression.NodeType == ExpressionType.Parameter)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;
                string columnName = propertyInfo != null ? GetColumnName(propertyInfo) : memberExpression.Member.Name;

                _sqlBuilder.Append(columnName);

                return memberExpression;
            }

            throw new NotSupportedException(string.Format("The member '{0}' is not supported", memberExpression.Member.Name));
        }

        internal string GetColumnName(PropertyInfo propertyInfo)
        {
            if (propertyInfo.IsDefined(typeof(ColumnAttribute), false))
                return propertyInfo.GetCustomAttribute<ColumnAttribute>().Name;

            return propertyInfo.Name;
        }

        public Expression VisitIsNull([NotNull] IsNullExpression isNullExpression)
        {
            Visit(isNullExpression.Operand);

            _sqlBuilder.Append(" IS NULL");

            return isNullExpression;
        }

        /// <summary>
        ///     Visits an IsNotNullExpression.
        /// </summary>
        /// <param name="isNotNullExpression"> The is not null expression. </param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        public virtual Expression VisitIsNotNull([NotNull] IsNullExpression isNotNullExpression)
        {
            Visit(isNotNullExpression.Operand);

            _sqlBuilder.Append(" IS NOT NULL");

            return isNotNullExpression;
        }

        public Expression VisitLike([NotNull] LikeExpression likeExpression)
        {
            Visit(likeExpression.Match);

            _sqlBuilder.Append(" LIKE ");

            Visit(likeExpression.Pattern);

            return likeExpression;
        }

        public Expression VisitStringCompare([NotNull] StringCompareExpression stringCompareExpression)
        {
            Visit(stringCompareExpression.Left);

            _sqlBuilder.Append(GenerateBinaryOperator(stringCompareExpression.Operator));

            Visit(stringCompareExpression.Right);

            return stringCompareExpression;
        }

        public virtual Expression VisitIn(InExpression inExpression)
        {
            if (inExpression.Values != null)
            {
                var inValues = ProcessInExpressionValues(inExpression.Values);
                var inValuesNotNull = ExtractNonNullExpressionValues(inValues);

                if (inValues.Count != inValuesNotNull.Count)
                {
                    var relationalNullsInExpression
                        = Expression.OrElse(
                            new InExpression(inExpression.Operand, inValuesNotNull),
                            new IsNullExpression(inExpression.Operand));

                    return Visit(relationalNullsInExpression);
                }

                if (inValuesNotNull.Count > 0)
                {
                    Visit(inExpression.Operand);

                    _sqlBuilder.Append(" IN (");

                    VisitJoin(inValuesNotNull);

                    _sqlBuilder.Append(")");
                }
                else
                {
                    _sqlBuilder.Append(ALWAYS_FALSE_SQL);
                }
            }
            else // SubQuery
            {
                //Visit(inExpression.Operand);

                //sb.Append(" IN ");

                //Visit(inExpression.SubQuery);

                throw new NotSupportedException(string.Format("The InExpression '{0}' for sub-query is not supported", inExpression));
            }

            return inExpression;
        }

        private void VisitJoin(
            IReadOnlyList<Expression> items)
        {
            int lastIndex = items.Count - 1;
            for (int x = 0; x < items.Count; x++)
            {
                this.VisitConstant((ConstantExpression)items[x]);

                if (x != lastIndex)
                    _sqlBuilder.Append(", ");
            }
        }

        /// <summary>
        ///     Process the InExpression values.
        /// </summary>
        /// <param name="inExpressionValues"> The in expression values. </param>
        /// <returns>
        ///     A list of expressions.
        /// </returns>
        protected virtual IReadOnlyList<Expression> ProcessInExpressionValues(
            [NotNull] IEnumerable<Expression> inExpressionValues)
        {
            var inConstants = new List<Expression>();

            foreach (var inValue in inExpressionValues)
            {
                var inConstant = inValue as ConstantExpression;

                if (inConstant != null)
                {
                    AddInExpressionValues(inConstant.Value, inConstants, inConstant);
                }
                else
                {
                    var inParameter = inValue as ParameterExpression;

                    if (inParameter != null)
                    {
                        object parameterValue;
                        if (_parametersValues.TryGetValue(inParameter.Name, out parameterValue))
                        {
                            AddInExpressionValues(parameterValue, inConstants, inParameter);

                        }
                    }
                    else
                    {
                        var inListInit = inValue as ListInitExpression;

                        if (inListInit != null)
                        {
                            inConstants.AddRange(ProcessInExpressionValues(
                                inListInit.Initializers.SelectMany(i => i.Arguments)));
                        }
                        else
                        {
                            var newArray = inValue as NewArrayExpression;

                            if (newArray != null)
                            {
                                inConstants.AddRange(ProcessInExpressionValues(newArray.Expressions));
                            }
                        }
                    }
                }
            }

            return inConstants;
        }

        private static void AddInExpressionValues(
            object value, List<Expression> inConstants, Expression expression)
        {
            var valuesEnumerable = value as IEnumerable;

            if (valuesEnumerable != null
                && value.GetType() != typeof(string)
                && value.GetType() != typeof(byte[]))
            {
                inConstants.AddRange(valuesEnumerable.Cast<object>().Select(Expression.Constant));
            }
            else
            {
                inConstants.Add(expression);
            }
        }

        /// <summary>
        ///     Extracts the non null expression values from a list of expressions.
        /// </summary>
        /// <param name="inExpressionValues"> The list of expressions. </param>
        /// <returns>
        ///     The extracted non null expression values.
        /// </returns>
        protected virtual IReadOnlyList<Expression> ExtractNonNullExpressionValues(
            [NotNull] IReadOnlyList<Expression> inExpressionValues)
        {
            var inValuesNotNull = new List<Expression>();

            foreach (var inValue in inExpressionValues)
            {
                var inConstant = inValue as ConstantExpression;

                if (inConstant?.Value != null)
                {
                    inValuesNotNull.Add(inValue);

                    continue;
                }

                var inParameter = inValue as ParameterExpression;

                if (inParameter != null)
                {
                    object parameterValue;

                    if (_parametersValues.TryGetValue(inParameter.Name, out parameterValue))
                    {
                        if (parameterValue != null)
                        {
                            inValuesNotNull.Add(inValue);
                        }
                    }
                }
            }

            return inValuesNotNull;
        }

        /// <summary>
        ///     Visit a negated InExpression.
        /// </summary>
        /// <param name="inExpression"> The in expression. </param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        protected virtual Expression VisitNotIn([NotNull] InExpression inExpression)
        {
            if (inExpression.Values != null)
            {
                var inValues = ProcessInExpressionValues(inExpression.Values);
                var inValuesNotNull = ExtractNonNullExpressionValues(inValues);

                if (inValues.Count != inValuesNotNull.Count)
                {
                    var relationalNullsNotInExpression
                        = Expression.AndAlso(
                            Expression.Not(new InExpression(inExpression.Operand, inValuesNotNull)),
                            Expression.Not(new IsNullExpression(inExpression.Operand)));

                    return Visit(relationalNullsNotInExpression);
                }

                if (inValues.Count > 0)
                {
                    Visit(inExpression.Operand);

                    _sqlBuilder.Append(" NOT IN (");

                    VisitJoin(inValues);

                    _sqlBuilder.Append(")");
                }
                else
                {
                    _sqlBuilder.Append(ALWAYS_TRUE_SQL);
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("The NotInExpression '{0}' for sub-query is not supported", inExpression));
            }

            return inExpression;
        }
    }
}
