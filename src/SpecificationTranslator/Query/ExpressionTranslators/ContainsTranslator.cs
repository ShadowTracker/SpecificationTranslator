// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using SpecificationTranslator.Query.Expressions;
using System.Linq.Expressions;
using System.Reflection;

namespace SpecificationTranslator.Query.ExpressionTranslators
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class ContainsTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _methodInfo
            = typeof(string).GetRuntimeMethod(nameof(string.Contains), new[] { typeof(string) });

        private static readonly MethodInfo _concat
            = typeof(string).GetRuntimeMethod(nameof(string.Concat), new[] { typeof(string), typeof(string) });

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
           // Check.NotNull(methodCallExpression, nameof(methodCallExpression));

            return ReferenceEquals(methodCallExpression.Method, _methodInfo)
                ? new LikeExpression(
                    // ReSharper disable once AssignNullToNotNullAttribute
                    methodCallExpression.Object,
                    Expression.Add(
                        Expression.Add(
                            Expression.Constant("%", typeof(string)),
                            methodCallExpression.Arguments[0],
                            _concat),
                        Expression.Constant("%", typeof(string)),
                        _concat))
                : null;
        }
    }
}
