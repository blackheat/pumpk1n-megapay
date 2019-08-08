using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Accounts
{
    public class UserDoesNotExistException : CustomException
    {
        public UserDoesNotExistException()
        {
            Code = ErrorCode.UserDoesNotExistException;
        }
    }
}