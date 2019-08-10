using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Tokens
{
    public class PendingTokenTransactionExistsException : CustomException
    {
        public PendingTokenTransactionExistsException()
        {
            Code = ErrorCode.PendingTokenTransactionExistsException;
        }
    }
}