using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Chain
{
    public class DataNotFoundInChainException : CustomException
    {
        public DataNotFoundInChainException()
        {
            Code = ErrorCode.DataNotFoundInChainException;
        }
    }
}