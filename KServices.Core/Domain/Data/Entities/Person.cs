using System;

namespace KServices.Core.Domain.Data.Entities
{
    public class Person
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Address { get; set; }

        public string MobilePhone1 { get; set; }

        public string MobilePhone2 { get; set; }

        public string MobilePhone3 { get; set; }

        public string HomePhone { get; set; }

        public DateTime ContactDate { get; set; }
    }
}