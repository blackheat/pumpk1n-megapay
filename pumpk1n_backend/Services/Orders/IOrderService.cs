using System.Collections.Generic;
using System.Threading.Tasks;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.ReturnModels.Orders;
using pumpk1n_backend.Models.TransferModels.Orders;

namespace pumpk1n_backend.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderReturnModel> Checkout(long userId, CheckoutTransferModel model);
        Task<OrderReturnModel> ConfirmOrder(long orderId);
        Task<OrderReturnModel> CancelOrder(long orderId);
        Task<CustomList<OrderReturnModel>> GetUserOrders(long userId, int page, int count);
        Task<CustomList<OrderReturnModel>> GetOrders(int page, int count);
        Task<OrderReturnModel> GetOrder(long orderId);
        Task<OrderReturnModel> GetUserOrder(long userId, long orderId);
    }
}