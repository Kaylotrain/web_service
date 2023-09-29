using web_service.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
        
    }
}