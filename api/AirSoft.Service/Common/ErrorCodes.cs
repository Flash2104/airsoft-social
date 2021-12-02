
namespace AirSoft.Service.Common;

public class ErrorCodes
{
    public const int CommonError = 81000;
    public const int RequestArgumentInvalid = 81001;
    public const int JwtSettingsIsNull = 81002;
    public const int InvalidParameters = 81003;

    public sealed class AuthService
    {
        public const int WrongLoginOrPass = 82002;
        public const int UserNotFound = 82003;
        public const int EmptyPassword = 82004;
    }

    public sealed class UserRepository
    {
        public const int MoreThanOneUserByPhone = 82101;
        public const int MoreThanOneUserByEmail = 82102;
    }
}
