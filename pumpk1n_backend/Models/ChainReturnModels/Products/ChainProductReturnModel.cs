using Newtonsoft.Json;

namespace pumpk1n_backend.Models.ChainReturnModels.Products
{
    public class ChainProductReturnModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("hash")]
        public string Hash { get; set; }
        
        [JsonProperty("created_date")]
        public string CreatedDate { get; set; }
    }
}