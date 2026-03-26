namespace HardwareStore.Infrastructure.Common
{
    using HardwareStore.Infrastructure.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using System;
    using System.Linq.Expressions;

    public interface IRepository
    {
        DbSet<T> Set<T>() where T : class;

        IQueryable<T> All<T>() where T : class;

        IQueryable<T> All<T>(Expression<Func<T, bool>> search) where T : class;

        IQueryable<T> AllReadonly<T>() where T : class;

        IQueryable<T> AllReadonly<T>(Expression<Func<T, bool>> search) where T : class;

        Task<T> FindAsync<T>(object id) where T : class;

        Task SaveChangesAsync();

        void Remove<T>(T model) where T : class;

        Task AddAsync<T>(T model) where T : class;

        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> search) where T : class;

        Task<bool> AnyAsync<T>() where T : class;

        void AddRange<T>(IEnumerable<T> entities) where T : class;

        T FirstOrDefault<T>(Expression<Func<T, bool>> search) where T : class;

        Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> search) where T : class;

        Task<ICollection<T>> FromSqlRawAsync<T>(string sql, params object[] parameters) where T : class;

        void RemoveRange<T>(ICollection<T> items) where T : class;

        /// <summary>
        /// In-memory provider only: returns a no-op transaction. On SQL Server, throws — use
        /// <see cref="ExecuteInRetryableTransactionAsync"/> instead (required with retry-on-failure).
        /// </summary>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Runs work inside a database transaction in a way that is compatible with
        /// <see cref="Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsBuilderExtensions.EnableRetryOnFailure"/>.
        /// </summary>
        Task ExecuteInRetryableTransactionAsync(Func<Task> action);
    }
}
