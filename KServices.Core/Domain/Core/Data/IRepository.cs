using System.Collections.Generic;
using System.Linq;

namespace MedTeam.Data.Core.Domain.Data
{
    /// <summary>
    /// Represents entities repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity> : IQueryable<TEntity>
    {
        #region Public Properties

        /// <summary>
        /// Gets the entities finder.
        /// </summary>
        /// <value>The entities finder.</value>
        IFinder<TEntity> Find { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes entities by the specified query.
        /// </summary>                
        /// <param name="parameters">The parameters which will be used to construct the query. Item1: field name, Item2: operation, Item3: value </param>
        int Delete(List<System.Tuple<string, string, dynamic>> parameters);

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        void Save(TEntity entity);

        /// <summary>
        /// Saves the specified entity.
        /// </summary>        
        void Commit();

        #endregion
    }
}