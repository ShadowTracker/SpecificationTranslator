using SpecificationTranslator.Query;
using SpecificationTranslator.Specifications;

namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    public abstract class GeneratorTestBase
    {
        protected string GenerateSql<T>(ISpecification<T> specification)
        {
           return new OracleWhereSqlGenerator(specification.AsExpression()).Generate();
        }
    }
}
