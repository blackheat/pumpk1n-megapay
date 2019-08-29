using System.Threading.Tasks;
using pumpk1n_backend.Models.ChainReturnModels.Accounts;
using pumpk1n_backend.Models.ChainTransferModels.Accounts;

namespace pumpk1n_backend.Services.Accounts
{
    public interface IAccountChainService
    {
        Task AddAccount(ChainAccountTransferModel model);
        Task<ChainAccountReturnModel> GetAccount(long accountId);
        Task DeleteAccount(long accountId);
    }
}