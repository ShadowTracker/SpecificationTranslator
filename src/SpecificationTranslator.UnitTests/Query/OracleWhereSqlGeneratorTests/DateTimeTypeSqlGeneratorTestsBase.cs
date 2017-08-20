using SpecificationTranslator.Query;
using System;

namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    public abstract class DateTimeTypeSqlGeneratorTestsBase : GeneratorTestBase
    {

        protected string GetOracleDateTimeFormat(DateTime dateTime)
        {
            return new OracleSqlGenerationHelper().GenerateLiteralValue(dateTime);
        }
    }
}
