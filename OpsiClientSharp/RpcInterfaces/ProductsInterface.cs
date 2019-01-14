using System.Collections.Generic;
using System.Threading.Tasks;
using OpsiClientSharp.Models.Results;
using OpsiClientSharp.Types;

namespace OpsiClientSharp.RpcInterfaces
{
    public class ProductsInterface : RpcInterface<Product>
    {
        public override string InterfaceName => "product";

        internal ProductsInterface(OpsiHttpClient opsiHttpClient) : base(opsiHttpClient) { }

        /// <summary>
        /// Returns all products on this opsi server depending on the product type
        /// </summary>
        /// <param name="productType">Which product type should be returned</param>
        /// <returns></returns>
        public Task<List<Product>> GetAllAsync(ProductType productType)
        {
            return base.GetAllAsync(new RequestFilter().Add("type", productType.ToOpsiName()));
        }

        /// <summary>
        /// Retrieves a single product from the server
        /// </summary>
        /// <param name="productId">The id of the product which should be returned</param>
        /// <returns></returns>
        public Task<Product> GetAsync(string productId)
        {
            return base.GetAsync(new RequestFilter().Add("id", productId));
        }

        /// <summary>
        /// Check whether the product id exists on the opsi server
        /// </summary>
        /// <param name="productId">The product id which should be checked</param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(string productId)
        {
            return base.ExistsAsync(new RequestFilter().Add("id", productId));
        }
    }
}
