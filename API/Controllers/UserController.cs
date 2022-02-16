using API.Attributes;
using API.Models;
using API.Services;
using API.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            this._userService = userService;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<object>> GetToken([FromBody] string username)
        {
            User user = await this._userService.GetUser(username);
            if (user == null)
            {
                return NotFound("User does not exist");
            }
        
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(Claims.User, user.Username));
            claims.Add(new Claim(Claims.Roles, string.Join(",", user.Roles)));
            return new { Token = Token.IssueToken(claims, this._configuration).ToString() };
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(UserRoles.Admin)]
        public async Task<ActionResult<string>> DoAdminAction()
        {
            return "Performed Admin Action";
        }

        [HttpGet]
        [Route("dev-or-admin")]
        [Authorize(UserRoles.Admin, UserRoles.Developer)]
        public async Task<ActionResult<string>> DoAdminOrDevAction()
        {
            return "Performed Admin or Developer Action";
        }

        [HttpGet]
        [Route("accountant")]
        [Authorize(UserRoles.Accountant)]
        public async Task<ActionResult<string>> DoAccountantAction()
        {
            return "Performed Accountant Action";
        }

        [HttpGet]
        [Route("any-role")]
        [Authorize]
        public async Task<ActionResult<string>> DoAnyAction()
        {
            return "Performed Action Without Role Restrictions";
        }
    }
}
