using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpsiClientSharp.Utils
{
    public class Result<TData>
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Result")]
        public TData Data { get; set; }

        public Error Error { get; set; }
    }
}
