using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Products
{
    public class ProductIsDeprecatedException : CustomException
    {
        public ProductIsDeprecatedException()
        {
            Code = ErrorCode.ProductIsDeprecatedException;
        }
    }
}