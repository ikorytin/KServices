using Microsoft.AspNet.Identity;

namespace KServices.Models
{
    public class AccountModel : IUser
    {
        private readonly string _id;
        private string _userName;

        public AccountModel() { }

        public AccountModel(string id, string userName)
        {
            _id = id;
            _userName = userName;
        }

        public string Id
        {
            get { return _id; }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }
    }
}