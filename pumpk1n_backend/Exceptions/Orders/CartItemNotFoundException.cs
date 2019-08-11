using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Orders
{
    public class CartItemNotFoundException : CustomException
    {
        public CartItemNotFoundException()
        {
            Code = ErrorCode.CartItemNotFoundException;
        }
    }
}