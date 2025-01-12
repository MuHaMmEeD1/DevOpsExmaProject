using DevOpsExmaProject.Identity.Entitys;
using System.Linq.Expressions;

namespace DevOpsExmaProject.Identity.Services.Abstracts
{
    public interface IUserService
    {
        Task<User> GetAsync(Expression<Func<User, bool>> filter);
        Task<List<User>> GetListAsync(Expression<Func<User, bool>>? filter = null);
        Task AddAsync(User entity);
        Task DeleteAsync(User entity);
        Task UpdateAsync(User entity);
    }
}
