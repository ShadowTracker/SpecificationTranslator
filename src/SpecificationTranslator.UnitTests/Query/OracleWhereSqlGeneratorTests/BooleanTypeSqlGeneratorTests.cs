using SpecificationTranslator.Specifications;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    [TestFixture]
    public class BooleanTypeSqlGeneratorTests : GeneratorTestBase, ICompareTests, IInTests
    {
        [Test]
        public void Generate_GenerateFromEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Enabled == true);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Enabled = 1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Enabled != true);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Enabled <> 1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromIn_ShouldEqualsSqlResult()
        {
            var sexes = new List<bool>() { false, true };
            var specification = new AnonymousSpecification<UserStub>(v => sexes.Contains(v.Enabled));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Enabled IN (0, 1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotIn_ShouldEqualsSqlResult()
        {
            var sexes = new List<bool>() { false, true };
            var specification = new AnonymousSpecification<UserStub>(v => !sexes.Contains(v.Enabled));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Enabled NOT IN (0, 1)", actualSql);
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

        public void Generate_GenerateFromGreaterThanOrEqual_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromGreaterThan_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromLessThanOrEqual_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }


        public void Generate_GenerateFromLessThan_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        public void Generate_GenerateFromNotEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
