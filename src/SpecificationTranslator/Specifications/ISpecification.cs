using System;
using System.Linq.Expressions;

namespace SpecificationTranslator.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T value);

        Expression<Func<T, bool>> AsExpression();

        ISpecification<T> And(ISpecification<T> other);

        ISpecification<T> Not();

        ISpecification<T> Or(ISpecification<T> other);

    }
}
