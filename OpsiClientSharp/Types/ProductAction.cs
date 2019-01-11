using System;

namespace OpsiClientSharp.Types
{
    public enum ProductAction
    {
        Setup,
        Update,
        Uninstall,
        Always,
        Once,
        Custom,
        None
    }

    public static class ProductActionExtension
    {
        public static string ToOpsiName(this ProductAction productAction)
        {
            switch (productAction)
            {
                case ProductAction.Setup:
                    return "setup";
                case ProductAction.Update:
                    return "update";
                case ProductAction.Uninstall:
                    return "uninstall";
                case ProductAction.Always:
                    return "always";
                case ProductAction.Once:
                    return "once";
                case ProductAction.Custom:
                    return "custom";
                case ProductAction.None:
                    return "none";
            }

            throw new ArgumentException($"{productAction} is not a valid product type");
        }
    }
}
