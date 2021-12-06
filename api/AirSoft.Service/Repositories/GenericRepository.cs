using System.Linq.Expressions;
using AirSoft.Data;
using Microsoft.EntityFrameworkCore;

namespace AirSoft.Service.Repositories;

public class GenericRepository<TEntity> where TEntity : class
{
    protected readonly IDbContext? _context;
    protected readonly DbSet<TEntity>? _dbSet;

    public GenericRepository(IDbContext context)
    {
        this._context = context;
        this._dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity>? query = _dbSet;
        if (query == null)
        {
            return Enumerable.Empty<TEntity>();
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        return await query.ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id)
    {
        return await _dbSet!.FindAsync(id);
    }

    public virtual TEntity? Insert(TEntity entity)
    {
        return _dbSet?.Add(entity).Entity;
    }

    public virtual void Delete(object id)
    {
        TEntity? entityToDelete = _dbSet?.Find(id);
        if (entityToDelete == null)
        {
            return;
        }
        Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (_context?.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet?.Attach(entityToDelete);
        }
        _dbSet?.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        _dbSet?.Attach(entityToUpdate);
        if (_context == null) return;
        _context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}