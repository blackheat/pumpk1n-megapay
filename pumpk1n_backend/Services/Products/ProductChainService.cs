using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using pumpk1n_backend.Exceptions.Chain;
using pumpk1n_backend.Models.ChainReturnModels.Products;
using pumpk1n_backend.Models.ChainTransferModels.Products;
using pumpk1n_backend.Settings;

namespace pumpk1n_backend.Services.Products
{
    public class ProductChainService : IProductChainService
    {
        private readonly HyperledgerFabricApiSettings _hyperledgerFabricApiSettings;

        public ProductChainService(IOptions<HyperledgerFabricApiSettings> hyperledgerFabricApiSettings)
        {
            _hyperledgerFabricApiSettings = hyperledgerFabricApiSettings.Value;
        }
        
        public async Task AddProduct(ChainProductTransferModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(_hyperledgerFabricApiSettings.Url + "/product", content);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new DataNotFoundInChainException();
        }

        public async Task<ChainProductReturnModel> GetProduct(long productId)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(_hyperledgerFabricApiSettings.Url + $"/product/{productId}");
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new DataNotFoundInChainException();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ChainProductReturnModel>(responseContent);
            return responseObject;
        }
        
        public async Task DeleteProduct(long productId)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(_hyperledgerFabricApiSettings.Url + $"/product/{productId}");
            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new DataNotFoundInChainException();
        }
    }
}