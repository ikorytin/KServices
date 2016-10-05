using System.Linq;

using MedTeam.Data.Core.Domain.Data;
using MedTeam.Data.Core.Domain.Model.Entities;

namespace MedTeam.Data.Core.Domain.Extensions
{
    public static class QueryableExtensions
    {
        #region Public Methods

        public static IFinder<TEntity> GetFinder<TEntity>(this IQueryable<TEntity> source) where TEntity : IEntity
        {
            return new Finder<TEntity>(source);
        }       

        #endregion
    }
}