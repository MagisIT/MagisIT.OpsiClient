using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpsiClientSharp.Utils;

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
        private static int _nextId = 1;

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
        public RequestFilter RequestFilter { get; private set; } = new RequestFilter();


        /// <summary>
        /// The unique id for this request
        /// </summary>
        public int Id { get; }

        public Request(string method)
        {
            Method = method;
            Id = _nextId++;
        }

        /// <summary>
        /// Adds an attribute to the attribute filter
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public Request AddAttribute(string attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            Attributes.Add(attribute);
            return this;
        }

        /// <summary>
        /// Adds a list of attributes to the attribute filter
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public Request AddAttributes(IEnumerable<string> attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));

            Attributes.AddRange(attributes);
            return this;
        }

        /// <summary>
        /// Filters the output based on the request filter
        /// </summary>
        /// <param name="requestFilter"></param>
        /// <returns></returns>
        public Request Filter(RequestFilter requestFilter)
        {
            RequestFilter = requestFilter ?? throw new ArgumentNullException(nameof(requestFilter));
            return this;
        }

        /// <summary>
        /// Adds a string parameter to the request
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Request AddParameter(string param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param));

            Params.Add(param);
            return this;
        }

        /// <summary>
        /// Adds a parameter containing a json array to the request
        /// </summary>
        /// <param name="jArray"></param>
        /// <returns></returns>
        public Request AddParameter(JArray jArray)
        {
            if (jArray == null)
                throw new ArgumentNullException(nameof(jArray));

            Params.Add(jArray);
            return this;
        }

        /// <summary>
        /// Adds a parameter containing a json object to the request
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        public Request AddParameter(JObject jObject)
        {
            if (jObject == null)
                throw new ArgumentNullException(nameof(jObject));

            Params.Add(jObject);
            return this;
        }

        /// <summary>
        /// Adds a json serializable object to the parameters
        /// </summary>
        /// <param name="jsonSerializable"></param>
        /// <returns></returns>
        public Request AddParameter(JsonSerializable jsonSerializable)
        {
            if (jsonSerializable == null)
                throw new ArgumentNullException(nameof(jsonSerializable));

            Params.Add(jsonSerializable.ToJsonObject());
            return this;
        }

        /// <summary>
        /// Adds multiple string parameters to this request
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Request AddParameters(IEnumerable<string> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            Params.AddRange(parameters);
            return this;
        }

        /// <summary>
        /// Adds a list of json serializables to the request
        /// This adds every json object as one argument of the rpc method
        /// If you need these objects as an array which will be interpreted as only one argument use
        /// <see cref="AddParametersAsJArray"/> instead
        /// </summary>
        /// <param name="jsonSerializables"></param>
        /// <returns></returns>
        public Request AddParameters(IEnumerable<JsonSerializable> jsonSerializables)
        {
            if (jsonSerializables == null)
                throw new ArgumentNullException(nameof(jsonSerializables));

            Params.Add(jsonSerializables.Select(serializable => serializable.ToJsonObject()));
            return this;
        }

        /// <summary>
        /// Adds an array or variable string parameters to this request
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Request AddParameters(params string[] parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            Params.AddRange(parameters);
            return this;
        }

        /// <summary>
        /// Adds a list of json serializable objects to the parameters
        /// Wrapps all json serializables into one single array so that the rpc interface interprets this as one argument
        /// </summary>
        /// <param name="jsonSerializables"></param>
        /// <returns></returns>
        public Request AddParametersAsJArray(IEnumerable<JsonSerializable> jsonSerializables)
        {
            if (jsonSerializables == null)
                throw new ArgumentNullException(nameof(jsonSerializables));

            // We need to wrap the json objects into an array, because OPSI expects the first parameter to be an array or only one json object
            Params.Add(JArray.FromObject(jsonSerializables.Select(jsonSerializable => jsonSerializable.ToJsonObject())));
            return this;
        }

        /// <summary>
        /// Adds a json parameter without escaping the string when adding to the request
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public Request AddRawJsonParameter(string json)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            Params.Add(JToken.FromObject(json));
            return this;
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
