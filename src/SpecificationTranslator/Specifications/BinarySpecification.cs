namespace SpecificationTranslator.Specifications
{
    public abstract class BinarySpecification<T> : Specification<T>
    {
        protected ISpecification<T> LeftSpecification
        {
            get;
            private set;
        }

        protected ISpecification<T> RightSpecification
        {
            get;
            private set;
        }

        protected BinarySpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.LeftSpecification = left;
            this.RightSpecification = right;
        }
    }
}
