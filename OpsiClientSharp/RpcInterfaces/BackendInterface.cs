using System.Threading.Tasks;
using OpsiClientSharp.Models;
using OpsiClientSharp.Models.Results;

namespace OpsiClientSharp.RpcInterfaces
{
    public class BackendInterface : RpcInterface<BackendInfo>
    {
        /// <summary>
        /// The prefix of this interface
        /// </summary>
        public override string InterfaceName => "backend";

        internal BackendInterface(OpsiHttpClient opsiHttpClient) : base(opsiHttpClient) { }

        /// <summary>
        /// Retrieve general opsi information of the backend
        /// </summary>
        public Task<BackendInfo> InfoAsync()
        {
            return OpsiHttpClient.ExecuteAsync<BackendInfo>(new Request(GetFullMethodName("info")));
        }
    }
}
