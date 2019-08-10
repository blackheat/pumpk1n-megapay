using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Tokens
{
    public class TokenTransactionNotFoundException : CustomException
    {
        public TokenTransactionNotFoundException()
        {
            Code = ErrorCode.TokenTransactionNotFoundException;
        }
    }
}