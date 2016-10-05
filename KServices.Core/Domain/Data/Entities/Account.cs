using MedTeam.Data.Core.Domain.Model.Entities;
using Microsoft.AspNet.Identity;

namespace KServices.Core.Domain.Data.Entities
{
    public class Account : IEntity
    {
        public virtual int Id { get; set; }

        public virtual string AccountNumber { get; set; }
        
        public virtual string PasswordHash { get; set; }
    }
}