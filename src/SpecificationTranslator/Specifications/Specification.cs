using System;
using System.Linq.Expressions;

namespace SpecificationTranslator.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {

        public virtual bool IsSatisfiedBy(T value)
        {
            return AsExpression().Compile().Invoke(value);
        }

        public abstract Expression<Func<T, bool>> AsExpression();

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }
    }
}
