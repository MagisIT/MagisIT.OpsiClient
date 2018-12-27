using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpsiClientSharp.Exceptions;
using OpsiClientSharp.Models;
using OpsiClientSharp.Utils;

namespace OpsiClientSharp
{
    /// <summary>
    /// Opsi-Client class communicating with the OPSI server rpc endpoint
    /// </summary>
    public class OpsiClient
    {
        private readonly CookieContainer _cookieContainer = new CookieContainer();

        /// <summary>
        /// HTTP-Client handling the communication with the OPSI server
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// RPC endpoint of the Opsi server
        /// </summary>
        public string OpsiServerRpcEndpoint { get; }

        public OpsiClient(string opsiServerRpcEndpoint, string username, string password, bool ignoreInvalidCert = false)
        {
            // Null Check parameters
            OpsiServerRpcEndpoint = opsiServerRpcEndpoint ?? throw new ArgumentNullException(nameof(opsiServerRpcEndpoint));

            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            // Set cookie container
            HttpClientHandler httpClientHandler = new HttpClientHandler {
                CookieContainer = _cookieContainer
            };

            // Ignore invalid ssl certificate if requested
            if (ignoreInvalidCert)
                httpClientHandler.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true;

            // Create http Client
            _httpClient = new HttpClient(httpClientHandler) {
                Timeout = TimeSpan.FromSeconds(10)
            };

            // Set Basic Auth header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
        }

        /// <summary>
        /// Sends a request to the Opsi server
        /// </summary>
        /// <param name="jsonData">The json data of the json-rpc request</param>
        /// <returns></returns>
        public async Task<TResult<T>> ExecuteAsync<T>(string jsonData) where T : Result
        {
            if (jsonData == null)
                throw new ArgumentNullException(nameof(jsonData));

            // Send json-rpc request
            HttpResponseMessage response = await _httpClient.PostAsync(OpsiServerRpcEndpoint, new StringContent(jsonData, Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            // Read the response
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Check if the response is valid
            if (response.StatusCode != HttpStatusCode.OK)
                throw new OpsiClientRequestException($"Server returns an error: {response.StatusCode}. Message: {content}");

            // Parse content to Json
            var commandResponse = JsonConvert.DeserializeObject<TResult<T>>(content, new JsonSettings());

            // Is there an OPSI server error?
            if (commandResponse.Error != null)
                throw new OpsiClientRequestException($"Server returns an error: {commandResponse.Error.Message}.");

            return commandResponse;
        }
    }
}
