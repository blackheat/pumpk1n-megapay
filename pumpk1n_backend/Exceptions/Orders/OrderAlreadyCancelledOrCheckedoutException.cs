using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Orders
{
    public class OrderAlreadyCancelledOrCheckedoutException : CustomException
    {
        public OrderAlreadyCancelledOrCheckedoutException()
        {
            Code = ErrorCode.OrderAlreadyCancelledOrCheckedoutException;
        }
    }
}