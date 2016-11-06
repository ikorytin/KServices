using System;
using System.Collections.Generic;

namespace KServices.Core.Domain.Data.Entities
{
    public class Entity
    {
        public Entity()
                {
                    Owners = new List<PersonShortInfo>();
                }

        public int Id { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public string СontractNumber { get; set; }

       public List<PersonShortInfo> Owners { get; set; }

        public DateTime ContactDate { get; set; }
    }
}