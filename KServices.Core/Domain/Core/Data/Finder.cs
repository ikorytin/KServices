using System;
using System.Collections.Generic;
using System.Linq;
using KServices.Core.Domain.Core.Extension;
using MedTeam.Data.Core.Domain.Model.Entities;

using MedTeam.Infrastructure.Specification;
using MedTeam.Infrastructure.Utils;

namespace MedTeam.Data.Core.Domain.Data
{
    /// <summary>
    /// Represents entities finder.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class Finder<TEntity> : IFinder<TEntity>
        where TEntity : IEntity
    {
        #region Constants and Fields

        /// <summary>
        /// The all entities in which need found something.
        /// </summary>
        private readonly IEnumerable<TEntity> _candidates;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Finder{TEntity}"/> class.
        /// </summary>
        /// <param name="candidates">The entities.</param>
        public Finder(IEnumerable<TEntity> candidates)
        {
            Check.Require<ArgumentNullException>(
                candidates != null,
                $"Can't create instance of {GetType()} because constructor argument candidates is null");

            _candidates = candidates;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Finds the all entities which specified by specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The all specified entities.</returns>
        public IEnumerable<TEntity> All(ISpecification<TEntity> specification)
        {
            Check.Require<ArgumentNullException>(
                specification != null, "Unable gets all entities by specification which is null");

            IEnumerable<TEntity> result = specification.SatisfyingElementsFrom(_candidates);

            return result;
        }

        /// <summary>
        /// Finds the single entity which satisfy of specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The single entity which satisfy of specification.</returns>
        public TEntity One(ISpecification<TEntity> specification)
        {
            Check.Require<ArgumentNullException>(
                specification != null, "Unable gets entity by specification which is null");

            TEntity result = specification.SatisfyingElementsFrom(_candidates).SingleOrDefault();

            return result;
        }

        #endregion
    }
}