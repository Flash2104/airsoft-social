
namespace AirSoft.Service.Common;

public class ErrorCodes
{
    public const int CommonError = 81000;
    public const int RequestArgumentInvalid = 81001;
    public const int JwtSettingsIsNull = 81002;
    public const int InvalidParameters = 81003;

    public sealed class AuthService
    {
        public const int EmptyLoginOrPass = 82001;
        public const int WrongLoginOrPass = 82002;
        public const int UserNotFound = 82003;
        public const int EmptyPassword = 82004;

        public const int AlreadyExist = 82005;
        public const int PasswordsNotEqual = 82006;
        public const int UserRoleNotFound = 82007;
        public const int CreatedUserIsNull = 82008;
    }

    public sealed class MemberService
    {
        public const int EmptyUserId = 82101;
        public const int NotFound = 82102;
        public const int AlreadyExist = 82103;
    }

    public sealed class UserRepository
    {
        public const int MoreThanOneUserByPhone = 82201;
        public const int MoreThanOneUserByEmail = 82202;
        public const int UserNotFound = 82203;
    }

    public sealed class TeamRepository
    {
        public const int MemberNotFound = 82300;
    }

    public sealed class TeamService
    {
        public const int EmptyUserId = 82310;
        public const int NotFound = 82311;
        public const int EmptyRoleTitle = 82312;
        public const int AlreadyExist = 82312;
    }

    public sealed class NavigationService
    {
        public const int EmptyUserId = 82320;
        public const int UserNotFound = 82321;
        public const int NavigationNotFound = 82322;
        public const int UserRolesNotFound = 82323;
    }
}
