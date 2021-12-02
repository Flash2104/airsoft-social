namespace AirSoftApi.Models.Auth.SignUp;

public class SignUpRequestDto
{
    public SignUpRequestDto(string phoneOrEmail, string password)
    {
        PhoneOrEmail = phoneOrEmail;
        Password = password;
    }

    public string PhoneOrEmail { get; }

    public string Password { get; }
}