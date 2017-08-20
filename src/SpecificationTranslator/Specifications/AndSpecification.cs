using System;
using System.Linq.Expressions;

namespace SpecificationTranslator.Specifications
{
    public class AndSpecification<T> : BinarySpecification<T>
    {
        public AndSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right) { }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return LeftSpecification.AsExpression().And(RightSpecification.AsExpression());
        }
    }
}
