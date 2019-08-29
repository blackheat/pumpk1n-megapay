using System.Threading.Tasks;
using pumpk1n_backend.Models.ChainReturnModels.Suppliers;
using pumpk1n_backend.Models.ChainTransferModels.Suppliers;

namespace pumpk1n_backend.Services.Suppliers
{
    public interface ISupplierChainService
    {
        Task AddSupplier(ChainSupplierTransferModel model);
        Task<ChainSupplierReturnModel> GetSupplier(long supplierId);
        Task DeleteSupplier(long supplierId);
    }
}