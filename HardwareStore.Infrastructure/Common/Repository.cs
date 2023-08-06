namespace HardwareStore.Infrastructure.Common
{
    using HardwareStore.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
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
            => await this.Set<T>()
                .FindAsync(id);

        public async Task SaveChangesAsync()
            => await this.context.SaveChangesAsync();

        public void Remove<T>(T model) where T : class
            => this.Set<T>().Remove(model);

        public async Task AddAsync<T>(T model) where T : class
            => await this.Set<T>().AddAsync(model);

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> search) where T : class
            => await this.Set<T>()
                .AnyAsync(search);

        public async Task<bool> AnyAsync<T>() where T : class
            => await this.Set<T>()
                .AnyAsync();

        public void AddRange<T>(IEnumerable<T> entities) where T : class
            => this.Set<T>().AddRange(entities);

        public T FirstOrDefault<T>(Expression<Func<T, bool>> search) where T : class
            => this.Set<T>().FirstOrDefault(search);

        public async Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> search) where T : class
            => await this.Set<T>().FirstOrDefaultAsync(search);

        public async Task<ICollection<T>> FromSqlRawAsync<T>(string sql, params object[] parameters) where T : class
            => await this.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();

        public void RemoveRange<T>(ICollection<T> items) where T : class
            => this.Set<T>().RemoveRange(items); 
    }
}
