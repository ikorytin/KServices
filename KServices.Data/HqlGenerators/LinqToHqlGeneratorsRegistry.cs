using NHibernate.Linq.Functions;

namespace KServices.Data.HqlGenerators
{
    public class LinqToHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry
    {
        public LinqToHqlGeneratorsRegistry()
        {
            this.Merge(new ConcatHqlGenerator());            
        }
    }
}
