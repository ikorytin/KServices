using System.Collections.Generic;

using MedTeam.Infrastructure.Specification;

namespace MedTeam.Data.Core.Domain.Data
{
    /// <summary>
    /// Represents entities finder.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IFinder<TEntity>
    {
        #region Public Methods

        /// <summary>
        /// Finds the all entities which satisfy of specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The all specified entities.</returns>
        IEnumerable<TEntity> All(ISpecification<TEntity> specification);

        /// <summary>
        /// Finds the single entity which satisfy of specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The single entity which satisfy of specification.</returns>
        TEntity One(ISpecification<TEntity> specification);

        #endregion
    }
}