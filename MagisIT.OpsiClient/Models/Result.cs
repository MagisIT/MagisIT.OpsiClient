using Newtonsoft.Json;

namespace MagisIT.OpsiClient.Models
{
    public class Result<TData>
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Result")]
        public TData Data { get; set; }

        public Error Error { get; set; }
    }
}
