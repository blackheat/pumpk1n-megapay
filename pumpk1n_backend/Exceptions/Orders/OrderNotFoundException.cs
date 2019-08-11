using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Orders
{
    public class OrderNotFoundException : CustomException
    {
        public OrderNotFoundException()
        {
            Code = ErrorCode.OrderNotFoundException;
        }
    }
}