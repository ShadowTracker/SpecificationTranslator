using System;
using System.Linq.Expressions;

namespace SpecificationTranslator.Specifications
{
    public sealed class AnonymousSpecification<T> : Specification<T>
    {
        private readonly Expression<Func<T, bool>> _predicateExpression;

        public AnonymousSpecification(Expression<Func<T, bool>> predicateExpression)
        {
            this._predicateExpression = predicateExpression;
        }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return _predicateExpression;
        }
    }
}
