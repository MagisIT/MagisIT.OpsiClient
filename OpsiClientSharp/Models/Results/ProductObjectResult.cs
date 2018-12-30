using System.Collections.Generic;
using OpsiClientSharp.Utils;

namespace OpsiClientSharp.Models.Results
{
    public class ProductObjectResult : JsonSerializable
    {
        public string OnceScript { get; set; }
        public string Ident { get; set; }
        public List<string> WindowsSoftwareIds { get; set; }
        public string Description { get; set; }
        public string SetupScript { get; set; }
        public string Changelog { get; set; }
        public string CustomScript { get; set; }
        public string Advice { get; set; }
        public string UninstallScript { get; set; }
        public string UserLoginScript { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string PackageVersion { get; set; }
        public string ProductVersion { get; set; }
        public string UpdateScript { get; set; }
        public List<string> ProductClassIds { get; set; }
        public string AlwaysScript { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public bool LicenseRequired { get; set; }
    }
}
