namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    public interface ILikeTests
    {
        void Generate_GenerateFromStartsWith_ShouldEqualsSqlResult();

        void Generate_GenerateFromEndsWith_ShouldEqualsSqlResult();

        void Generate_GenerateFromContains_ShouldEqualsSqlResult();

    }
}
