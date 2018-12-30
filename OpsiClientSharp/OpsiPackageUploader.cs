using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OpsiClientSharp.Exceptions;

namespace OpsiClientSharp
{
    public class OpsiPackageUploader
    {
        /// <summary>
        /// HttpClient for uploading the files using webdav
        /// </summary>
        private HttpClient _httpClient;

        public string WebdavUrl { get; }

        /// <summary>
        /// Instantiate the OpsiPackageUploader class
        /// </summary>
        /// <param name="webdavServerUrl">The webdav server where the files should be uploaded</param>
        /// <param name="username">The username to authenticate with</param>
        /// <param name="password">The password to authenticate with</param>
        /// <param name="ignoreInvalidCert">Ignore any invalid certificate</param>
        public OpsiPackageUploader(string webdavServerUrl, string username, string password, bool ignoreInvalidCert = false)
        {
            WebdavUrl = webdavServerUrl ?? throw new ArgumentNullException(nameof(webdavServerUrl));

            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            HttpClientHandler httpClientHandler = new HttpClientHandler();

            // Ignore invalid ssl certificate if requested
            if (ignoreInvalidCert)
                httpClientHandler.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true;

            // Create http Client
            _httpClient = new HttpClient(httpClientHandler) {
                Timeout = TimeSpan.FromSeconds(10)
            };

            // Set authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
        }

        public async Task UploadAsync(string pathOnServer, string filename, Stream streamToUpload)
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.PutAsync($"{WebdavUrl}/{pathOnServer}/{filename}", new StreamContent(streamToUpload)).ConfigureAwait(false);

            if (httpResponseMessage.StatusCode != HttpStatusCode.Created)
                throw new OpsiPackageUploadException($"Server returns error {httpResponseMessage.StatusCode}");
        }
    }
}
