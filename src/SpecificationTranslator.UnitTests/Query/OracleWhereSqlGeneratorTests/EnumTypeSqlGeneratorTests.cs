using SpecificationTranslator.Specifications;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    [TestFixture]
    public class EnumTypeSqlGeneratorTests : GeneratorTestBase, ICompareTests, IInTests
    {

        [Test]
        public void Generate_GenerateFromEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Sex == SexStub.Male);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Sex = 1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromGreaterThanOrEqual_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Sex >= SexStub.Male);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Sex >= 1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromGreaterThan_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Sex > SexStub.Male);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Sex > 1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromLessThanOrEqual_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Sex <= SexStub.Male);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Sex <= 1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromLessThan_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Sex < SexStub.Male);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Sex < 1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Sex != SexStub.Male);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Sex <> 1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromIn_ShouldEqualsSqlResult()
        {
            var sexes = new List<SexStub>() { SexStub.Male, SexStub.Female };
            var specification = new AnonymousSpecification<UserStub>(v => sexes.Contains(v.Sex));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Sex IN (1, 2)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotIn_ShouldEqualsSqlResult()
        {
            var sexes = new List<SexStub>() { SexStub.Male, SexStub.Female };
            var specification = new AnonymousSpecification<UserStub>(v => !sexes.Contains(v.Sex));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Sex NOT IN (1, 2)", actualSql);
        }

        #region Not Support Tests

        public void Generate_GenerateFromNotEqualsNullMethodCall_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromNotEqualsNull_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromEqualsNullMethodCall_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromEqualsNull_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Sex.Equals(SexStub.Male));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Sex = 1)", actualSql);
        }

        public void Generate_GenerateFromNotEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => !v.Sex.Equals(SexStub.Male));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("NOT ((Sex = 1))", actualSql);
        }

        #endregion

    }
}
