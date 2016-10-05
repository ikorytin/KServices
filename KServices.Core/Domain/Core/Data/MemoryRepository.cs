using System;
using System.Collections.Generic;
using System.Linq;

using MedTeam.Data.Core.Domain.Model.Entities;

namespace MedTeam.Data.Core.Domain.Data
{
    public class MemoryRepository<TEntity> : RepositoryBase<TEntity> where TEntity : IEntity
    {
        #region Constants and Fields

        /// <summary>
        /// The repository source.
        /// </summary>
        private ICollection<TEntity> _source;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryRepository&lt;TEntity&gt;"/> class.
        /// </summary>
        public MemoryRepository()
            : this(new List<TEntity>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryRepository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public MemoryRepository(IEnumerable<TEntity> entities)
        {
            SetItems(entities);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the repository query.
        /// </summary>
        /// <value>The repository query.</value>
        protected override IQueryable<TEntity> RepositoryQuery
        {
            get
            {
                return _source.AsQueryable();
            }
        }

        #endregion

        #region Public Methods

        public void SetItems(IEnumerable<TEntity> entities)
        {
            _source = new List<TEntity>(entities);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Delete(TEntity entity)
        {
            _source.Remove(entity);
        }

        public override int Delete(List<Tuple<string, string, dynamic>> parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Save(TEntity entity)
        {
            if (!_source.Contains(entity))
            {
                _source.Add(entity);
            }
        }

        public override void Commit()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}