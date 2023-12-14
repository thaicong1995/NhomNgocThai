using WebApi.Models;
using WebApi.MyDbContext;

namespace WebApi.Controllers.View
{
    public class ResetService
    {
        private readonly MyDb _myDb;
        public ResetService(MyDb myDb)
        {
            _myDb = myDb;
        }
        public bool IsValidResetToken(string activationToken)
        {
            var user = _myDb.Users.FirstOrDefault(u => u.ActivationToken == activationToken);
            return user != null;
        }

        public User GetUserByActivationToken(string activationToken)
        {
            return _myDb.Users.FirstOrDefault(u => u.ActivationToken == activationToken);
        }

        public void UpdatePassword(int userId, string newPassword)
        {
            var user = _myDb.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.ActivationToken = null;
                _myDb.SaveChanges();
            }
        }
    }
}
