using System.Runtime.Serialization;

namespace WebApi.Models.Enum
{
    public enum ProductStatus
    {
        [EnumMember(Value = "OutOfStock")]
        OutOfStock,
        [EnumMember(Value = "InOfStock")]
        InOfStock
    }
}
