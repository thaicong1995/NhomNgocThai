using System.Runtime.Serialization;

namespace WebApi.Models.Enum
{
    public enum OrderStatus
    {
        [EnumMember(Value = "WaitPay")]
        WaitPay,
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "Refunded")]
        Refunded,
        [EnumMember(Value = "Fail")]
        Fail
    }
}
