using System.Linq;
using System.Net;
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
    [HandleException]
    [Route("cart")]
    public class CartController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public CartController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        /// <summary>
        /// Create cart for current logged-in user
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> CreateCart()
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _orderService.CreateCart(userId);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Get current logged-in user's cart
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("item")]
        public async Task<IActionResult> GetCurrentCart()
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _orderService.GetCurrentCart(userId);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Add product to cart
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("item")]
        public async Task<IActionResult> AddProductToCart([FromBody] OrderItemTransferModel model)
        {
            var userId = long.Parse(User.Claims.First().Subject.Name);
            var result = await _orderService.AddToCart(userId, model);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Remove product from cart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("item/{id}")]
        public async Task<IActionResult> DeleteCartItem(long id)
        {
            await _orderService.DeleteCartItem(id);
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }

        /// <summary>
        /// Update product cart
        /// </summary>
        /// <param name="id">Cart Item ID</param>
        /// <param name="quantity">Cart Item Quantity</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("item/{id}/{quantity}")]
        public async Task<IActionResult> UpdateCartItemQuantity(long id, long quantity)
        {
            var result = await _orderService.UpdateCartItemQuantity(id, quantity);
            return ApiResponder.RespondSuccess(result);
        }
    }
}