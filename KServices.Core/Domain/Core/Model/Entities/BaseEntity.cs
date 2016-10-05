namespace MedTeam.Data.Core.Domain.Model.Entities
{
    /// <summary>
    ///   Base entities for all mapping.
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the id.
        /// </summary>
        /// <value> The id value. </value>
        public virtual int Id { get; set; }

        #endregion
    }
}