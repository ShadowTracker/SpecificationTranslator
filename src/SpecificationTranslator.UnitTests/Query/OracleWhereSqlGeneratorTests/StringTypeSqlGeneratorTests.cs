using SpecificationTranslator.Specifications;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    [TestFixture]
    public class StringTypeSqlGeneratorTests : GeneratorTestBase, IInTests, ILikeTests, ICompareTests
    {
        [Test]
        public void Generate_GenerateFromStartsWith_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Name.StartsWith("M"));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Name LIKE ('M' || '%')", actualSql);
        }

        [Test]
        public void Generate_GenerateFromContains_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Name.Contains("M"));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Name LIKE (('%' || 'M') || '%')", actualSql);
        }

        [Test]
        public void Generate_GenerateFromEndsWith_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Name.EndsWith("M"));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Name LIKE ('%' || 'M')", actualSql);
        }

        [Test]
        public void Generate_GenerateFromIn_ShouldEqualsSqlResult()
        {
            var names = new List<string>() { "Michael", "Lee", "Jacky", "Maggie" };
            var specification = new AnonymousSpecification<UserStub>(v => names.Contains(v.Name));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Name IN ('Michael', 'Lee', 'Jacky', 'Maggie')", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotIn_ShouldEqualsSqlResult()
        {
            var names = new List<string>() { "Michael", "Lee", "Jacky", "Maggie" };
            var specification = new AnonymousSpecification<UserStub>(v => !names.Contains(v.Name));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Name NOT IN ('Michael', 'Lee', 'Jacky', 'Maggie')", actualSql);
        }

        [Test]
        public void Generate_GenerateFromEqualsNullMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Name.Equals(null));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Name IS NULL)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsNullMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => !v.Name.Equals(null));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("NOT ((Name IS NULL))", actualSql);
        }

        [Test]
        public void Generate_GenerateFromEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Name.Equals("M"));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Name = 'M')", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => !v.Name.Equals("M"));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("NOT ((Name = 'M'))", actualSql);
        }

        [Test]
        public void Generate_GenerateFromEqualsNull_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Name == null);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Name IS NULL)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsNull_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Name != null);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Name IS NOT NULL)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Name == "M");
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Name = 'M')", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Name != "M");
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Name <> 'M')", actualSql);
        }

        [Test]
        public void Generate_GenerateFromStringIsNullOrEmptyMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => string.IsNullOrEmpty(v.Name));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Name IS NULL OR (Name = ''))", actualSql);
        }

        [Test]
        public void Generate_GenerateFromStringIsNotNullOrEmptyMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => !string.IsNullOrEmpty(v.Name));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("NOT ((Name IS NULL OR (Name = '')))", actualSql);
        }

        #region Not Support Tests

        public void Generate_GenerateFromGreaterThan_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromGreaterThanOrEqual_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromLessThan_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromLessThanOrEqual_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
