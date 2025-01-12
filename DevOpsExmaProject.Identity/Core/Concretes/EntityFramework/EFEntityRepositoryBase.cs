using DevOpsExmaProject.Identity.Core.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DevOpsExmaProject.Identity.Core.Concretes.EntityFramework
{
    public class EFEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
      where TEntity : class, IEntity, new()
      where TContext : DbContext
    {

        private readonly TContext _context;

        public EFEntityRepositoryBase(TContext context)
        {
            _context = context;

        }

        public async Task AddAsync(TEntity entity)
        {
            var addedEntity = _context.Entry(entity);
            addedEntity.State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            var deletedEntity = _context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(filter);
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return filter == null ?
                await _context.Set<TEntity>().ToListAsync() :
                await _context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var updateedEntity = _context.Entry(entity);
            updateedEntity.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
