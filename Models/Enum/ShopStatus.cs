using System.Runtime.Serialization;

namespace WebApi.Models.Enum
{
    public enum ShopStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Inactive")]
        InActive,
    }
}
