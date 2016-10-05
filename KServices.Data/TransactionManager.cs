using System;

using MedTeam.Data.Core.Domain.Data;

using MedTeam.Infrastructure.Utils;

using NHibernate;

namespace MedTeam.DocumentTracking.Data
{
    /// <summary>
    /// Represents transaction manager.
    /// </summary>
    public class TransactionManager : ITransactionManager
    {
        #region Constants and Fields

        private readonly ITransaction _transaction;

        #endregion

        #region Constructors and Destructors

        public TransactionManager(ISession session)
        {
            Check.Require<ArgumentNullException>(
                session != null, "Can't create TransactionManager instance because session is null");
            _transaction = session.Transaction;
        }

        #endregion

        #region Public Properties

        public bool IsActive
        {
            get
            {
                return _transaction.IsActive;
            }
        }

        #endregion

        #region Public Methods

        public void Begin()
        {
            _transaction.Begin();
        }

        public void Commit()
        {
           _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        #endregion
    }
}