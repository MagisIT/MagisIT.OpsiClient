using System;

namespace OpsiClientSharp.Types
{
    public enum ProductAction
    {
        Setup,
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
                case ProductAction.None:
                    return "none";
            }

            throw new ArgumentException($"{productAction} is not a valid product type");
        }
    }
}
