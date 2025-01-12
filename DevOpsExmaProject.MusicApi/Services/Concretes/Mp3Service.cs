using DevOpsExmaProject.Mp3Api.Entitys;
using DevOpsExmaProject.Mp3Api.Repositorise.Abstracts;
using DevOpsExmaProject.Mp3Api.Services.Abstracts;
using System.Linq.Expressions;

namespace DevOpsExmaProject.Mp3Api.Services.Concretes
{
    public class Mp3Service : IMp3Service
    {
        private readonly IMp3Dal _mp3Dal;

        public Mp3Service(IMp3Dal mp3Dal)
        {
            _mp3Dal = mp3Dal;
        }

        public async Task AddAsync(Mp3 entity)
        {
            await _mp3Dal.AddAsync(entity);
        }

        public async Task DeleteAsync(Mp3 entity)
        {
            await _mp3Dal.DeleteAsync(entity);
        }

        public async Task<Mp3> GetAsync(Expression<Func<Mp3, bool>> filter)
        {
            return await _mp3Dal.GetAsync(filter);
        }

        public async Task<List<Mp3>> GetListAsync(Expression<Func<Mp3, bool>>? filter = null)
        {
            return await _mp3Dal.GetListAsync(filter);
        }

        public async Task UpdateAsync(Mp3 entity)
        {
            await _mp3Dal.UpdateAsync(entity);
        }
    }
}
