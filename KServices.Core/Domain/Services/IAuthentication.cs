namespace KServices.Core.Domain.Services
{
   public interface IAuthentication
    {
        bool Authenticate(string account, string passwordHesh);

       bool Authenticate(string account);
    }
}
