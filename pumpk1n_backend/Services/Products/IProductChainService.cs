using System.Threading.Tasks;
using pumpk1n_backend.Models.ChainReturnModels.Products;
using pumpk1n_backend.Models.ChainTransferModels.Products;

namespace pumpk1n_backend.Services.Products
{
    public interface IProductChainService
    {
        Task AddProduct(ChainProductTransferModel model);
        Task<ChainProductReturnModel> GetProduct(long productId);
        Task DeleteProduct(long productId);
    }
}