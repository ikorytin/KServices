using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using KServices.Core.Domain.Core.Specification;
using MedTeam.Data.Core.Domain.Model.Entities;
using MedTeam.Infrastructure.Specification;
using MedTeam.Infrastructure.Utils;

namespace KServices.Core.Domain.Core.Data.Specifications
{
    /// <summary>
    /// Represents all common specifications.
    /// </summary>
    public static class CommonSpecifications
    {
        #region Public Methods

        /// <summary>
        /// Gets the specification for select entity by id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>        
        /// <param name="id">The entities id.</param>
        /// <returns>
        /// The specification for select entity by id.
        /// </returns>
        public static ISpecification<TEntity> ById<TEntity>(int id) where TEntity : BaseEntity
        {
            return new SingleSpecification<TEntity>(e => e.Id == id);
        }

        /// <summary>
        /// Gets the specification for select entity with specified property which started with specified term.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>        
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The term of property value.</param>
        /// <returns>The query specification.</returns>
        public static ISpecification<TEntity> StartedWith<TEntity>(string propertyName, string value)
            where TEntity : BaseEntity
        {
            return new SingleSpecification<TEntity>(e => e.GetValueByProperty<string>(propertyName).StartsWith(value));
        }

        public static ISpecification<TEntity> Where<TEntity>(Expression<Func<TEntity, bool>> matchingCriteria)
            where TEntity : BaseEntity
        {
            return new SingleSpecification<TEntity>(matchingCriteria);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value by property name.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The value of property.</returns>
        private static TProperty GetValueByProperty<TProperty>(this object source, string propertyName)
        {
            IEnumerable<PropertyInfo> properties = Reflector.GetProperties(source.GetType());
            PropertyInfo targetProperty =
                properties.SingleOrDefault(p => p.PropertyType == typeof(TProperty) && p.Name == propertyName && p.CanRead);
            TProperty propertyValue = default(TProperty);
            if (targetProperty != null)
            {
                propertyValue = (TProperty)targetProperty.GetValue(source, null);
            }

            return propertyValue;
        }

        #endregion
    }
}