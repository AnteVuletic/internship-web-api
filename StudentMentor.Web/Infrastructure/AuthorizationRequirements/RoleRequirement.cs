using Microsoft.AspNetCore.Authorization;
using StudentMentor.Data.Enums;

namespace StudentMentor.Web.Infrastructure.AuthorizationRequirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public UserRole Role { get; set; }

        public RoleRequirement(UserRole role)
        {
            Role = role;
        }
    }
}
