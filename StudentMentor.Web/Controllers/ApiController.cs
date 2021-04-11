using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentMentor.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public abstract class ApiController : ControllerBase
    {
    }
}
