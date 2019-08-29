using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pumpk1n_backend.Attributes;
using pumpk1n_backend.Models.TransferModels.Suppliers;
using pumpk1n_backend.Responders;
using pumpk1n_backend.Services.Suppliers;

namespace pumpk1n_backend.Controllers
{
    [ApiController]
    [HandleException]
    [Route("supplier")]
    public class SupplierController
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        
        /// <summary>
        /// [Internal] Add Supplier
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "InternalUser")]
        [Route("")]
        public async Task<IActionResult> InsertSupplier([FromBody] SupplierInsertModel model)
        {
            var result = await _supplierService.AddSupplier(model);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Update supplier details
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateSupplier(long id, [FromBody] SupplierInsertModel model)
        {
            var result = await _supplierService.UpdateSupplier(model, id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Delete supplier
        /// </summary>
        /// <param name="id">Supplier ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteSupplier(long id)
        {
            await _supplierService.DeleteSupplier(id);
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }

        /// <summary>
        /// [Internal] Get specific supplier details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> GetSupplier(long id)
        {
            var result = await _supplierService.GetSupplier(id);
            return ApiResponder.RespondSuccess(result);
        }

        /// <summary>
        /// [Internal] Get suppliers
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="count">Count</param>
        /// <param name="name">Search by name</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "InternalUser")]
        [Route("")]
        public async Task<IActionResult> GetSuppliers(int page = 1, int count = 10, string name = "")
        {
            var result = await _supplierService.GetSuppliers(page, count, name);
            return ApiResponder.RespondSuccess(result, null, result.GetPaginationData());
        }
        
        /// <summary>
        /// Resync chain information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("chain/resync")]
        public async Task<IActionResult> ChainResync()
        {
            await _supplierService.Resync();
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }
    }
}