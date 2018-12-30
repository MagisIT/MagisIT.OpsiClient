using System.Threading.Tasks;
using OpsiClientSharp.Models.Results;
using OpsiClientSharp.Utils;

namespace OpsiClientSharp.RpcInterfaces
{
    public class BackendInterface : RpcInterface
    {
        /// <summary>
        /// The prefix of this interface
        /// </summary>
        public override string InterfaceName => "backend";


        public BackendInterface(OpsiClient opsiClient) : base(opsiClient) { }

        /// <summary>
        /// Retrieve general opsi information of the backend
        /// </summary>
        public async Task<BackendInfoResult> InfoAsync()
        {
            return await OpsiClient.ExecuteAsync<BackendInfoResult>(new Request(GetFullMethodName("info")));
        }
    }
}
