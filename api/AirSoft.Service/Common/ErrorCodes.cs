
namespace AirSoft.Service.Common;

internal class ErrorCodes
{
    public const int CommonError = 81000;
    public const int RequestArgumentInvalid = 81001;
    public const int JwtSettingsIsNull = 81002;
    public const int InvalidParameters = 81003;

    public sealed class AuthService
    {
        public const int WrongLoginOrPass = 82002;
    }
}
