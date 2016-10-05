using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KServices.Core.Domain.Core.Extension;
using MedTeam.Data.Core.Domain.Data;
using MedTeam.Data.Core.Domain.Model.Entities;
using NHibernate;
using NHibernate.Linq;

namespace KServices.Data.Repository
{
    /// <summary>
    ///   Represents repository using <c>NHibernate</c> storing model.
    /// </summary>
    /// <typeparam name="TEntity"> The type of the entity. </typeparam>
    public class Repository<TEntity> : RepositoryBase<TEntity>
        where TEntity : IEntity
    {
        #region Constants and Fields

        /// <summary>
        ///   The <c>NHibernate</c> session.
        /// </summary>
        protected readonly ISession Session;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="Repository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="session"> The session. </param>
        public Repository(ISession session)
        {
            Check.Require<ArgumentNullException>(
                session != null, "Can't create Repository<TEntity> instance because session is null");

            Session = session;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the repository query.
        /// </summary>
        /// <value> The repository query. </value>
        protected override IQueryable<TEntity> RepositoryQuery
        {
            get
            {
                return Session.Query<TEntity>();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Deletes the specified entity.
        /// </summary>
        /// <param name="entity"> The entity. </param>
        public override void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }

        /// <summary>
        /// Deletes entities by the specified query.
        /// </summary>                
        /// <param name="parameters">The parameters which will be used to construct the query. Item1: field name, Item2: operation, Item3: value. If operation is 'in' then value must be a list</param>
        public override int Delete(List<System.Tuple<string, string, dynamic>> parameters)
        {
            Dictionary<string, object> queryParameters = new Dictionary<string, object>();
            string queryString = string.Format("delete {0}", typeof(TEntity));
            if (parameters.Count != 0)
            {
                queryString += " where";
                foreach (System.Tuple<string, string, dynamic> parameter in parameters)
                {
                    if (parameter.Item2 == "in")
                    {
                        int i = 1;
                        string param = string.Empty;
                        foreach (var item in (IEnumerable)parameter.Item3)
                        {
                            string key = string.Format("{0}{1}", parameter.Item1, i);
                            queryParameters.Add(key, item);

                            param += string.Format(":{0},", key);
                            i++;
                        }

                        queryString += string.Format(" {0} {1} ({2}) and", parameter.Item1, parameter.Item2, param.Remove(param.Length - 1));
                    }
                    else
                    {
                        queryString += string.Format(" {0} {1} :{0} and", parameter.Item1, parameter.Item2);
                        queryParameters.Add(parameter.Item1, parameter.Item3);
                    }
                }

                queryString = queryString.Remove(queryString.Length - 4);
            }

            IQuery query = Session.CreateQuery(queryString);
            query = queryParameters.Aggregate(query, (current, parameter) => current.SetParameter(parameter.Key, parameter.Value));

            return query.ExecuteUpdate();
        }

        /// <summary>
        ///   Saves the specified entity.
        /// </summary>
        /// <param name="entity"> The entity. </param>
        public override void Save(TEntity entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public override void Commit()
        {
            Session.Transaction.Commit();
        }

        #endregion
    }
}