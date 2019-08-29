using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using pumpk1n_backend.Exceptions.Chain;
using pumpk1n_backend.Models.ChainReturnModels.Suppliers;
using pumpk1n_backend.Models.ChainTransferModels.Suppliers;
using pumpk1n_backend.Settings;

namespace pumpk1n_backend.Services.Suppliers
{
    public class SupplierChainService : ISupplierChainService
    {
        private readonly HyperledgerFabricApiSettings _hyperledgerFabricApiSettings;

        public SupplierChainService(IOptions<HyperledgerFabricApiSettings> hyperledgerFabricApiSettings)
        {
            _hyperledgerFabricApiSettings = hyperledgerFabricApiSettings.Value;
        }
        
        public async Task AddSupplier(ChainSupplierTransferModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(_hyperledgerFabricApiSettings.Url + "/supplier", content);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new DataNotFoundInChainException();
        }

        public async Task<ChainSupplierReturnModel> GetSupplier(long supplierId)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(_hyperledgerFabricApiSettings.Url + $"/supplier/{supplierId}");
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new DataNotFoundInChainException();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ChainSupplierReturnModel>(responseContent);
            return responseObject;
        }
        
        public async Task DeleteSupplier(long supplierId)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(_hyperledgerFabricApiSettings.Url + $"/supplier/{supplierId}");
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new DataNotFoundInChainException();
        }
    }
}