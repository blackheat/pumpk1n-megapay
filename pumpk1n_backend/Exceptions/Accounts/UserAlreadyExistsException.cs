using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Accounts
{
    public class UserAlreadyExistsException : CustomException
    {
        public UserAlreadyExistsException()
        {
            Code = ErrorCode.UserAlreadyExistsException;
        }
    }
}