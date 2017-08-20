namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    public interface IInTests
    {
        void Generate_GenerateFromIn_ShouldEqualsSqlResult();

        void Generate_GenerateFromNotIn_ShouldEqualsSqlResult();
    }
}
