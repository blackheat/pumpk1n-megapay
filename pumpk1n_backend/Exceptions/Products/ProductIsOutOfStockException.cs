using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Products
{
    public class ProductIsOutOfStockException : CustomException
    {
        public ProductIsOutOfStockException()
        {
            Code = ErrorCode.ProductIsOutOfStockException;
        }
    }
}