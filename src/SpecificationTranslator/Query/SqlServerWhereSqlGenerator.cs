using System.Linq.Expressions;

namespace SpecificationTranslator.Query
{
    public class SqlServerWhereSqlGenerator : WhereSqlGenerator
    {
        public SqlServerWhereSqlGenerator(Expression expression) : base(new SqlServerSqlGenerationHelper(), expression)
        {

        }

        protected override string ConcatOperator => "+";

    }
}
