using System.Threading.Tasks;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.ReturnModels.Accounts;
using pumpk1n_backend.Models.TransferModels.Accounts;

namespace pumpk1n_backend.Services.Accounts
{
    public interface IAccountService
    {
        Task RegisterAccount(UserRegisterModel model, UserType userType);
        Task<UserBearerTokenModel> UserLogin(UserLoginModel model);
        Task<UserInformationModel> GetUserDetails(long userId);
        Task<CustomList<UserInformationModel>> GetUsers(int page, int count, UserAccountFilterModel filterModel);
    }
}