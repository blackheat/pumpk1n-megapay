using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Tokens
{
    public class TokenTransactionAlreadyCancelledException : CustomException
    {
        public TokenTransactionAlreadyCancelledException()
        {
            Code = ErrorCode.TokenTransactionAlreadyCancelledException;
        }
    }
}