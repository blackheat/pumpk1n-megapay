using System;
using System.Threading.Tasks;
using pumpk1n_backend.Models.ReturnModels.Accounts;
using pumpk1n_backend.Models.TransferModels.Accounts;

namespace pumpk1n_backend.Services.Accounts
{
    public interface IAccountService
    {
        Task RegisterAccount(UserRegisterModel model);
        Task<UserBearerTokenModel> UserLogin(UserLoginModel model);
        Task<UserInformationModel> GetUserDetails(Int64 userId);
    }
}