using System.Threading.Tasks;
using WebApplication5.Models;
using Core.Entities;

namespace WebApplication5.Services
{
    public interface IUserService
    {
        Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(RegisterModel model);
        Task<User> ValidateUserAsync(string username, string password);
        Task<bool> SetUserRoleToAdmin(string username);
    }
}