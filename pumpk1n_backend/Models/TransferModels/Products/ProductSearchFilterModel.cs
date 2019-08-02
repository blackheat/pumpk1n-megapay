namespace pumpk1n_backend.Models.TransferModels.Products
{
    public class ProductSearchFilterModel
    {
        public string Name { get; set; } = "";
        public float MinPrice { get; set; } = (float) 0.0;
        public float MaxPrice { get; set; } = float.MaxValue;
    }
}