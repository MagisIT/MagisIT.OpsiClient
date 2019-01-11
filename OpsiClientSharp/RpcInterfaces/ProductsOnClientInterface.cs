using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OpsiClientSharp.Exceptions;
using OpsiClientSharp.Models;
using OpsiClientSharp.Models.Results;
using OpsiClientSharp.Types;

namespace OpsiClientSharp.RpcInterfaces
{
    public class ProductsOnClientInterface : RpcInterface<ProductOnClient>
    {
        public override string InterfaceName => "productOnClient";

        public string ClientId { get; }

        public ProductsOnClientInterface(OpsiClient opsiClient, string clientId) : base(opsiClient)
        {
            ClientId = clientId;
        }

        /// <summary>
        /// Returns all products for this client
        /// </summary>
        /// <returns></returns>
        public override Task<List<ProductOnClient>> GetAllAsync()
        {
            return base.GetAllAsync(new RequestFilter().Add("clientId", ClientId));
        }

        /// <summary>
        /// Returns the product by the specified product id
        /// </summary>
        /// <param name="productId">The product id of the product</param>
        /// <returns>The product result as list because the opsi response is a list too</returns>
        public Task<ProductOnClient> GetAsync(string productId)
        {
            return base.GetAsync(new RequestFilter().Add("clientId", ClientId).Add("productId", productId));
        }

        public Task<bool> ExistsAsync(string productId)
        {
            return ExistsAsync(new RequestFilter().Add("clientId", ClientId).Add("productId", productId));
        }

        /// <summary>
        /// Checks whether the product is already defined on the server for this client
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> IsProductCreatedAsync(Product product)
        {
            return await GetAsync(product.Id) != null;
        }

        /// <summary>
        /// Creates a new product for this client
        /// Runs only if the product isn't already created for this client. Otherwise an exception will be thrown
        /// </summary>
        /// <returns></returns>
        public async Task CreateProductAsync(Product product)
        {
            if (await IsProductCreatedAsync(product))
                throw new OpsiProductAlreadyExistsException($"The product {product.Id} is already defined for this client {ClientId}");

            // Create the product definition for this client
            await OpsiClient.ExecuteAsync<List<string>>(new Request(GetFullMethodName("create")).AddParameters(product.Id, product.Type, ClientId));
        }

        /// <summary>
        /// Sets a product action e.g for setup
        /// </summary>
        /// <param name="product"></param>
        /// <param name="productAction"></param>
        /// <returns></returns>
        public async Task SetProductAction(Product product, ProductAction productAction)
        {
            // If the object doesn't exist for this client. Create it
            if (!await ExistsAsync(product.Id))
            {
                await CreateProductAsync(product);
            }

            ProductOnClient productOnClient = await GetAsync(product.Id);
            productOnClient.ActionRequest = productAction.ToOpsiName();

            await OpsiClient.ExecuteAsync<string>(new Request(GetFullMethodName("updateObject")).AddParameter(productOnClient));
        }

        public async Task SetProductsAction(List<Product> products, ProductAction productAction)
        {
            // Get all products for this client
            List<ProductOnClient> productsOnClient = await GetAllAsync();

            // Get all products that aren't already created for this client
            List<Product> notCreatedProducts = products.Where((product) => productsOnClient.All(productOnClient => productOnClient.ProductId != product.Id)).ToList();

            // Create objects if any need to be created
            foreach (var product in notCreatedProducts)
                await CreateProductAsync(product);

            // Update productsOnClient only if there are new products for this client
            if (notCreatedProducts.Any())
                productsOnClient = await GetAllAsync();

            // Get all Products On Client that should be applied
            productsOnClient = productsOnClient.Where((productOnClient) => products.Any((product) => product.Id == productOnClient.ProductId)).ToList();

            // Set Action for all products
            productsOnClient.ForEach((productOnClient) => productOnClient.ActionRequest = productAction.ToOpsiName());

            await OpsiClient.ExecuteAsync<List<string>>(new Request(GetFullMethodName("updateObjects")).AddParametersAsJArray(productsOnClient));
        }
    }
}
