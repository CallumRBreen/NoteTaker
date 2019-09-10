using System.Threading.Tasks;
using NoteTaker.Core.Models;

namespace NoteTaker.Core.Services.Interfaces
{
    public interface IUsersService
    {
        Task<AuthenticatedUser> AuthenticateAsync(string username, string password);
    }
}
