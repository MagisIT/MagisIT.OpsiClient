using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpsiClientSharp.Exceptions;
using OpsiClientSharp.Models;
using OpsiClientSharp.Models.Results;
using OpsiClientSharp.Types;

namespace OpsiClientSharp.RpcInterfaces
{
    public class ProductsInterface : RpcInterface
    {
        public override string InterfaceName => "product";

        public ProductsInterface(OpsiClient opsiClient) : base(opsiClient) { }

        /// <summary>
        /// Returns all products on this opsi server
        /// </summary>
        /// <returns></returns>
        public Task<List<ProductObjectResult>> GetAllAsync()
        {
            return OpsiClient.ExecuteAsync<List<ProductObjectResult>>(new Request(GetFullMethodName("getObjects")));
        }

        /// <summary>
        /// Returns all products on this opsi server depending on the product type
        /// </summary>
        /// <param name="productType">Which product type should be returned</param>
        /// <returns></returns>
        public Task<List<ProductObjectResult>> GetAllAsync(ProductType productType)
        {
            return OpsiClient.ExecuteAsync<List<ProductObjectResult>>(
                new Request(GetFullMethodName("getObjects"), new Dictionary<string, string> {
                    {"type", productType.ToOpsiName()}
                })
            );
        }

        /// <summary>
        /// Retrieves a single product from the server
        /// </summary>
        /// <param name="productId">The id of the product which should be returned</param>
        /// <returns></returns>
        public Task<List<ProductObjectResult>> GetAsync(string productId)
        {
            return OpsiClient.ExecuteAsync<List<ProductObjectResult>>(
                new Request(GetFullMethodName("getObjects"), new Dictionary<string, string> {
                    {"ident", productId}
                })
            );
        }

        /// <summary>
        /// Check whether the product id exists on the opsi server
        /// </summary>
        /// <param name="productId">The product id which should be checked</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string productId)
        {
            List<ProductObjectResult> products = await GetAsync(productId);

            return products.Any();
        }
    }
}
