using System;
using KServices.Core.Domain.Core.Data.Specifications;
using KServices.Core.Domain.Core.Extension;
using MedTeam.Data.Core.Domain.Data;
using MedTeam.Data.Core.Domain.Model.Entities;

namespace KServices.Core.Domain.Core.Data
{
    /// <summary>
    /// Represents extensions of <see cref="Finder{TEntity}"/>.
    /// </summary>
    public static class FinderExtensions
    {
        #region Public Methods

        /// <summary>
        /// Find by the id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>        
        /// <param name="finder">The finder.</param>
        /// <param name="id">The entity id.</param>
        /// <returns>The entity with specified id.</returns>
        public static TEntity ById<TEntity>(this IFinder<TEntity> finder, int id) where TEntity : BaseEntity
        {
            Check.Require<ArgumentNullException>(
                finder != null, "Unable find entity by id because argument finder is null");

            TEntity entity = finder.One(CommonSpecifications.ById<TEntity>(id));

            return entity;
        }

        #endregion
    }
}