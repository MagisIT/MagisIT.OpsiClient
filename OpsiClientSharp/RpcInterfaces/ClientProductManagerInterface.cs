using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpsiClientSharp.Exceptions;
using OpsiClientSharp.Models;
using OpsiClientSharp.Models.Results;
using OpsiClientSharp.Types;

namespace OpsiClientSharp.RpcInterfaces
{
    public class ClientProductManagerInterface : RpcInterface
    {
        public override string InterfaceName => "productOnClient";

        public string ClientId { get; }

        public ClientProductManagerInterface(OpsiClient opsiClient, string clientId) : base(opsiClient)
        {
            ClientId = clientId;
        }

        /// <summary>
        /// Retrieves all products registered for this clients.
        /// This also returns products which are "created" and neither installed nor any action set
        /// </summary>
        /// <returns></returns>
        public Task<List<ProductOnClientResult>> GetProductsAsync()
        {
            return OpsiClient.ExecuteAsync<List<ProductOnClientResult>>(new Request(GetFullMethodName("getObjects")));
        }

        /// <summary>
        /// Returns the product by the specified product id
        /// </summary>
        /// <param name="productId">The product id of the product</param>
        /// <returns>The product result as list because the opsi response is a list too</returns>
        public Task<List<ProductOnClientResult>> GetProductAsync(string productId)
        {
            return OpsiClient.ExecuteAsync<List<ProductOnClientResult>>(
                new Request(GetFullMethodName("getObjects"), new Dictionary<string, string> {
                    {"productId", productId}
                })
            );
        }


        /// <summary>
        /// Checks whether the product is already defined on the server for this client
        /// </summary>
        /// <param name="productId">The product id which should be checked</param>
        /// <returns></returns>
        public async Task<bool> IsProductCreatedAsync(string productId)
        {
            List<ProductOnClientResult> productOnClientResults = await GetProductAsync(productId);
            return productOnClientResults.Any();
        }

        /// <summary>
        /// Creates a new product for this client
        /// Runs only if the product isn't already created for this client. Otherwise an exception will be thrown
        /// </summary>
        /// <param name="productId">The product id of the new product</param>
        /// <param name="productType">The product type</param>
        /// <returns></returns>
        public async Task CreateProductAsync(string productId, ProductType productType)
        {
            // Check wheter the product id exists
            ProductsInterface productsInterface = new ProductsInterface(OpsiClient);

            if (!await productsInterface.ExistsAsync(productId))
                throw new OpsiClientRequestException($"A product {productId} doesn't exists");

            if (await IsProductCreatedAsync(productId))
                throw new OpsiProductAlreadyExistsException($"The product {productId} is already defined for this client");

            // Create the product definition for this client
            await OpsiClient.ExecuteAsync<string>(new Request(GetFullMethodName("create"), productId, productType.ToOpsiName(), ClientId));
        }

        public async Task SetProductActionAsync(string productId)
        {
            throw new NotImplementedException();
        }
    }
}
