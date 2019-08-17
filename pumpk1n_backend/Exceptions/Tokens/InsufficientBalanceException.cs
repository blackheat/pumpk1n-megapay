using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Tokens
{
    public class InsufficientBalanceException : CustomException
    {
        public InsufficientBalanceException()
        {
            Code = ErrorCode.InsufficientBalanceException;
        }
    }
}