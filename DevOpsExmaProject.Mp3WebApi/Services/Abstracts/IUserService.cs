using DevOpsExmaProject.Mp3WebApi.Entitys;
using System.Linq.Expressions;

namespace DevOpsExmaProject.Mp3WebApi.Services.Abstracts
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
