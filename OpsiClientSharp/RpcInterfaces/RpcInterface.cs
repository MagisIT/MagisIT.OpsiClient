using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpsiClientSharp.Models;

namespace OpsiClientSharp.RpcInterfaces
{
    public abstract class RpcInterface<TResultObject>
    {
        public abstract string InterfaceName { get; }

        protected OpsiClient OpsiClient { get; }

        public RpcInterface(OpsiClient opsiClient)
        {
            OpsiClient = opsiClient;
        }

        /// <summary>
        /// Returns the full method name by pattern interface_methodname
        /// </summary>
        /// <param name="methodName">The name to append to the interface name</param>
        /// <returns></returns>
        public string GetFullMethodName(string methodName)
        {
            return $"{InterfaceName}_{methodName}";
        }

        /// <summary>
        /// Returns all objects of this interface
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<TResultObject>> GetAllAsync()
        {
            return OpsiClient.ExecuteAsync<List<TResultObject>>(new Request(GetFullMethodName("getObjects")));
        }

        /// <summary>
        /// Returns all objects specified by a request filter
        /// </summary>
        /// <param name="requestFilter"></param>
        /// <returns></returns>
        public Task<List<TResultObject>> GetAllAsync(RequestFilter requestFilter)
        {
            return OpsiClient.ExecuteAsync<List<TResultObject>>(new Request(GetFullMethodName("getObjects")).Filter(requestFilter));
        }

        /// <summary>
        /// Returns one element specified by the request filter
        /// </summary>
        /// <param name="requestFilter"></param>
        /// <returns>The element or null if the result was empty</returns>
        public async Task<TResultObject> GetAsync(RequestFilter requestFilter)
        {
            List<TResultObject> resultObjects = await OpsiClient.ExecuteAsync<List<TResultObject>>(new Request(GetFullMethodName("getObjects")).Filter(requestFilter));
            return resultObjects.FirstOrDefault();
        }

        /// <summary>
        /// Check whether any object with the specified filter exists
        /// </summary>
        /// <param name="requestFilter"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(RequestFilter requestFilter)
        {
            return await GetAsync(requestFilter) != null;
        }
    }
}
