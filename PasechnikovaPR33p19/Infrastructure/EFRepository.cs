using PasechnikovaPR33p19.Data;
using PasechnikovaPR33p19.Domain.Entities;
using PasechnikovaPR33p19.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PasechnikovaPR33p19.Infrastructure
{
    public class EFRepository<T> : IRepository<T> where T : Entity
    {
        public async Task<T> AddAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Added;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public async Task<T?> FindAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> FindWhere(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        private readonly ELibraryContext context;

        public EFRepository(ELibraryContext context)
        {
            this.context = context;
        }

    }
}
