using System.Threading.Tasks;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.ReturnModels.Products;
using pumpk1n_backend.Models.TransferModels.Products;

namespace pumpk1n_backend.Services.Products
{
    public interface IProductService
    {
        Task<ProductReturnModel> AddProduct(ProductInsertModel model);
        Task<ProductReturnModel> UpdateProduct(ProductInsertModel model, long productId);
        Task DeleteProduct(long productId);
        Task<ProductReturnModel> GetProduct(long productId);
        Task<CustomList<ProductReturnModel>> GetProducts(int page, int count, string name = "");
        Task ChangeProductDeprecatedStatus(long productId, bool isDeprecated);
        Task ChangeProductStockStatus(long productId, bool isOutOfStock);
    }
}