using SpecificationTranslator.Query;
using SpecificationTranslator.Specifications;
using NUnit.Framework;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Moq;

namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    [TestFixture]
    public class SqlGeneratorTests: GeneratorTestBase
    {
        [Test]
        public void Generate_GenerateAnySpecification_ReturnsAlwaysTrueClause()
        {
            var specification = new AnySpecification<EntityStub>();
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("1 = 1", actualSql);
        }

        [Test]
        public void Generate_GenerateNoneSpecification_ReturnsAlwaysFalseClause()
        {
            var specification = new NoneSpecification<EntityStub>();
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("0 = 1", actualSql);
        }

        [Test]
        public void Generate_ValidatePropertyMap_MatchedColumnName()
        {
            var propertyInfo = typeof(EntityStub).GetProperty("UserName");
            var columnName = propertyInfo.GetCustomAttribute<ColumnAttribute>().Name;
            var specification = new AnySpecification<EntityStub>();
            string actualSql = GenerateSql(specification);

            string actualColumnName = new OracleWhereSqlGenerator(specification.AsExpression()).GetColumnName(propertyInfo);

            Assert.AreEqual(columnName, actualColumnName);
        }
    }

    public class EntityStub
    {
        [Column("USER_NAME")]
        public string UserName { get; set; }
    }
}
