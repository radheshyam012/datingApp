using API.Entities;
using Microsoft.IdentityModel.Tokens;

namespace API.Interface
{
    public interface ITokenServices
    {
        
        Task<string> CreateToken(AppUser user);
        
    }
}