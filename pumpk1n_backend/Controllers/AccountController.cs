using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pumpk1n_backend.Attributes;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models.TransferModels.Accounts;
using pumpk1n_backend.Responders;
using pumpk1n_backend.Services.Accounts;

namespace pumpk1n_backend.Controllers
{
    [ApiController]
    [HandleException]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var result = await _accountService.UserLogin(model);
            return ApiResponder.RespondSuccess(result);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            await _accountService.RegisterAccount(model, UserType.NormalUser);
            return ApiResponder.RespondStatusCode(HttpStatusCode.Created);
        }

        [HttpGet]
        [Authorize]
        [Route("info")]
        public async Task<IActionResult> GetCurrentUserDetails()
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _accountService.GetUserDetails(userId);
            return ApiResponder.RespondSuccess(result);
        }
    }
}