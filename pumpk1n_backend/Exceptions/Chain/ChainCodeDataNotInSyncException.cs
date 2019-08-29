using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Chain
{
    public class ChainCodeDataNotInSyncException : CustomException
    {
        public ChainCodeDataNotInSyncException()
        {
            Code = ErrorCode.ChainCodeDataNotInSyncException;
        }
    }
}