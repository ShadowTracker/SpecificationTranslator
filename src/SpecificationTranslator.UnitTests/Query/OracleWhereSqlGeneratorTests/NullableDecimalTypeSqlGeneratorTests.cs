using SpecificationTranslator.Specifications;
using NUnit.Framework;
using System.Collections.Generic;

namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    [TestFixture]
    public class NullableDecimalTypeSqlGeneratorTests : GeneratorTestBase, ICompareTests, IInTests
    {

        [Test]
        public void Generate_GenerateFromEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Point == 1.1m);
            string actualSql =  GenerateSql(specification);

            Assert.AreEqual("(Point = 1.1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromGreaterThanOrEqual_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Point >= 1.1m);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Point >= 1.1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromGreaterThan_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Point > 1.1m);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Point > 1.1)", actualSql);
        }


        [Test]
        public void Generate_GenerateFromLessThanOrEqual_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Point <= 1.1m);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Point <= 1.1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromLessThan_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Point < 1.1m);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Point < 1.1)", actualSql);
        }


        [Test]
        public void Generate_GenerateFromNotEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Point != 1.1m);
            string actualSql =  GenerateSql(specification);

            Assert.AreEqual("(Point <> 1.1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromIn_ShouldEqualsSqlResult()
        {
            var balances = new List<decimal?>() { 2.32m, 292.22m, 1.222m };
            var specification = new AnonymousSpecification<UserStub>(v => balances.Contains(v.Point));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Point IN (2.32, 292.22, 1.222)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotIn_ShouldEqualsSqlResult()
        {
            var balances = new List<decimal?>() { 2.32m, 292.22m, 1.222m };
            var specification = new AnonymousSpecification<UserStub>(v => !balances.Contains(v.Point));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Point NOT IN (2.32, 292.22, 1.222)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsNull_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Point != null);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Point IS NOT NULL)", actualSql);
        }


        [Test]
        public void Generate_GenerateFromEqualsNull_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Point == null);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Point IS NULL)", actualSql);
        }

        #region Not Support Tests

        public void Generate_GenerateFromNotEqualsNullMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => !v.Point.Equals(null));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("NOT (Point IS NULL)", actualSql);
        }

        public void Generate_GenerateFromEqualsNullMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Point.Equals(null));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Point IS NULL)", actualSql);
        }

        public void Generate_GenerateFromEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Point.Equals(1.1m));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Point = 1.1)", actualSql);
        }

        public void Generate_GenerateFromNotEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => !v.Point.Equals(1.1m));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("NOT ((Point = 1.1))", actualSql);
        }

        #endregion

    }
}
