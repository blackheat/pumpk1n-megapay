using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Orders
{
    public class CartAlreadyExistsException : CustomException
    {
        public CartAlreadyExistsException()
        {
            Code = ErrorCode.CartAlreadyExistsException;
        }
    }
}