namespace KServices.Core.Domain.Data.Repositories
{
    public interface ILoginRepository
    {
        string Login(string userName, string password);
    }
}