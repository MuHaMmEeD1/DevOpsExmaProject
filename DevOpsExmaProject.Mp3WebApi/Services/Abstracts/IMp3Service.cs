using DevOpsExmaProject.Mp3WebApi.Entitys;
using System.Linq.Expressions;

namespace DevOpsExmaProject.Mp3WebApi.Services.Abstracts
{
    public interface IMp3Service
    {

        Task<Mp3> GetAsync(Expression<Func<Mp3, bool>> filter);
        Task<List<Mp3>> GetListAsync(Expression<Func<Mp3, bool>>? filter = null);
        Task AddAsync(Mp3 entity);
        Task DeleteAsync(Mp3 entity);
        Task UpdateAsync(Mp3 entity);

    }
}
