using System;
using System.Threading.Tasks;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Models.Entities.Tokens;
using pumpk1n_backend.Models.ReturnModels.Tokens;
using pumpk1n_backend.Models.TransferModels.Tokens;

namespace pumpk1n_backend.Helpers.Tokens
{
    public interface ITokenHelper
    {
        Task<CoinGateInvoiceReturnModel> GenerateInvoice(CoinGateInvoiceTransferModel model);
        Task<CoinGateInvoiceReturnModel> GetInvoiceInfo(long orderId);

        UserTokenTransaction AddTokenTransaction(long userId, DateTime addedDate,
            DateTime confirmedDate, TokenTransactionInsertModel model, TokenTransactionType transactionType);
    }
}