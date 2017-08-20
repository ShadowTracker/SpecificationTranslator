using System;
using System.Linq.Expressions;

namespace SpecificationTranslator.Specifications
{
    public sealed class AnySpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> AsExpression()
        {
            return v => true;
        }
    }
}
