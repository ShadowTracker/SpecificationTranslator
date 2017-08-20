using SpecificationTranslator.Specifications;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SpecificationTranslator.UnitTests.Query.OracleWhereSqlGeneratorTests
{
    [TestFixture]
    public class DateTimeTypeSqlGeneratorTests : DateTimeTypeSqlGeneratorTestsBase, ICompareTests, IInTests
    {
        private DateTime _expectedDateTime;
        private string _expectedOracleDateTime;

        public DateTimeTypeSqlGeneratorTests()
        {
            _expectedDateTime = new DateTime(2017, 8, 15);
            _expectedOracleDateTime = GetOracleDateTimeFormat(_expectedDateTime);
        }

        [Test]
        public void Generate_GenerateFromEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.InputDate.Equals(_expectedDateTime));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual($"(InputDate = {_expectedOracleDateTime})", actualSql);
        }

        [Test]
        public void Generate_GenerateFromEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.InputDate == _expectedDateTime);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual($"(InputDate = {_expectedOracleDateTime})", actualSql);
        }

        [Test]
        public void Generate_GenerateFromGreaterThanOrEqual_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.InputDate >= _expectedDateTime);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual($"(InputDate >= {_expectedOracleDateTime})", actualSql);
        }

        [Test]
        public void Generate_GenerateFromGreaterThan_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.InputDate > _expectedDateTime);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual($"(InputDate > {_expectedOracleDateTime})", actualSql);
        }

        [Test]
        public void Generate_GenerateFromLessThanOrEqual_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.InputDate <= _expectedDateTime);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual($"(InputDate <= {_expectedOracleDateTime})", actualSql);
        }

        [Test]
        public void Generate_GenerateFromLessThan_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.InputDate < _expectedDateTime);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual($"(InputDate < {_expectedOracleDateTime})", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsValueMethodCall_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => !v.InputDate.Equals(_expectedDateTime));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual($"NOT ((InputDate = {_expectedOracleDateTime}))", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotEqualsValue_ShouldEqualsSqlResult()
        {
            var specification = new AnonymousSpecification<UserStub>(v => v.InputDate != _expectedDateTime);
            string actualSql = GenerateSql(specification);

            Assert.AreEqual($"(InputDate <> {_expectedOracleDateTime})", actualSql);
        }

        [Test]
        public void Generate_GenerateFromIn_ShouldEqualsSqlResult()
        {
            var dateTime1 = new DateTime(2017, 8, 15, 1, 2, 3);
            var dateTime2 = new DateTime(2017, 8, 15, 4, 5, 6);
            var dateTime3 = new DateTime(2017, 8, 15, 7, 8, 9);
            var inputDateTimes = new List<DateTime>() { dateTime1, dateTime2, dateTime3 };
            var specification = new AnonymousSpecification<UserStub>(v => inputDateTimes.Contains(v.InputDate));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual($"InputDate IN ({GetOracleDateTimeFormat(dateTime1)}, {GetOracleDateTimeFormat(dateTime2)}, {GetOracleDateTimeFormat(dateTime3)})", actualSql);
        }

        [Test]
        public void Generate_GenerateFromNotIn_ShouldEqualsSqlResult()
        {
            var dateTime1 = new DateTime(2017, 8, 15, 1, 2, 3);
            var dateTime2 = new DateTime(2017, 8, 15, 4, 5, 6);
            var dateTime3 = new DateTime(2017, 8, 15, 7, 8, 9);
            var inputDateTimes = new List<DateTime>() { dateTime1, dateTime2, dateTime3 };
            var specification = new AnonymousSpecification<UserStub>(v => !inputDateTimes.Contains(v.InputDate));
            string actualSql = GenerateSql(specification);

            Assert.AreEqual($"InputDate NOT IN ({GetOracleDateTimeFormat(dateTime1)}, {GetOracleDateTimeFormat(dateTime2)}, {GetOracleDateTimeFormat(dateTime3)})", actualSql);
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
