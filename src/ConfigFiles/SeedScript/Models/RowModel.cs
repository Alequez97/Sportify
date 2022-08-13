using Newtonsoft.Json;

namespace SeedScript.Models
{
    public class RowModel
    {
        public string Row { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SqlSeedModel InnerInsertModel { get; set; }
    }
}
