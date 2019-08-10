using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pumpk1n_backend.Attributes;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models.TransferModels.Tokens;
using pumpk1n_backend.Responders;
using pumpk1n_backend.Services.Tokens;

namespace pumpk1n_backend.Controllers
{
    [ApiController]
    [HandleException]
    [Route("token")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Get current logged-in user balance
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("balance")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserBalance()
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _tokenService.GetUserBalance(userId);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Get specific user's balance
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("balance/user/{id}")]
        [Authorize(Roles = "InternalUser")]
        public async Task<IActionResult> GetSpecificUserBalance(long id)
        {
            var result = await _tokenService.GetUserBalance(id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Create token purchase request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("transaction/request")]
        [Authorize]
        public async Task<IActionResult> CreateTokenPurchaseRequest([FromBody] TokenTransactionInsertModel model)
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _tokenService.CreateTokenPurchaseRequest(userId, model);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Cancel token purchase request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("transaction/request/{id}")]
        [Authorize]
        public async Task<IActionResult> CancelTokenPurchaseRequest(long id)
        {
            var result = await _tokenService.CancelTokenPurchaseRequest(id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Get token purchase requests
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="page">Page number</param>
        /// <returns></returns>
        [HttpGet]
        [Route("transaction/request")]
        [Authorize]
        public async Task<IActionResult> GetUserTokenPurchaseRequests(int count = 10, int page = 1)
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _tokenService.GetUserTokenPurchaseRequests(userId, count, page);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Get user's specific token purchase request
        /// </summary>
        /// <param name="id">Request ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("transaction/request/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserTokenPurchaseRequest(long id)
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _tokenService.GetUserTokenPurchaseRequest(userId, id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Create invoice for token purchase request
        /// </summary>
        /// <param name="id">Request ID</param>
        /// <returns></returns>
        [HttpPut]
        [Route("balance/request/{id}/billing")]
        [Authorize]
        public async Task<IActionResult> CreateUserTokenPurchaseRequestBilling(long id)
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _tokenService.CreateUserBilling(userId, id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Create token transaction for specific user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="model"></param>
        /// <param name="transactionType">Transaction type</param>
        /// <returns></returns>
        [HttpPost]
        [Route("transaction/user/{id}/request")]
        [Authorize(Roles = "InternalUser")]
        public async Task<IActionResult> CreateSpecificUserTokenTransaction(long userId, [FromBody] TokenTransactionInsertModel model, TokenTransactionType transactionType)
        {
            var result = await _tokenService.CreateTokenTransaction(userId, model, transactionType);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [CoinGate] Process CoinGate Payment web-hook
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("coingate/hook")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> ProcessCoinGateHook([FromForm] CoinGateHookTransferModel model)
        {
            foreach (var key in Request.Form.Keys)
            {
                switch (key)
                {
                    case "order_id":
                        model.OrderId = long.Parse(Request.Form[key]);
                        break;
                    case "status":
                        model.Status = Request.Form[key];
                        break;
                    case "id":
                        model.Id = long.Parse(Request.Form[key]);
                        break;
                    case "price_amount":
                        model.PriceAmount = float.Parse(Request.Form[key]);
                        break;
                    case "price_currency":
                        model.PriceCurrency = Request.Form[key];
                        break;
                    case "receive_amount":
                        model.ReceiveAmount = float.Parse(Request.Form[key]);
                        break;
                    case "receive_currency":
                        model.ReceiveCurrency = Request.Form[key];
                        break;
                    case "pay_amount":
                        model.PayAmount = float.Parse(Request.Form[key]);
                        break;
                    case "pay_currency":
                        model.PayCurrency = Request.Form[key];
                        break;
                    case "created_at":
                        model.CreatedAt = DateTime.Parse(Request.Form[key]);
                        break;
                    case "token":
                        model.Token = Request.Form[key];
                        break;
                }
            }

            await _tokenService.ProcessCoinGateHook(model);
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }
    }
}