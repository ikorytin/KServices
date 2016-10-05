using System;

using MedTeam.Data.Core.Domain.Model.Entities;

namespace MedTeam.Sam.Core.Domain.Model.Entities
{
    public class FileStorage : BaseEntity
    {
        public virtual Guid FileId { get; set; } 

        public virtual byte[] File { get; set; }

        ////public virtual string ContentType { get; set; }
    }
}