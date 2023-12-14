
//Mail duoc xac thuc danh tinh thi moi nhan duoc email. Mailgun k cho gui nhung mail rac chua duoc xac thuc
using RestSharp;
using System.Text;

public class EmailService
{
    private readonly string apiKey;
    private readonly string domain;

    public EmailService()
    {
        this.apiKey = "9f32b64d3468220276b2386f9ef6b2a1-1c7e8847-817e449e";
        this.domain = "sandboxa58c4a4fc7ab4c1f92bcb203d73f88e0.mailgun.org";
    }
    public bool SendActivationEmail(string toEmail, string activationLink)
    {
        try
        {
            var client = new RestClient("https://api.mailgun.net/v3");
            var request = new RestRequest();
            request.Resource = $"{domain}/messages";
            request.AddParameter("from", "thaicong1995@gmail.com");
            request.AddParameter("to", toEmail);
            request.AddParameter("subject", "Activate Your Account");
            request.AddParameter("html", $"Click the link to activate your account: {activationLink}");
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("api:" + apiKey)));
            request.Method = Method.Post;

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                Console.WriteLine($"Email sent to {toEmail} successfully.");
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to send email to {toEmail}. Error: {response.ErrorMessage}");
                return false;
            }
        }
        catch (Exception ex)
        {
            // Xử lý lỗi xảy ra trong quá trình gửi email.
            Console.WriteLine($"An error occurred while sending email to {toEmail}. Error: {ex.Message}");
            return false;
        }
    }

    public bool SendPasswordResetEmail(string toEmail, string resetLink)
    {
        try
        {
            var client = new RestClient("https://api.mailgun.net/v3");
            var request = new RestRequest();
            request.Resource = $"{domain}/messages";
            request.AddParameter("from", "thaicong1995@gmail.com");
            request.AddParameter("to", toEmail);
            request.AddParameter("subject", "Reset Your Password");
            request.AddParameter("html", $"Click the link to reset your password: {resetLink}");
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("api:" + apiKey)));
            request.Method = Method.Post;

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                Console.WriteLine($"Password reset email sent to {toEmail} successfully.");
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to send password reset email to {toEmail}. Error: {response.ErrorMessage}");
                return false;
            }
        }
        catch (Exception ex)
        {
            // Xử lý lỗi xảy ra trong quá trình gửi email.
            Console.WriteLine($"An error occurred while sending password reset email to {toEmail}. Error: {ex.Message}");
            return false;
        }
    }

}
