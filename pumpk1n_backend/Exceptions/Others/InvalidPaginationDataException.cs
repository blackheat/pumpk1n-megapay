using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Others
{
    public class InvalidPaginationDataException : CustomException
    {
        public InvalidPaginationDataException()
        {
            Code = ErrorCode.InvalidPaginationDataException;
        }
    }
}