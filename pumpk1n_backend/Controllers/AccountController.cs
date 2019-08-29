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
        
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var result = await _accountService.UserLogin(model);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            await _accountService.RegisterAccount(model, UserType.NormalUser);
            return ApiResponder.RespondStatusCode(HttpStatusCode.Created);
        }

        /// <summary>
        /// Get current logged in user's information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("info")]
        public async Task<IActionResult> GetCurrentUserDetails()
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _accountService.GetUserDetails(userId);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Get specific user's information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}/info")]
        public async Task<IActionResult> GetSpecificUserInfo(long id)
        {
            var result = await _accountService.GetUserDetails(id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Get users information
        /// </summary>
        /// <param name="filterModel"></param>
        /// <param name="count">Count</param>
        /// <param name="page">Page number</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "InternalUser")]
        [Route("")]
        public async Task<IActionResult> GetUsers([FromQuery] UserAccountFilterModel filterModel,
            int count = 10, int page = 1)
        {
            var result = await _accountService.GetUsers(page, count, filterModel);
            return ApiResponder.RespondSuccess(result, null, result.GetPaginationData());
        }

        /// <summary>
        /// Resync chain information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("chain/resync")]
        public async Task<IActionResult> ChainResync()
        {
            await _accountService.Resync();
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }
    }
}