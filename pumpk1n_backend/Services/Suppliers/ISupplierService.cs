using System.Threading.Tasks;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.ReturnModels.Suppliers;
using pumpk1n_backend.Models.TransferModels.Suppliers;

namespace pumpk1n_backend.Services.Suppliers
{
    public interface ISupplierService
    {
        Task Resync();
        Task<SupplierReturnModel> AddSupplier(SupplierInsertModel model);
        Task<SupplierReturnModel> UpdateSupplier(SupplierInsertModel model, long supplierId);
        Task DeleteSupplier(long supplierId);
        Task<SupplierReturnModel> GetSupplier(long supplierId);
        Task<CustomList<SupplierReturnModel>> GetSuppliers(int page, int count, string name = "");
    }
}