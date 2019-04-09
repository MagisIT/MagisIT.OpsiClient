using MagisIT.OpsiClient.Utils;

namespace MagisIT.OpsiClient.Models.Results
{
    public class Host : JsonSerializable
    {
        public string Ident { get; set; }
        public string Description { get; set; }
        public string Created { get; set; }
        public string InventoryNumber { get; set; }
        public string IpAddress { get; set; }
        public string Notes { get; set; }
        public string OneTimePassword { get; set; }
        public string LastSeen { get; set; }
        public string HardwareAddress { get; set; }
        public string OpsiHostKey { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
    }
}
