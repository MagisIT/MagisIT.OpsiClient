using MagisIT.OpsiClient.Utils;

namespace MagisIT.OpsiClient.Models.Results
{
    public class ProductOnClient : JsonSerializable
    {
        public string Ident { get; set; }
        public string ActionProgress { get; set; }
        public string ActionResult { get; set; }
        public string ClientId { get; set; }
        public string ModificationTime { get; set; }
        public string ActionRequest { get; set; }
        public string TargetConfiguration { get; set; }
        public string ProductVersion { get; set; }
        public string ProductType { get; set; }
        public string LastAction { get; set; }
        public string PackageVersion { get; set; }
        public int ActionSequence { get; set; }
        public string Type { get; set; }
        public string InstallationStatus { get; set; }
        public string ProductId { get; set; }
    }
}
