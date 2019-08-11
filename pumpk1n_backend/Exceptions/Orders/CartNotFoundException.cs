using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Orders
{
    public class CartNotFoundException : CustomException
    {
        public CartNotFoundException()
        {
            Code = ErrorCode.CartNotFoundException;
        }
    }
}