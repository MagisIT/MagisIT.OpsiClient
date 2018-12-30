using System.Collections.Generic;
using System.Threading.Tasks;
using OpsiClientSharp.Models;

namespace OpsiClientSharp.RpcInterfaces
{
    public class DepotInterface : RpcInterface
    {
        public override string InterfaceName => "depot";

        public DepotInterface(OpsiClient opsiClient) : base(opsiClient) { }

        /// <summary>
        /// Returns a md5 sum of a file in the depot
        /// </summary>
        /// <param name="absoluteFilePath">The absolute file path to the file. Usually /var/lib/opsi/repository</param>
        /// <returns>The md5Sum of the file</returns>
        public Task<string> GetMd5SumAsync(string absoluteFilePath)
        {
            return OpsiClient.ExecuteAsync<string>(
                new Request(GetFullMethodName("getMD5Sum"), absoluteFilePath)
            );
        }

        /// <summary>
        /// Installs an opsi package.
        /// This method returns as soon as the installation is fully completed
        /// </summary>
        /// <see cref="OpsiPackageUploader">To upload a file to the server use the OpsiPackageUploader</see>
        /// <param name="absoluteFilePath"></param>
        /// <param name="timeout">The timeout to wait until the product is installed. We don't have any status update only if an error appears. Another solution would be to use the SSH Command or check the logs</param>
        /// <returns>Nothing if successful otherwise an exception</returns>
        public Task InstallPackageAsync(string absoluteFilePath, int timeout = 180)
        {
            return OpsiClient.ExecuteAsync<string>(
                new Request(GetFullMethodName("installPackage"), absoluteFilePath),
                timeout
            );
        }

        /// <summary>
        /// Uninstalls an opsi package
        /// </summary>
        /// <param name="productId">The product id which should be uninstalled</param>
        /// <param name="timeout">The timeout to wait until the product is uninstalled. We don't have any status update only if an error appears. Another solution would be to use the SSH Command or check the logs</param>
        /// <returns>Nothing if successful otherwise an exception</returns>
        public Task UninstallPackageAsync(string productId, int timeout = 180)
        {
            return OpsiClient.ExecuteAsync<string>(
                new Request(GetFullMethodName("uninstallPackage"), productId)
            );
        }
    }
}
