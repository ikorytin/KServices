namespace MedTeam.Data.Core.Domain.Model.Entities
{
    /// <summary>
    /// Represents interface for base entity which has model of storing.
    /// </summary>
    public interface IEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets the entity id.
        /// </summary>
        /// <value>The entity id value.</value>
        int Id { get; }

        #endregion
    }
}