using System.Runtime.Serialization;

namespace WebApi.Models.Enum
{
    public enum CartItemStatus
    {
        [EnumMember(Value = "WaitPay")]
        WaitPay,
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "Refun")]
        Refun,
        [EnumMember(Value = "Fail")]
        Fail
    }
}
