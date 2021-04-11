using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentMentor.Domain.Models.ViewModels;
using StudentMentor.Domain.Models.ViewModels.Account;
using StudentMentor.Domain.Repositories.Interfaces;
using StudentMentor.Domain.Services.Interfaces;

namespace StudentMentor.Web.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IJwtService _jwtService;

        public AccountController(IUserRepository userRepository, IStudentRepository studentRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost(nameof(RegisterStudent))]
        public ActionResult<string> RegisterStudent(RegistrationModel model)
        {
            var checkMailResponse = _userRepository.CheckEmail(model.Email);
            if (checkMailResponse.IsError)
                return BadRequest(checkMailResponse.Message);

            var registerStudentResult = _studentRepository.RegisterStudent(model);
            if (registerStudentResult.IsError)
                return BadRequest(registerStudentResult.Message);

            var token = _jwtService.GetJwtTokenForUser(registerStudentResult.Data);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public ActionResult<string> Login(LoginModel model)
        {
            var result = _userRepository.GetUserIfValidCredentials(model);
            if (result.IsError)
                return BadRequest(result.Message);

            var user = result.Data;
            var token = _jwtService.GetJwtTokenForUser(user);
            return Ok(token);
        }

        [HttpGet]
        public ActionResult<UserModel> GetCurrentUserModel()
        {
            var user = _userRepository.GetCurrentUserModel();
            if (user == null)
                return Unauthorized();

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet(nameof(RefreshToken))]
        public ActionResult<string> RefreshToken([FromQuery] string token)
        {
            var newToken = _jwtService.GetNewToken(token);

            return Ok(newToken);
        }
    }
}
