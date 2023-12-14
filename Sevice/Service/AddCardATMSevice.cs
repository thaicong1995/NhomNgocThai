using WebApi.Dto;
using WebApi.Models;
using WebApi.MyDbContext;
using WebApi.Sevice.Interface;

namespace WebApi.Sevice.Service
{
    public class AddCardATMSevice : IAddCardATMSevice
    {
        private readonly MyDb _myDb;

        public AddCardATMSevice(MyDb myDb)
        {
            _myDb = myDb;
        }

        // Kiểm tra thẻ thực sự tồn tại dùng OTP để xac nhận// hoặc 1 cách nào đó để kiểm tra 
        public AddCardATM AddCardATM(int userId, AddCardATMDTO addCardATMDTO)
        {
            try
            {
                var user = _myDb.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                if (addCardATMDTO == null || string.IsNullOrWhiteSpace(addCardATMDTO.Name) || string.IsNullOrWhiteSpace(addCardATMDTO.CardNumber))
                {
                    throw new ArgumentException("Invalid card information");
                }
                var newCard = new AddCardATM
                {
                    userId = userId,
                    BankName = addCardATMDTO.BankName,
                    Name = addCardATMDTO.Name,
                    CardNumber = addCardATMDTO.CardNumber,
                    DateAdd = DateTime.Now
                };

                _myDb.Add(newCard);
                _myDb.SaveChanges();
                return newCard;
            }catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
            
        }
    }
}
