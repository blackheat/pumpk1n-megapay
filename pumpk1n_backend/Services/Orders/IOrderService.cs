using System.Collections.Generic;
using System.Threading.Tasks;
using pumpk1n_backend.Models.ReturnModels.Orders;
using pumpk1n_backend.Models.TransferModels.Orders;

namespace pumpk1n_backend.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderReturnModel> CreateCart(long userId);
        Task<OrderReturnModel> GetCurrentCart(long userId);
        Task<OrderReturnModel> AddToCart(long userId, IEnumerable<OrderItemTransferModel> model);
        Task DeleteCartItem(long orderItemId);
        Task<OrderItemReturnModel> UpdateCartItemQuantity(long orderItemId, long quantity);
        Task<OrderReturnModel> Checkout(long userId, CustomerInformationCheckoutTransferModel model);
        Task<OrderReturnModel> ConfirmOrder(long orderId);
        Task<OrderReturnModel> CancelOrder(long orderId);
    }
}