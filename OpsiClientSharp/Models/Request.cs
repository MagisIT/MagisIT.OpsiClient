using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace OpsiClientSharp.Utils
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
        public List<string> Params { get; } = new List<string>();

        /// <summary>
        /// Only shows the defined attributes in the result
        /// </summary>
        public List<String> Attributes { get; } = new List<string>();

        /// <summary>
        /// Only shows the results with the following filter
        /// </summary>
        public Dictionary<string, string> Filter { get; } = new Dictionary<string, string>();


        /// <summary>
        /// The unique id for this request
        /// </summary>
        public int Id { get; }

        public Request(string method)
        {
            Method = method;
            Id = _id++;
        }

        public Request(string method, params string[] methodParams) : this(method)
        {
            Params = new List<string>(methodParams);
        }

        public Request(string method, Dictionary<string, string> filter, params string[] methodParams) : this(method, methodParams)
        {
            Filter = filter;
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

            if (Filter.Any())
            {
                // We need to add an empty attributes array if there are no attributes so that the filter parameter is respected
                if (!Attributes.Any())
                    parameters.Add(new JArray());

                parameters.Add(JObject.FromObject(Filter));
            }

            JObject request = new JObject(
                new JProperty("method", Method),
                new JProperty("params", parameters),
                new JProperty("id", Id));

            return request.ToString();
        }
    }
}
