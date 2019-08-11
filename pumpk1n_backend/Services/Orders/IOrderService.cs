using System.Threading.Tasks;
using pumpk1n_backend.Models.ReturnModels.Orders;
using pumpk1n_backend.Models.TransferModels.Orders;

namespace pumpk1n_backend.Services.Orders
{
    public interface IOrderService
    {
        Task<OrderReturnModel> CreateCart(long userId);
        Task<OrderReturnModel> GetCurrentCart(long userId);
        Task<OrderItemReturnModel> AddToCart(long userId, OrderItemTransferModel model);
        Task DeleteCartItem(long orderItemId);
        Task<OrderItemReturnModel> UpdateCartItemQuantity(long orderItemId, long quantity);
        Task<OrderItemReturnModel> Checkout(long userId, CustomerInformationCheckoutTransferModel model);
        Task<OrderItemReturnModel> ConfirmOrder(long orderId);
        Task<OrderItemReturnModel> CancelOrder(long orderId);
    }
}