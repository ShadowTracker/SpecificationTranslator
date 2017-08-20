using System;
using System.Linq.Expressions;

namespace SpecificationTranslator.Specifications
{
    public static class SpecificationExtension
    {
        public static ISpecification<T> AndIf<T>(this ISpecification<T> specification, bool condition, ISpecification<T> other)
        {
            return condition ? specification.And(other) : specification;
        }

        public static ISpecification<T> NotIf<T>(this ISpecification<T> specification, bool condition)
        {
            return condition ? specification.Not() : specification;
        }

        public static ISpecification<T> OrIf<T>(this ISpecification<T> specification, bool condition, ISpecification<T> other)
        {
            return condition ? specification.Or(other) : specification;
        }

        public static ISpecification<T> AndIf<T>(this ISpecification<T> specification, bool condition, Expression<Func<T, bool>> predicateExpression)
        {
            return condition ? specification.And(new AnonymousSpecification<T>(predicateExpression)) : specification;
        }

        public static ISpecification<T> OrIf<T>(this ISpecification<T> specification, bool condition, Expression<Func<T, bool>> predicateExpression)
        {
            return condition ? specification.Or(new AnonymousSpecification<T>(predicateExpression)) : specification;
        }

        public static ISpecification<T> And<T>(this ISpecification<T> specification, Expression<Func<T, bool>> predicateExpression)
        {
            return specification.And(new AnonymousSpecification<T>(predicateExpression));
        }

        public static ISpecification<T> Or<T>(this ISpecification<T> specification, Expression<Func<T, bool>> predicateExpression)
        {
            return specification.Or(new AnonymousSpecification<T>(predicateExpression));
        }
       
    }
}
