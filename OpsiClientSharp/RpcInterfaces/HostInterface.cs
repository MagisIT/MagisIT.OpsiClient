using System.Collections.Generic;
using System.Threading.Tasks;
using OpsiClientSharp.Models.Results;
using OpsiClientSharp.Utils;

namespace OpsiClientSharp.RpcInterfaces
{
    public class HostInterface : RpcInterface
    {
        public override string InterfaceName => "host";

        public HostInterface(OpsiClient opsiClient) : base(opsiClient) { }

        /// <summary>
        /// Retrieves all registered clients
        /// </summary>
        /// <returns></returns>
        public Task<List<ClientObjectResult>> GetClientsAsync()
        {
            return OpsiClient.ExecuteAsync<List<ClientObjectResult>>(
                new Request(GetFullMethodName("getObjects"), new Dictionary<string, string> {{"type", "OpsiClient"}})
            );
        }
    }
}
