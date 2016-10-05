using MedTeam.Data.Core.Domain.Model.Entities;

namespace MedTeam.Sam.Core.Domain.Model.Interfaces
{
    public interface ISimpleEntity : IEntity
    {
        string Name { get; set; }
    }
}