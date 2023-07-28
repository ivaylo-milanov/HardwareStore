namespace HardwareStore.Infrastructure.Common
{
    using HardwareStore.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class Repository : IRepository
    {
        private readonly HardwareStoreDbContext context;

        public Repository(HardwareStoreDbContext context)
        {
            this.context = context;
        }

        public IQueryable<T> AllReadonly<T>() where T : class
            => this
                .Set<T>()
                .AsQueryable()
                .AsNoTracking();

        public IQueryable<T> All<T>() where T : class
            => this
                .Set<T>()
                .AsQueryable();

        public IQueryable<T> All<T>(Expression<Func<T, bool>> search) where T : class
            => this
                .Set<T>()
                .Where(search)
                .AsQueryable();

        public DbSet<T> Set<T>() where T : class
            => this.context
                .Set<T>();

        public IQueryable<T> AllReadonly<T>(Expression<Func<T, bool>> search) where T : class
            => this
                .Set<T>()
                .Where(search)
                .AsQueryable()
                .AsNoTracking();

        public async Task<T> FindAsync<T>(object id) where T : class
            => await Set<T>()
                .FindAsync(id);

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
