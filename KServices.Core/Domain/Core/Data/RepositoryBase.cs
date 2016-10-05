using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using MedTeam.Data.Core.Domain.Model.Entities;

namespace MedTeam.Data.Core.Domain.Data
{
    /// <summary>
    /// Represents base functionality of entities repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : IEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
        public Type ElementType
        {
            get
            {
                return RepositoryQuery.ElementType;
            }
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Linq.Expressions.Expression"/> that is associated with this instance of <see cref="T:System.Linq.IQueryable"/>.</returns>
        public Expression Expression
        {
            get
            {
                return RepositoryQuery.Expression;
            }
        }

        /// <summary>
        /// Gets the entities finder.
        /// </summary>
        /// <value>The entities finder.</value>
        public virtual IFinder<TEntity> Find
        {
            get
            {
                return new Finder<TEntity>(this);
            }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Linq.IQueryProvider"/> that is associated with this data source.</returns>
        public IQueryProvider Provider
        {
            get
            {
                return RepositoryQuery.Provider;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the repository query.
        /// </summary>
        /// <value>The repository query.</value>
        protected abstract IQueryable<TEntity> RepositoryQuery { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public abstract void Delete(TEntity entity);

        /// <summary>
        /// Deletes entities by the specified query.
        /// </summary>                
        /// <param name="parameters">The parameters which will be used to construct the query. Item1: field name, Item2: operation, Item3: value </param>
        public abstract int Delete(List<Tuple<string, string, dynamic>> parameters);        

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return RepositoryQuery.GetEnumerator();
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public abstract void Save(TEntity entity);

        public abstract void Commit();

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return RepositoryQuery.GetEnumerator();
        }

        #endregion
    }
}