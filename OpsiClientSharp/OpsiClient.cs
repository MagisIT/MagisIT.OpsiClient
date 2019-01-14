using OpsiClientSharp.Models.Results;
using OpsiClientSharp.RpcInterfaces;

namespace OpsiClientSharp
{
    public class OpsiClient
    {
        public BackendInterface BackendInterface { get; }
        public DepotInterface DepotInterface { get; }
        public HostInterface HostInterface { get; }
        public ProductsInterface ProductsInterface { get; }

        private readonly OpsiHttpClient _opsiHttpClient;

        public OpsiClient(string opsiServerRpcEndpoint, string username, string password, bool ignoreInvalidCert = false)
        {
            _opsiHttpClient = new OpsiHttpClient(opsiServerRpcEndpoint, username, password, ignoreInvalidCert);

            BackendInterface = new BackendInterface(_opsiHttpClient);
            DepotInterface = new DepotInterface(_opsiHttpClient);
            HostInterface = new HostInterface(_opsiHttpClient);
            ProductsInterface = new ProductsInterface(_opsiHttpClient);
            BackendInterface = new BackendInterface(_opsiHttpClient);
        }

        /// <summary>
        /// Returns a new product on client interface for the specified client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public ProductsOnClientInterface GetProductsOnClientInterface(string clientId)
        {
            return new ProductsOnClientInterface(_opsiHttpClient, clientId);
        }

        /// <summary>
        /// Returns a new product on client interface for the specified client host
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public ProductsOnClientInterface GetProductsOnClientInterface(Host host)
        {
            return new ProductsOnClientInterface(_opsiHttpClient, host);
        }
    }
}
