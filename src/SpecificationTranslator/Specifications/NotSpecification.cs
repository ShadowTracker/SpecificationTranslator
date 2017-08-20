using System;
using System.Linq.Expressions;

namespace SpecificationTranslator.Specifications
{
    public class NotSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _toNegateSpecification;

        public NotSpecification(ISpecification<T> toNegateSpecification)
        {
            this._toNegateSpecification = toNegateSpecification;
        }

        public override Expression<Func<T, bool>> AsExpression()
        {
            var expression = _toNegateSpecification.AsExpression();
            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(expression.Body),
                expression.Parameters
            );
        }

    }
}
