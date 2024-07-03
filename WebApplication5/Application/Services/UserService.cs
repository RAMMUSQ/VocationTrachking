using System.Threading.Tasks;
using WebApplication5.Models;
using Infrastructure.Repositories;
using Core.Entities;
using Core.Enums;

namespace WebApplication5.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private IUserService _userServiceImplementation;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(RegisterModel model)
        {
            // Kullanıcı adı kontrolü
            var existingUser = await _userRepository.GetUserByUsernameAsync(model.Username);
            if (existingUser != null)
            {
                return (false, "Username already taken.");
            }

            var user = new User
            {
                Username = model.Username,
                Password = (model.Password),
               // Birthdate = model.Birthdate ,
               /* YearsWorked = model.YearsWorked ,
                Department = model.Department,
                Occupation = model.Occupation,
                Age = model.Age,
                Email = model.Email,*/
                // Parola güvenliği için hashing yapılması önerilir 
                Role = UserRole.User
               
            };

            var result = await _userRepository.AddAsync(user);
            return (result, result ? null : "Failed to register user.");
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            return await _userRepository.GetUserByUsernameAndPasswordAsync(username, password);
        }

        public async Task<bool> SetUserRoleToAdmin(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) return false;

            user.Role = UserRole.Admin;
            return await _userRepository.UpdateAsync(user);
        }
    }
}