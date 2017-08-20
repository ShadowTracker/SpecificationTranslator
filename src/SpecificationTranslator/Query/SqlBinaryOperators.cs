using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpecificationTranslator.Query
{
    public class SqlBinaryOperators
    {
        private static readonly Dictionary<ExpressionType, string> _binaryOperatorMap = new Dictionary<ExpressionType, string>
        {
            { ExpressionType.Equal, " = " },
            { ExpressionType.NotEqual, " <> " },
            { ExpressionType.GreaterThan, " > " },
            { ExpressionType.GreaterThanOrEqual, " >= " },
            { ExpressionType.LessThan, " < " },
            { ExpressionType.LessThanOrEqual, " <= " },
            { ExpressionType.AndAlso, " AND " },
            { ExpressionType.OrElse, " OR " },
            { ExpressionType.Subtract, " - " },
            { ExpressionType.Multiply, " * " },
            { ExpressionType.Divide, " / " },
            { ExpressionType.Modulo, " % " },
            { ExpressionType.And, " & " },
            { ExpressionType.Or, " | " }
        };

        public static bool TryGetValue(ExpressionType expressionType, out string result)
        {
            return _binaryOperatorMap.TryGetValue(expressionType, out result);
        }

        public static bool ContainsKey(ExpressionType expressionType)
        {
            return _binaryOperatorMap.ContainsKey(expressionType);
        }

        public static string Get(ExpressionType expressionType)
        {
            if (!_binaryOperatorMap.ContainsKey(expressionType))
                throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", expressionType));

            return _binaryOperatorMap[expressionType];
        }
    }
}
