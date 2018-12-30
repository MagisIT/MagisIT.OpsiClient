﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OpsiClientSharp.Exceptions;
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
                Timeout = System.Threading.Timeout.InfiniteTimeSpan
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
            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(timeout));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Serialize Request object
            string jsonRequest = request.ToJson();

            Console.WriteLine(jsonRequest);

            // Send json-rpc request
            HttpResponseMessage response = await _httpClient
                .PostAsync(OpsiServerRpcEndpoint, new StringContent(jsonRequest, Encoding.UTF8, "application/json"), cancellationSource.Token)
                .ConfigureAwait(false);

            // Read the response
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Console.WriteLine(content);
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
}
