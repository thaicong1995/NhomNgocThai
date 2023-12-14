using System.Runtime.Serialization;

namespace WebApi.Models.Enum
{
    public enum StatusToken
    {
        [EnumMember(Value = "Valid")]
        Valid,
        [EnumMember(Value = "Expired")]
        Expired
    }
}
