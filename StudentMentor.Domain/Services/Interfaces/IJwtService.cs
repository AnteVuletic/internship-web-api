using StudentMentor.Data.Entities.Models;

namespace StudentMentor.Domain.Services.Interfaces
{
    public interface IJwtService
    {
        string GetJwtTokenForUser(User user);
        string GetNewToken(string token);
    }
}
