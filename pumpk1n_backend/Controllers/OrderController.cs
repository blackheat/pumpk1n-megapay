using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pumpk1n_backend.Attributes;
using pumpk1n_backend.Models.TransferModels.Orders;
using pumpk1n_backend.Responders;
using pumpk1n_backend.Services.Orders;

namespace pumpk1n_backend.Controllers
{
    [ApiController]
    [Route("order")]
    [HandleException]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Checkout
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("checkout")]
        public async Task<IActionResult> Checkout(CheckoutTransferModel model)
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _orderService.Checkout(userId, model);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Confirm order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}/confirmation")]
        public async Task<IActionResult> ConfirmOrder(long id)
        {
            var result = await _orderService.ConfirmOrder(id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Cancel order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}/cancellation")]
        public async Task<IActionResult> CancelOrder(long id)
        {
            var result = await _orderService.CancelOrder(id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Get current user's orders
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> GetUserOrders(int page = 1, int count = 10)
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _orderService.GetUserOrders(userId, page, count);
            return ApiResponder.RespondSuccess(result, null, result.GetPaginationData());
        }

        /// <summary>
        /// [Internal] Get specific user's orders
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="page">Page number</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "InternalUser")]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetSpecificUserOrders(long userId, int page = 1, int count = 10)
        {
            var result = await _orderService.GetUserOrders(userId, page, count);
            return ApiResponder.RespondSuccess(result, null, result.GetPaginationData());
        }

        /// <summary>
        /// [Internal] Get all user's orders
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "InternalUser")]
        [Route("all")]
        public async Task<IActionResult> GetAllUsersOrders(int page = 1, int count = 10)
        {
            var result = await _orderService.GetOrders(page, count);
            return ApiResponder.RespondSuccess(result, null, result.GetPaginationData());
        }

        /// <summary>
        /// Get current user's specific order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("current/{id}")]
        public async Task<IActionResult> GetUserOrder(long id)
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _orderService.GetUserOrder(userId, id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Get specific order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> GetOrder(long id)
        {
            var result = await _orderService.GetOrder(id);
            return ApiResponder.RespondSuccess(result);
        }
    }
}