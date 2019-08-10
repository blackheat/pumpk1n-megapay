using System.Threading.Tasks;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.ReturnModels.Tokens;
using pumpk1n_backend.Models.TransferModels.Tokens;

namespace pumpk1n_backend.Services.Tokens
{
    public interface ITokenService
    {
        Task<UserTokenBalanceModel> GetUserBalance(long userId);

        Task<UserTokenTransactionModel> CreateTokenPurchaseRequest(long userId,
            TokenTransactionInsertModel model);

        Task<UserTokenTransactionModel> GetTokenPurchaseRequest(long txId);
        Task<UserTokenTransactionModel> GetUserTokenPurchaseRequest(long userId, long txId);
        Task<UserTokenTransactionModel> CancelTokenPurchaseRequest(long requestId);
        Task<CustomList<UserTokenTransactionModel>> GetUserTokenPurchaseRequests(long userId, int count = 10, int page = 1);
        Task<CoinGateBillModel> CreateUserBilling(long userId, long txId);
        Task ProcessCoinGateHook(CoinGateHookTransferModel model);

        Task<UserTokenTransactionModel> CreateTokenTransaction(long userId,
            TokenTransactionInsertModel model, TokenTransactionType transactionType);
    }
}