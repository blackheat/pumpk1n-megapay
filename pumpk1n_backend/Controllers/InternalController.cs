using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pumpk1n_backend.Attributes;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models.TransferModels.Accounts;
using pumpk1n_backend.Responders;
using pumpk1n_backend.Services.Accounts;
using pumpk1n_backend.Services.InternalStuffs;

namespace pumpk1n_backend.Controllers
{
    [ApiController]
    [HandleException]
    [Route("internal")]
    public class InternalController
    {
        private readonly IAccountService _accountService;
        private readonly IInternalService _internalService;
        private const string InternalHailMaryToken = "E49BNgyVUfFdhUYyB8J29kfnxcPPjZRV";

        public InternalController(IAccountService accountService, IInternalService internalService)
        {
            _accountService = accountService;
            _internalService = internalService;
        }

        [HttpPost]
        [Route("account/register")]
        public async Task<IActionResult> RegisterInternalAccount([FromBody] UserRegisterModel model, [FromHeader(Name = "X-Internal-Hail-Mary-Token")] string internalToken)
        {
            if (!internalToken.Equals(InternalHailMaryToken))
                return ApiResponder.RespondStatusCode(HttpStatusCode.Unauthorized);
            await _accountService.RegisterAccount(model, UserType.InternalUser);
            return ApiResponder.RespondStatusCode(HttpStatusCode.Created);
        }

        [HttpPut]
        [Authorize(Roles = "InternalUser")]
        [Route("account/{id}/role/{roleId}")]
        public async Task<IActionResult> ChangeAccountRole(long id, UserType roleId)
        {
            await _internalService.ChangeAccountRole(id, roleId);
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }
    }
}