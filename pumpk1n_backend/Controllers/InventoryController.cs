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
        /// [Internal] Get inventory list
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
        /// [Internal] Get inventory item details
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "InternalUser")]
        public async Task<IActionResult> GetInventoryItem(long id)
        {
            var result = await _inventoryService.GetInventoryItem(id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Import a product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("imported")]
        [Authorize(Roles = "InternalUser")]
        public async Task<IActionResult> ImportProducts([FromBody] InventoryImportModel model)
        {
            var result = await _inventoryService.ImportProducts(model);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Export one or more products
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("exported")]
        [Authorize(Roles = "InternalUser")]
        public async Task<IActionResult> ExportProducts([FromBody] InventoryExportModel model)
        {
            var result = await _inventoryService.ExportProducts(model);
            return ApiResponder.RespondSuccess(result);
        }
    }
}