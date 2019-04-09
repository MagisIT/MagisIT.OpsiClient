using System.Threading.Tasks;
using MagisIT.OpsiClient.Models;
using MagisIT.OpsiClient.Models.Results;

namespace MagisIT.OpsiClient.RpcInterfaces
{
    public class BackendInterface : RpcInterface<BackendInfo>
    {
        /// <summary>
        /// The prefix of this interface
        /// </summary>
        public override string InterfaceName => "backend";

        public BackendInterface(OpsiHttpClient opsiHttpClient) : base(opsiHttpClient) { }

        /// <summary>
        /// Retrieve general opsi information of the backend
        /// </summary>
        public Task<BackendInfo> InfoAsync() => OpsiHttpClient.ExecuteAsync<BackendInfo>(new Request(GetFullMethodName("info")));
  }
}
