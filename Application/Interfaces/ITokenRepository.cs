// Infrastructure/Repositories/ITokenRepository.cs

using Domain.Core;

namespace Application.Interfaces
{
    public interface ITokenRepository
    {
        Task AddAsync(Token token);
        Task<Token> CreateTokenAsync(Token token);
        Task<Token> GetTokenAsync(string tokenValue);
    }
}