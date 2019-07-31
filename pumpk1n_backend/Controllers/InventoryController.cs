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
        /// <param name="startAt">From Nth product</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "InternalUser")]
        public async Task<IActionResult> GetInventory(int startAt, int count)
        {
            var result = await _inventoryService.GetInventory(startAt, count);
            return ApiResponder.RespondSuccess(result);
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