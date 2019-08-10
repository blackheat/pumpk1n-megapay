using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Tokens
{
    public class PendingTokenBillingExistsException : CustomException
    {
        public PendingTokenBillingExistsException()
        {
            Code = ErrorCode.PendingTokenBillingExistsException;
        }
    }
}