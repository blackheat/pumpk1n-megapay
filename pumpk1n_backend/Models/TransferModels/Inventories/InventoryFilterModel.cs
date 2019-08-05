namespace pumpk1n_backend.Models.TransferModels.Inventories
{
    public class InventoryFilterModel
    {
        public long ProductId { get; set; }
        public long SupplierId { get; set; }
        public string ProductName { get; set; } = "";
        public string SupplierName { get; set; } = "";
    }
}