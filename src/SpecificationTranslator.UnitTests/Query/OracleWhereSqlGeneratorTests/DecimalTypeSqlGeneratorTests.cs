using SpecificationTranslator.Specifications;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    [TestFixture]
    public class DecimalTypeSqlGeneratorTests : GeneratorTestBase, ICompareTests, IInTests
    {
        [Test]
        public void Generate_GenerateFromEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Balance.Equals(1.1m));
            string actualSql =  GenerateSql(specification);

            Assert.AreEqual("(Balance = 1.1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Balance == 1.1m);
            string actualSql =  GenerateSql(specification);

            Assert.AreEqual("(Balance = 1.1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromGreaterThanOrEqual_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Balance >= 1.1m);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Balance >= 1.1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromGreaterThan_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Balance > 1.1m);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Balance > 1.1)", actualSql);
        }


        [Test]
        public void Generate_GenerateFromLessThanOrEqual_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Balance <= 1.1m);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Balance <= 1.1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromLessThan_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Balance < 1.1m);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("(Balance < 1.1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => !v.Balance.Equals(1.1m));
            string actualSql =  GenerateSql(specification);

            Assert.AreEqual("NOT ((Balance = 1.1))", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.Balance != 1.1m);
            string actualSql =  GenerateSql(specification);

            Assert.AreEqual("(Balance <> 1.1)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromIn_ShouldEqualsSqlResult()
        {
            var balances = new List<decimal>() { 2.32m, 292.22m, 1.222m };
            var specification = new AnonymousSpecification<UserStub>(v => balances.Contains(v.Balance));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Balance IN (2.32, 292.22, 1.222)", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotIn_ShouldEqualsSqlResult()
        {
            var balances = new List<decimal>() { 2.32m, 292.22m, 1.222m };
            var specification = new AnonymousSpecification<UserStub>(v => !balances.Contains(v.Balance));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual("Balance NOT IN (2.32, 292.22, 1.222)", actualSql);
        }

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
    }
}
