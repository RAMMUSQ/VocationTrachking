using Domain.Core;
using Presentation.Models;


namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(RegisterModel model);
        Task<User> ValidateUserAsync(string username, string password);
        Task<bool> SetUserRoleToAdmin(string username);
    }
}