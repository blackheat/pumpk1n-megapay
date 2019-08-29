using Newtonsoft.Json;

namespace pumpk1n_backend.Models.ChainReturnModels.Accounts
{
    public class ChainAccountReturnModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("hash")]
        public string Hash { get; set; }
        
        [JsonProperty("created_date")]
        public string CreatedDate { get; set; }
        
        [JsonProperty("balance")]
        public string Balance { get; set; }
    }
}