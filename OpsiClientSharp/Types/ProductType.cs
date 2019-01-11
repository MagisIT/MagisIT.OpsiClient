using System;

namespace OpsiClientSharp.Types
{
    public enum ProductType
    {
        NetBoot,
        LocalBoot
    }

    public static class ProductTypeExtension
    {
        public static string ToOpsiName(this ProductType productType)
        {
            switch (productType)
            {
                case ProductType.NetBoot:
                    return "NetbootProduct";
                case ProductType.LocalBoot:
                    return "LocalbootProduct";
            }

            throw new ArgumentException($"{productType} is not a valid product type");
        }
    }
}
