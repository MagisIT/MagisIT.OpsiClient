using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace OpsiClientSharp.Utils
{
    public abstract class JsonSerializable
    {
        public JObject ToJsonObject()
        {
            return JObject.FromObject(this, new JsonSerializer() {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
