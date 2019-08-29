using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using pumpk1n_backend.Exceptions.Chain;
using pumpk1n_backend.Models.ChainReturnModels.Accounts;
using pumpk1n_backend.Models.ChainTransferModels.Accounts;
using pumpk1n_backend.Settings;

namespace pumpk1n_backend.Services.Accounts
{
    public class AccountChainService : IAccountChainService
    {
        private readonly HyperledgerFabricApiSettings _hyperledgerFabricApiSettings;

        public AccountChainService(IOptions<HyperledgerFabricApiSettings> hyperledgerFabricApiSettings)
        {
            _hyperledgerFabricApiSettings = hyperledgerFabricApiSettings.Value;
        }
        
        public async Task AddAccount(ChainAccountTransferModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(_hyperledgerFabricApiSettings.Url + "/account", content);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new DataNotFoundInChainException();
        }

        public async Task<ChainAccountReturnModel> GetAccount(long accountId)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(_hyperledgerFabricApiSettings.Url + $"/account/{accountId}");
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new DataNotFoundInChainException();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ChainAccountReturnModel>(responseContent);
            return responseObject;
        }
        
        public async Task DeleteAccount(long accountId)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(_hyperledgerFabricApiSettings.Url + $"/account/{accountId}");
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new DataNotFoundInChainException();
        }
    }
}