using Domain.Core;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
        Task<bool> ValidateTokenAsync(string tokenValue);
        Task SaveTokenAsync(Token token);
    }
}