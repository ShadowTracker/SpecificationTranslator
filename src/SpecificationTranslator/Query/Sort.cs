namespace SpecificationTranslator.Query
{
    public class Sort
    {
        public Sort(string property, Direction direction)
        {
            this.Property = property;
            this.Direction = direction;
        }

        public string Property { get; set; }

        public Direction Direction { get; set; }

    }

    public enum Direction
    {
        Asc,
        Desc
    }
}
