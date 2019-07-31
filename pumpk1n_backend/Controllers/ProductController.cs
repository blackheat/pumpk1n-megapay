using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pumpk1n_backend.Attributes;
using pumpk1n_backend.Models.TransferModels.Products;
using pumpk1n_backend.Responders;
using pumpk1n_backend.Services.Products;

namespace pumpk1n_backend.Controllers
{
    [ApiController]
    [HandleException]
    [Route("product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// [Internal] Add Product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "InternalUser")]
        [Route("")]
        public async Task<IActionResult> InsertProduct([FromBody] ProductInsertModel model)
        {
            var result = await _productService.AddProduct(model);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Update Product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct(long id, [FromBody] ProductInsertModel model)
        {
            var result = await _productService.UpdateProduct(model, id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Delete Product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            await _productService.DeleteProduct(id);
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }

        /// <summary>
        /// Get specific product details
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var result = await _productService.GetProduct(id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// Get products
        /// </summary>
        /// <param name="startAt">From Nth product</param>
        /// <param name="count">Count</param>
        /// <param name="name">Search by name</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> GetProducts(int startAt = 0, int count = 10, string name = "")
        {
            var result = await _productService.GetProducts(startAt, count, name);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Mark Product As Deprecated
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}/deprecation")]
        public async Task<IActionResult> MarkProductAsDeprecated(long id)
        {
            await _productService.ChangeProductDeprecatedStatus(id, true);
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }

        /// <summary>
        /// [Internal] Mark Product as NOT Deprecated
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}/deprecation")]
        public async Task<IActionResult> UnmarkProductAsDeprecated(long id)
        {
            await _productService.ChangeProductDeprecatedStatus(id, false);
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }
    }
}