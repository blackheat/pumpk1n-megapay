using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Products
{
    public class ProductNotFoundException : CustomException
    {
        public ProductNotFoundException()
        {
            Code = ErrorCode.ProductNotFoundException;
        }
    }
}