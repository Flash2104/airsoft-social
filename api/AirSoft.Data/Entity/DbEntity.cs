
namespace AirSoft.Data.Entity;

public interface IDbEntity<T>
{
    public T? Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime? AddedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}

public class DbEntity<T> : IDbEntity<T>
{
    public T? Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid ModifiedBy { get; set; }

    public DateTime? AddedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

}
