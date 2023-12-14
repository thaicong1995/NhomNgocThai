using System.Runtime.Serialization;

namespace WebApi.Models.Enum
{
    public enum UserStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Inactive")]
        Inactive,
        [EnumMember(Value = "IsLocked")]
        IsLocked
    }
}
