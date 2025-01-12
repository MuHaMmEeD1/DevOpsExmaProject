using System.Linq.Expressions;

namespace DevOpsExmaProject.Mp3Api.Core.Abstracts
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {

        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>>? filter = null);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);

    }
}
