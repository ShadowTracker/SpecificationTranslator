// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using JetBrains.Annotations;
using SpecificationTranslator.Query.Expressions;

namespace SpecificationTranslator.Query
{
    /// <summary>
    ///     Expression visitor dispatch methods for extension expressions.
    /// </summary>
    public interface ISqlExpressionVisitor
    {
        
        /// <summary>
        ///     Visit an IsNullExpression.
        /// </summary>
        /// <param name="isNullExpression"> The is null expression. </param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        Expression VisitIsNull([NotNull] IsNullExpression isNullExpression);

        /// <summary>
        ///     Visit a LikeExpression.
        /// </summary>
        /// <param name="likeExpression"> The like expression. </param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        Expression VisitLike([NotNull] LikeExpression likeExpression);

        /// <summary>
        ///     Visit an InExpression.
        /// </summary>
        /// <param name="inExpression"> The in expression. </param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        Expression VisitIn([NotNull] InExpression inExpression);

        /// <summary>
        ///     Visit a StringCompareExpression.
        /// </summary>
        /// <param name="stringCompareExpression"> The string compare expression. </param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        Expression VisitStringCompare([NotNull] StringCompareExpression stringCompareExpression);

        /// <summary>
        ///     Visit an ExplicitCastExpression.
        /// </summary>
        /// <param name="explicitCastExpression"> The explicit cast expression. </param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        //Expression VisitExplicitCast([NotNull] ExplicitCastExpression explicitCastExpression);

        /// <summary>
        ///     Visit a PropertyParameterExpression.
        /// </summary>
        /// <param name="propertyParameterExpression"> The property parameter expression. </param>
        /// <returns>
        ///     An Expression.
        /// </returns>
        //Expression VisitPropertyParameter([NotNull] PropertyParameterExpression propertyParameterExpression);
    }
}
