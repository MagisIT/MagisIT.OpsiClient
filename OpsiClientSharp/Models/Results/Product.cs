using System.Collections.Generic;
using OpsiClientSharp.Types;
using OpsiClientSharp.Utils;

namespace OpsiClientSharp.Models.Results
{
    public class Product : JsonSerializable
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

        public bool HasSetupScript()
        {
            return SetupScript != "";
        }

        public bool HasCustomScript()
        {
            return CustomScript != "";
        }

        public bool HasUninstallScript()
        {
            return UninstallScript != "";
        }

        public bool HasUpdateScript()
        {
            return UpdateScript != "";
        }

        public bool HasAlwaysScript()
        {
            return AlwaysScript != "";
        }

        public bool HasOnceScript()
        {
            return OnceScript != "";
        }

        /// <summary>
        /// Returns all available product actions for this product
        /// </summary>
        /// <returns></returns>
        public List<ProductAction> GetAvailableProductActions()
        {
            List<ProductAction> productActions = new List<ProductAction>();

            if (HasSetupScript())
                productActions.Add(ProductAction.Setup);

            if (HasCustomScript())
                productActions.Add(ProductAction.Custom);

            if (HasUninstallScript())
                productActions.Add(ProductAction.Uninstall);

            if (HasUpdateScript())
                productActions.Add(ProductAction.Update);

            if (HasAlwaysScript())
                productActions.Add(ProductAction.Always);

            if (HasOnceScript())
                productActions.Add(ProductAction.Once);

            return productActions;
        }
    }
}
