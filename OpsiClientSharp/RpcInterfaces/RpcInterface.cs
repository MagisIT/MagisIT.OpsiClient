using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpsiClientSharp.Exceptions;
using OpsiClientSharp.Models;

namespace OpsiClientSharp.RpcInterfaces
{
    public abstract class RpcInterface<TResultObject>
    {
        public abstract string InterfaceName { get; }

        protected OpsiClient OpsiClient { get; }

        public RpcInterface(OpsiClient opsiClient) => OpsiClient = opsiClient;

        /// <summary>
        ///     Returns the full method name by pattern interface_methodname
        /// </summary>
        /// <param name="methodName">The name to append to the interface name</param>
        /// <returns></returns>
        public string GetFullMethodName(string methodName) => $"{InterfaceName}_{methodName}";

        /// <summary>
        ///     Returns all objects of this interface
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<TResultObject>> GetAllAsync() => GetAllAsync(new RequestFilter());

        /// <summary>
        ///     Returns all objects specified by a request filter
        /// </summary>
        /// <param name="requestFilter"></param>
        /// <returns></returns>
        public async Task<List<TResultObject>> GetAllAsync(RequestFilter requestFilter)
        {
            List<TResultObject> resultObjects = await GetAllAsyncInternal(requestFilter);
            if (!resultObjects.Any())
                throw new OpsiClientRequestException($"Cannot find objects on interface {InterfaceName} with request filter {requestFilter.ToJson()}");

            return resultObjects;
        }

        /// <summary>
        ///     Returns one element specified by the request filter
        /// </summary>
        /// <param name="requestFilter"></param>
        /// <returns>The element or null if the result was empty</returns>
        public async Task<TResultObject> GetAsync(RequestFilter requestFilter) => (await GetAllAsync(requestFilter)).First();

        /// <summary>
        ///     Check whether any object with the specified filter exists
        /// </summary>
        /// <param name="requestFilter"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(RequestFilter requestFilter) => (await GetAllAsyncInternal(requestFilter)).Any();

        private Task<List<TResultObject>> GetAllAsyncInternal(RequestFilter requestFilter) =>
            OpsiClient.ExecuteAsync<List<TResultObject>>(new Request(GetFullMethodName("getObjects")).Filter(requestFilter));
    }
}
