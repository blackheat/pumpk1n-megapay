using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Tokens
{
    public class TokenBillingNotFoundException : CustomException
    {
        public TokenBillingNotFoundException()
        {
            Code = ErrorCode.TokenBillingNotFoundException;
        }
    }
}