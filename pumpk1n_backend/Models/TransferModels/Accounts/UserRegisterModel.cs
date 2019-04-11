using System;
using AutoMapper;
using pumpk1n_backend.Models.Entities.Accounts;

namespace pumpk1n_backend.Models.TransferModels.Accounts
{
    public class UserRegisterModel : UserCompleteProfileModel
    {
        public String Email { get; set; }
    }

    public class AccountRegisterByGoogleModel
    {
        public String GoogleAuthenticationToken { get; set; }
    }
}
