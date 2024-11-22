using Application.Interfaces;
using Domain.Core;
using Domain.Enums;
using Infrastructure.Data;
using Presentation.Models;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        //private IUserService _userServiceImplementation;
        private readonly ApplicationDbContext _context;

        public UserService(IUserRepository userRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> RegisterAsync(RegisterModel model)
        { 
            if (_userRepository == null)
            {
                throw new InvalidOperationException("User repository is not initialized.");
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Register model cannot be null.");
            }
            if (_context == null)
            {
                throw new InvalidOperationException("Database context is not initialized.");
            }
            // Kullanıcı adı kontrolü
            var existingUser = await _userRepository.GetUserByUsernameAsync(model.Username);
            if (existingUser != null)
            {
                return (false, "Username already taken.");
            }

            var user = new User
            {
                Username = model.Username,
                Password = model.Password, // Parola güvenliği için hashing yapılması önerilir 
                Birthdate = model.Birthdate,
                Department = model.Department,
                Occupation = model.Occupation,
                StartDateForworks = model.StartDateForworks,
                Age = model.Age,
                Email = model.Email,
                Role = UserRole.User // KURALLAR GELECEK
            };

            var result = await _userRepository.AddAsync(user);

            if (result)
            {
                // Kullanıcının işe başlama tarihinden itibaren geçen yılı hesapla
                var totalDaysWorked = (DateTime.UtcNow - user.StartDateForworks).TotalDays;
                int yearsWorked = (int)(totalDaysWorked / 365);
                int leaveDays = yearsWorked * 14; // Her yıl için 14 gün

                var leaveRight = new AnnualLeaveRight
                {
                    UserId = user.Id,
                    LeaveDays = leaveDays
                };

                _context.AnnualLeaveRights.Add(leaveRight);
                await _context.SaveChangesAsync();

                return (true, null);
            }

            return (false, "Failed to register user.");

            return (false, "Failed to register user.");
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