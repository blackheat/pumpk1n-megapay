using System.Threading.Tasks;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.ReturnModels.Inventories;
using pumpk1n_backend.Models.TransferModels.Inventories;

namespace pumpk1n_backend.Services.Inventories
{
    public interface IInventoryService
    {
        Task<InventoryReturnModel> ImportProduct(InventoryImportModel model);
        Task<CustomList<InventoryReturnModel>> GetInventory(int page, int count);
    }
}