namespace OpsiClientSharp.RpcInterfaces
{
    public abstract class RpcInterface
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
    }
}
