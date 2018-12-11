using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OpsiClientSharp.Utils
{
    public class JsonSettings : JsonSerializerSettings
    {
        public JsonSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Formatting = Formatting.Indented;
            TypeNameHandling = TypeNameHandling.None;
            NullValueHandling = NullValueHandling.Include;
        }
    }
}
