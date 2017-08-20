namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    public interface ICompareTests
    {
        void Generate_GenerateFromEqualsNullMethodCall_ShouldEqualsSqlResult();

        void Generate_GenerateFromNotEqualsNullMethodCall_ShouldEqualsSqlResult();

        void Generate_GenerateFromEqualsValueMethodCall_ShouldEqualsSqlResult();

        void Generate_GenerateFromNotEqualsValueMethodCall_ShouldEqualsSqlResult();

        void Generate_GenerateFromEqualsNull_ShouldEqualsSqlResult();

        void Generate_GenerateFromNotEqualsNull_ShouldEqualsSqlResult();

        void Generate_GenerateFromEqualsValue_ShouldEqualsSqlResult();

        void Generate_GenerateFromNotEqualsValue_ShouldEqualsSqlResult();

        void Generate_GenerateFromGreaterThan_ShouldEqualsSqlResult();

        void Generate_GenerateFromGreaterThanOrEqual_ShouldEqualsSqlResult();

        void Generate_GenerateFromLessThan_ShouldEqualsSqlResult();

        void Generate_GenerateFromLessThanOrEqual_ShouldEqualsSqlResult();

    }
}
