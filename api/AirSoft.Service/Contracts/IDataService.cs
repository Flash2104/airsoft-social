using AirSoft.Service.Repositories;

namespace AirSoft.Service.Contracts;

public interface IDataService
{
    UserRepository Users { get; }
}