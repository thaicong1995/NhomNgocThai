
//Công việc cần xử lý : Xử lý token khi log out. - mã token hết hạn lập tức không còn hiệu lực.
                    // - khi đăng nhập lần 2 mã token lần trước đó không còn hiệu lực.
                    // - Kích hoạt tài khoản đăng ký qua email - Đổi password xac thực qua email.
using WebApi.Dto;
using WebApi.Models;
using WebApi.Models.Enum;
using WebApi.MyDbContext;
using WebApi.Reposetory.Interface;
using WebApi.Sevice.Interface;
using WebApi.TokenConfig;

namespace WebApi.Sevice.Service
{
    public class UserService : IUserService
    {
        private readonly Token _token;

        private readonly MyDb _myDb;
        private readonly IShopService _iShopService;
        private readonly IWalletService _iWalletService;
        private readonly EmailService _emailService;
        private readonly IRepository _iRepository;

        public UserService(MyDb myDb, Token token, IShopService shopService, IWalletService walletService, EmailService emailService, IRepository repository)
        {
            _myDb = myDb;
            _token = token;
            _iShopService = shopService;
            _iWalletService = walletService;
            _emailService = emailService;
            _iRepository = repository;
        }


        public User GetUserByID(int UserId)
        {
            return _iRepository.GetUserById(UserId);
        }

        //Chưa test trường hợp gửi mail thất bại (Nếu đăng ký thành công gửi mail fail hoặc hết hạn -- đã xong.)
        // Login xác thực active 1 lần nữa.
        public User RegisterUser(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "Not enough information has been entered.");
                }

                // Mã hóa mật khẩu và thêm người dùng vào cơ sở dữ liệu
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user._userStatus = UserStatus.Inactive;

                string activationToken = Guid.NewGuid().ToString();
                user.ActivationToken = activationToken;

                // Thêm người dùng vào cơ sở dữ liệu trước khi gửi email xác nhận
                _myDb.Users.Add(user);
                _myDb.SaveChanges();

                // Tạo liên kết kích hoạt
                string activationLink = "https://localhost:7212/api/activate?activationToken=" + activationToken;
                _emailService.SendActivationEmail(user.Email, activationLink);
                // Gửi liên kết kích hoạt cho người dùng 

                _iShopService.CreateShopForUser(user);
                _iWalletService.CreateWalletForUser(user);

                return user;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }


        // Active bằng Email
        public bool ActivateUser(string activationToken)
        {
            var user = _iRepository.GetUserByActivationToken(activationToken);

            if (user != null)
            {
                user._userStatus = UserStatus.Active;
                user.ActivationToken = null;
                _myDb.SaveChanges();
                return true;
            }

            return false;
        }

        public bool SendPasswordResetEmail(string email)
        {
            var user = _iRepository.GetUserByEmail(email);

            if (user != null)
            {
                string resetToken = Guid.NewGuid().ToString();
                user.ActivationToken = resetToken;

                _myDb.SaveChanges();

                string resetLink = "https://localhost:7212/api/ResetPasswordForEmail/Reset-Password?token=" + resetToken;
                _emailService.SendPasswordResetEmail(user.Email, resetLink);

                return true;
            }

            return false;
        }


        public User LoginUser(UserDto userDto)
        {
            try
            {
                // Lấy user = email
                User user = _iRepository.GetUserByEmail(userDto.Email);

                if (user == null)
                    throw new Exception("User not found!");

                if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
                    throw new Exception("Wrong password!");

                ActivateUser(user);

                UpdateOrCreateAccessToken(user);

                return user;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }

        private void ActivateUser(User user)
        {
            if (user._userStatus == UserStatus.Inactive)
            {
                string newActivationToken = Guid.NewGuid().ToString();
                user.ActivationToken = newActivationToken;
                _myDb.SaveChanges();

                string activationLink = "https://localhost:7212/api/activate?activationToken=" + newActivationToken;
                _emailService.SendActivationEmail(user.Email, activationLink);

                throw new Exception("Account has not been activated. Please check your email to activate your account.");
            }
        }


        // tạo mới hoặc ghi đè token
        private void UpdateOrCreateAccessToken(User user)
        {
            var existingToken = _iRepository.GetValidTokenByUserId(user.Id);

            if (existingToken != null)
            {
                var token = _token.CreateToken(user);

                if (string.IsNullOrEmpty(token))
                    throw new Exception("Failed to create a token.");

                existingToken.AccessToken = token;
                existingToken.ExpirationDate = DateTime.Now.AddMinutes(30);
            }
            else
            {

                // không có tạo mới token
                var token = _token.CreateToken(user);

                if (string.IsNullOrEmpty(token))
                    throw new Exception("Failed to create a token.");

                var accessToken = new AcessToken
                {
                    UserID = user.Id,
                    AccessToken = token,
                    statusToken = StatusToken.Valid,
                    ExpirationDate = DateTime.Now.AddMinutes(30)
                };

                _myDb.AccessTokens.Add(accessToken);
            }

            _myDb.SaveChanges();
        }


        public bool LogoutUser(int userId)
        {
            try
            {
                // Lấy danh sách các token của người dùng
                var userToken = _iRepository.GetValidTokenByUserId(userId);

                if (userToken != null)
                {
                    var tokenValue = userToken.AccessToken;
                    var principal = _token.ValidateToken(tokenValue); // Xác thực token

                    if (principal != null)
                    {
                        // Token hợp lệ, bạn có thể đánh dấu token đã hết hạn
                        userToken.statusToken = StatusToken.Expired;
                    }
                }

                _myDb.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
