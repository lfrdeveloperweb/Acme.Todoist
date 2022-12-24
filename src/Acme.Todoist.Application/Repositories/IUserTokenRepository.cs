using Acme.Todoist.Domain.Security;
using System.Threading.Tasks;

namespace Acme.Todoist.Application.Repositories;

/// <summary>
/// Repository to handle information about <see cref="UserToken"/>.
/// </summary>
public interface IUserTokenRepository
{
    Task<UserToken<TUserTokenData>> GetAsync<TUserTokenData>(string userId, UserTokenType type, string value)
        where TUserTokenData : IUserTokenData;
    
    Task<bool> ExistsAsync(string userId, UserTokenType type, string value);

    Task CreateAsync<TUserTokenData>(UserToken<TUserTokenData> userToken) where TUserTokenData : IUserTokenData;

    Task DeleteTokenAsync(string userId, UserTokenType tokenType);

    /*
    
    Task CreateRefreshTokenAsync(RefreshToken refreshToken);

    Task<bool> RefreshTokenIsValidByTokenAndDateAsync(string token, DateTime currentDate);
    
    Task<RefreshToken> GetRefreshTokenByTokenAsync(string token);
    
    Task<RefreshToken> GetRefreshTokenByMembershipIdAsync(string membershipId, DateTime currentDate);
    
    Task DeleteRefreshTokenByMembershipIdAsync(string membershipId);

    */
}