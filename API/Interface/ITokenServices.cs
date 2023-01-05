using API.Entities;
using Microsoft.IdentityModel.Tokens;

namespace API.Interface
{
    public interface ITokenServices
    {
        
        string CreateToken(AppUser user);
        
    }
}