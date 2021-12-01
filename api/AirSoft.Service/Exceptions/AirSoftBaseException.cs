
namespace AirSoft.Service.Exceptions;

public class AirSoftBaseException : ApplicationException
{
    public AirSoftBaseException(int code, string message)
        : base(message)
    {
        Code = code;
    }

    public int Code { get; }
}
