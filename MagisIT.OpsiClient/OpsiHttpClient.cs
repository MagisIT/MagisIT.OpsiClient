using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MagisIT.OpsiClient.Exceptions;
using MagisIT.OpsiClient.Models;
using MagisIT.OpsiClient.Utils;
using Newtonsoft.Json;

namespace MagisIT.OpsiClient
{
    /// <summary>
    /// Opsi-Client class communicating with the OPSI server rpc endpoint
    /// </summary>
    public class OpsiHttpClient
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

        internal OpsiHttpClient(string opsiServerRpcEndpoint, string username, string password, bool ignoreInvalidCert = false)
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
        /// <param name="request">The request object for the json rpc call</param>
        /// <param name="timeout">The time before the request ist canceled</param>
        /// <returns></returns>
        public async Task<T> ExecuteAsync<T>(Request request, int timeout = 10)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            using (var cancellationSource = new CancellationTokenSource())
            {
                cancellationSource.CancelAfter(TimeSpan.FromSeconds(timeout));

                // Serialize Request object
                string jsonRequest = request.ToJson();

                Debug.WriteLine($"RPC-Request: {jsonRequest}");

                // Send json-rpc request
                HttpResponseMessage response = await _httpClient
                    .PostAsync(OpsiServerRpcEndpoint, new StringContent(jsonRequest, Encoding.UTF8, "application/json"), cancellationSource.Token)
                    .ConfigureAwait(false);

                // Read the response
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                Debug.WriteLine($"RPC-Response: {content}");

                // Check if the response is valid
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new OpsiClientRequestException($"Server returns an error: {response.StatusCode}. Message: {content}");

                // Parse JSON by using the ResultParser of the Result
                var result = JsonConvert.DeserializeObject<Result<T>>(content, new JsonSettings());

                // Is there an OPSI server error?
                if (result.Error != null)
                    throw new OpsiClientRequestException($"Server returns an error: {result.Error.Message}.");

                return result.Data;
            }
        }

        /// <summary>
        /// Uploads a stream to the specified webdav server
        /// </summary>
        /// <param name="webdavServerUrl"></param>
        /// <param name="pathOnServer"></param>
        /// <param name="filename"></param>
        /// <param name="streamToUpload"></param>
        /// <returns></returns>
        /// <exception cref="OpsiPackageUploadException"></exception>
        public async Task UploadAsync(string webdavServerUrl, string pathOnServer, string filename, Stream streamToUpload)
        {
            HttpResponseMessage httpResponseMessage =
                await _httpClient.PutAsync($"{webdavServerUrl}/{pathOnServer}/{filename}", new StreamContent(streamToUpload)).ConfigureAwait(false);

            if (httpResponseMessage.StatusCode != HttpStatusCode.Created)
                throw new OpsiPackageUploadException($"Server returns error {httpResponseMessage.StatusCode}");
        }
    }
}
