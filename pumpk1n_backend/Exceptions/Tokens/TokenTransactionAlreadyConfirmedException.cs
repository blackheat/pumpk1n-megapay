using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Tokens
{
    public class TokenTransactionAlreadyConfirmedException : CustomException
    {
        public TokenTransactionAlreadyConfirmedException()
        {
            Code = ErrorCode.TokenTransactionAlreadyConfirmedException;
        }
    }
}