using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Accounts
{
    public class UserNotActivatedException : CustomException
    {
        public UserNotActivatedException()
        {
            Code = ErrorCode.UserNotActivatedException;
        }
    }
}