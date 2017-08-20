using System.Linq.Expressions;

namespace SpecificationTranslator.Query
{
    public class OracleWhereSqlGenerator : WhereSqlGenerator
    {
        public OracleWhereSqlGenerator(Expression expression) : base(new OracleSqlGenerationHelper(), expression)
        {

        }

        protected override string ConcatOperator => "||";

    }
}
