using Newtonsoft.Json;

namespace pumpk1n_backend.Models.ChainTransferModels.Products
{
    public class ChainProductTransferModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("hash")]
        public string Hash { get; set; }
        
        [JsonProperty("created_date")]
        public string CreatedDate { get; set; }
    }
}