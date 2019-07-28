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
        
        [HttpPost]
        [Authorize(Roles = "InternalUser")]
        [Route("")]
        public async Task<IActionResult> InsertSupplier([FromBody] SupplierInsertModel model)
        {
            var result = await _supplierService.AddSupplier(model);
            return ApiResponder.RespondSuccess(result);
        }

        [HttpPatch]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateSupplier(long id, [FromBody] SupplierInsertModel model)
        {
            var result = await _supplierService.UpdateSupplier(model, id);
            return ApiResponder.RespondSuccess(result);
        }

        [HttpDelete]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteSupplier(long id)
        {
            await _supplierService.DeleteSupplier(id);
            return ApiResponder.RespondStatusCode(HttpStatusCode.OK);
        }

        [HttpGet]
        [Authorize(Roles = "InternalUser")]
        [Route("{id}")]
        public async Task<IActionResult> GetSupplier(long id)
        {
            var result = await _supplierService.GetSupplier(id);
            return ApiResponder.RespondSuccess(result);
        }

        [HttpGet]
        [Authorize(Roles = "InternalUser")]
        [Route("")]
        public async Task<IActionResult> GetSuppliers(int startAt = 0, int count = 10, string name = "")
        {
            var result = await _supplierService.GetSuppliers(startAt, count, name);
            return ApiResponder.RespondSuccess(result);
        }
    }
}