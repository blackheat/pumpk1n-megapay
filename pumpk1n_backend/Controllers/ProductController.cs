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

        [HttpPost]
        [Authorize(Roles = "InternalUser")]
        [Route("")]
        public async Task<IActionResult> InsertProduct([FromBody] ProductInsertModel model)
        {
            var result = await _productService.AddProduct(model);
            return ApiResponder.RespondSuccess(result);
        }

        [HttpPatch]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct(long id, [FromBody] ProductInsertModel model)
        {
            var result = await _productService.UpdateProduct(model, id);
            return ApiResponder.RespondSuccess(result);
        }

        [HttpDelete]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            await _productService.DeleteProduct(id);
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var result = await _productService.GetProduct(id);
            return ApiResponder.RespondSuccess(result);
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> GetProducts(int startAt = 0, int count = 10, string name = "")
        {
            var result = await _productService.GetProducts(startAt, count, name);
            return ApiResponder.RespondSuccess(result);
        }

        [HttpPut]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}/deprecation")]
        public async Task<IActionResult> MarkProductAsDeprecated(long id)
        {
            await _productService.ChangeProductDeprecatedStatus(id, true);
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }

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