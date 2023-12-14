using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Sevice.Interface
{
    public interface IAddCardATMSevice
    {
        AddCardATM AddCardATM(int userId, AddCardATMDTO addCardATMDTO);
    }
}
