using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Helpers.Accounts
{
    public interface IAccountHelper
    {
        string JwtGenerator(long userId, string fullName, long loginAttemptId, UserType userType);
    }
}