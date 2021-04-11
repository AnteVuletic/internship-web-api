using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;

namespace StudentMentor.Domain.Repositories.Interfaces
{
    public interface IUserRepository
    {
        ResponseResult CheckEmail(string email);
        ResponseResult<User> GetUserIfValidCredentials(LoginModel model);
        User GetUser(int userId);
        UserModel GetCurrentUserModel();
    }
}
