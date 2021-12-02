namespace AirSoft.Service.Contracts.Auth.SignUp;

public class SignUpRequest
{
    public SignUpRequest(string phoneOrEmail, string password)
    {
        PhoneOrEmail = phoneOrEmail;
        Password = password;
    }

    public string PhoneOrEmail { get; }

    public string Password { get; }
}