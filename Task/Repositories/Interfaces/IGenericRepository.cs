using System.Linq.Expressions;

namespace Tasks.Repositories.Interfaces;

public interface IGenericRrepository<TEntity> where TEntity : class
{
    TEntity? GetById(long id);
    IQueryable<TEntity> GetAll();
    ValueTask<TEntity> AddAsync(TEntity entity);
    ValueTask<TEntity> Remove(TEntity entity);
    ValueTask<TEntity> Update(TEntity entity);
}