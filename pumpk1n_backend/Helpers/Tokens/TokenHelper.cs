using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Exceptions.Accounts;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Models.Entities.Tokens;
using pumpk1n_backend.Models.ReturnModels.Tokens;
using pumpk1n_backend.Models.TransferModels.Tokens;
using pumpk1n_backend.Settings;

namespace pumpk1n_backend.Helpers.Tokens
{
    public class TokenHelper : ITokenHelper
    {
        private readonly CoinGateSettings _coinGateSettings;
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;

        public TokenHelper(IOptions<CoinGateSettings> coinGateSettings, IMapper mapper, DatabaseContext context)
        {
            _coinGateSettings = coinGateSettings.Value;
            _mapper = mapper;
            _context = context;
        }

        public async Task<CoinGateInvoiceReturnModel> GenerateInvoice(CoinGateInvoiceTransferModel model)
        {
            if (string.IsNullOrEmpty(model.CallbackUrl) || string.IsNullOrWhiteSpace(model.CallbackUrl))
                model.CallbackUrl = _coinGateSettings.CallbackUrl;

            if (string.IsNullOrEmpty(model.CancelUrl) || string.IsNullOrWhiteSpace(model.CancelUrl))
                model.CancelUrl = _coinGateSettings.CallbackUrl;

            if (string.IsNullOrEmpty(model.SuccessUrl) || string.IsNullOrWhiteSpace(model.SuccessUrl))
                model.SuccessUrl = _coinGateSettings.CallbackUrl;
            
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _coinGateSettings.Token);
            var response = await httpClient.PostAsync(_coinGateSettings.Url + "/orders", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<CoinGateInvoiceReturnModel>(responseContent);
            return responseObject;
        }

        public async Task<CoinGateInvoiceReturnModel> GetInvoiceInfo(long orderId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _coinGateSettings.Token);
            var response = await httpClient.GetAsync($"{_coinGateSettings.Url}/orders/{orderId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<CoinGateInvoiceReturnModel>(responseContent);
            return responseObject;
        }

        public UserTokenTransaction AddTokenTransaction(long userId, DateTime addedDate,
            DateTime confirmedDate, TokenTransactionInsertModel model, TokenTransactionType transactionType)
        {
            var tokenTransaction = _mapper.Map<TokenTransactionInsertModel, UserTokenTransaction>(model);
            tokenTransaction.CustomerId = userId;
            tokenTransaction.AddedDate = addedDate;
            tokenTransaction.ConfirmedDate = confirmedDate;
            tokenTransaction.TransactionType = transactionType;

            _context.UserTokenTransactions.Add(tokenTransaction);

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new UserDoesNotExistException();
            if (transactionType == TokenTransactionType.Add)
                user.Balance += model.Amount;
            else
                user.Balance -= model.Amount;

            _context.Users.Update(user);
            _context.SaveChanges();
            return tokenTransaction;
        }
    }
}