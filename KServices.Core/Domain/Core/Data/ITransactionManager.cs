using System;

namespace MedTeam.Data.Core.Domain.Data
{
    /// <summary>
    /// Represents interface for managing transaction.
    /// </summary>
    public interface ITransactionManager : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Indicates if transaction active
        /// </summary>
        bool IsActive { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Begins transaction.
        /// </summary>
        void Begin();

        /// <summary>
        /// Commits transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks transaction.
        /// </summary>
        void Rollback();

        #endregion
    }
}