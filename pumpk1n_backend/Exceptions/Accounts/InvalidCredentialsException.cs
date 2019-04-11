using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Exceptions.Accounts
{
    public class InvalidCredentialsException : CustomException
    {
        public InvalidCredentialsException()
        {
            Code = ErrorCode.InvalidCredentialsException;
        }
    }
}