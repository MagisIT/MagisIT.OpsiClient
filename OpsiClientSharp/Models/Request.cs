using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace OpsiClientSharp.Models
{
    /// <summary>
    /// Base class for an opsi rpc request
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Each request needs a unique request id
        /// Must start at 1
        /// </summary>
        private static int _id = 1;

        /// <summary>
        /// The method name of the request
        /// </summary>
        public string Method { get; }

        /// <summary>
        /// The parameters the requests uses per definition
        /// </summary>
        public List<object> Params { get; } = new List<object>();

        /// <summary>
        /// Only shows the defined attributes in the result
        /// </summary>
        public List<string> Attributes { get; } = new List<string>();

        /// <summary>
        /// Only shows the results with the following filter
        /// </summary>
        public RequestFilter RequestFilter { get; } = new RequestFilter();


        /// <summary>
        /// The unique id for this request
        /// </summary>
        public int Id { get; }

        public Request(string method)
        {
            Method = method;
            Id = _id++;
        }

        public Request(string method, params object[] methodParams) : this(method)
        {
            Params = new List<object>(methodParams);
        }

        public Request(string method, RequestFilter requestFilter, params object[] methodParams) : this(method, methodParams)
        {
            RequestFilter = requestFilter;
        }

        /// <summary>
        /// Returns the json of this request
        /// We need to build it manually because it needs a specific structure
        /// TODO: Maybe there's a better way?
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            JArray parameters = new JArray();
            parameters.Add(Params);

            // Add attributes if any exist
            if (Attributes.Any())
                parameters.Add(Attributes);

            // Get JSON from filter

            if (RequestFilter.HasElements())
            {
                // We need to add an empty attributes array if there are no attributes so that the filter parameter is respected
                if (!Attributes.Any())
                    parameters.Add(new JArray());

                parameters.Add(RequestFilter.ToJson());
            }

            JObject request = new JObject(
                new JProperty("method", Method),
                new JProperty("params", parameters),
                new JProperty("id", Id));

            return request.ToString();
        }
    }
}
