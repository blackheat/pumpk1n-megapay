using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pumpk1n_backend.Attributes;
using pumpk1n_backend.Models.TransferModels.Inventories;
using pumpk1n_backend.Responders;
using pumpk1n_backend.Services.Inventories;

namespace pumpk1n_backend.Controllers
{
    [ApiController]
    [Route("inventory")]
    [HandleException]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Get inventory list
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "InternalUser")]
        public async Task<IActionResult> GetInventory(int page = 1, int count = 10)
        {
            var result = await _inventoryService.GetInventory(page, count);
            return ApiResponder.RespondSuccess(result, null, result.GetPaginationData());
        }

        /// <summary>
        /// Import a product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "InternalUser")]
        public async Task<IActionResult> ImportProduct(InventoryImportModel model)
        {
            var result = await _inventoryService.ImportProduct(model);
            return ApiResponder.RespondSuccess(result);
        }
    }
}