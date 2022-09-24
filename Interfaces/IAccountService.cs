using dotnet_hero.Entities;

namespace dotnet_hero.Interfaces
{
    public interface IAccountService
    {
        Task Register(Account account);
        Task Login(string username, string password);

    }
}
