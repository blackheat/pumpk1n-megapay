using System.Collections.Generic;

namespace pumpk1n_backend.Models.TransferModels.Orders
{
    public class CheckoutTransferModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public IEnumerable<OrderItemTransferModel> Items = new List<OrderItemTransferModel>();
    }
}