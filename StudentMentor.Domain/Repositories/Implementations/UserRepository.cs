using System.Linq;
using StudentMentor.Data.Entities;
using StudentMentor.Data.Entities.Models;
using StudentMentor.Domain.Abstractions;
using StudentMentor.Domain.Helpers;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Interfaces;

namespace StudentMentor.Domain.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly StudentMentorDbContext _dbContext;
        private readonly IClaimProvider _claimProvider;

        public UserRepository(StudentMentorDbContext dbContext, IClaimProvider claimProvider)
        {
            _dbContext = dbContext;
            _claimProvider = claimProvider;
        }

        public ResponseResult CheckEmail(string email)
        {
            var isEmailTaken = _dbContext.Users.Any(u => u.Email == email.ToLower().Trim());

            return isEmailTaken
                ? ResponseResult.Error($"{email} is already taken")
                : ResponseResult.Ok;
        }

        public ResponseResult<User> GetUserIfValidCredentials(LoginModel model)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email.ToLower().Trim());
            if (user is null)
                return ResponseResult<User>.Error("Invalid password or email");

            var isValidPassword = EncryptionHelper.ValidatePassword(model.Password, user.Password);
            return isValidPassword
                ? new ResponseResult<User>(user)
                : ResponseResult<User>.Error("Invalid password or email");
        }

        public User GetUser(int userId)
        {
            var user = _dbContext.Users.Find(userId);
            return user;
        }

        public UserModel GetCurrentUserModel()
        {
            var user = _dbContext.Users
                .Where(u => u.Id == _claimProvider.GetUserId())
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .SingleOrDefault();

            return user;
        }
    }
}
