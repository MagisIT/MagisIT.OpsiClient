using System.Collections.Generic;
using System.Threading.Tasks;
using OpsiClientSharp.Models.Results;

namespace OpsiClientSharp.RpcInterfaces
{
    public class HostInterface : RpcInterface<Host>
    {
        public override string InterfaceName => "host";

        public HostInterface(OpsiClient opsiClient) : base(opsiClient) { }

        /// <summary>
        ///     Retrieves all registered clients
        /// </summary>
        /// <returns></returns>
        public Task<List<Host>> GetClientsAsync() => GetAllAsync(new RequestFilter().Add("type", "OpsiClient"));
    }
}
