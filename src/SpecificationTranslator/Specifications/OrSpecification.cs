using System;
using System.Linq.Expressions;

namespace SpecificationTranslator.Specifications
{
    public class OrSpecification<T> : BinarySpecification<T>
    {
        public OrSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right) { }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return LeftSpecification.AsExpression().Or(RightSpecification.AsExpression());
        }

    }
}
