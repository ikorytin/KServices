using Microsoft.AspNet.Identity;

namespace KServices.Core.Domain.Data.Entities
{
    public class Account : IUser<int>, IEntity
    {
        public virtual int Id { get; set; }

        public virtual string UserName { get; set; }
        
        public virtual string PasswordHash { get; set; }
    }
}