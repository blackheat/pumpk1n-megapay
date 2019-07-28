using System;
using pumpk1n_backend.Enumerations;

namespace pumpk1n_backend.Helpers.Accounts
{
    public interface IAccountHelper
    {
        string JwtGenerator(Int64 userId, Int64 loginAttemptId, UserType userType);
    }
}