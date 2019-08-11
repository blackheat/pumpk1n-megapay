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
        public async Task<IActionResult> Checkout(CustomerInformationCheckoutTransferModel model)
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _orderService.Checkout(userId, model);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Confirm order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("confirmation")]
        public async Task<IActionResult> ConfirmOrder(long orderId)
        {
            var result = await _orderService.ConfirmOrder(orderId);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Cancel order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("cancellation")]
        public async Task<IActionResult> CancelOrder(long orderId)
        {
            var result = await _orderService.CancelOrder(orderId);
            return ApiResponder.RespondSuccess(result);
        }
    }
}