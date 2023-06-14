namespace HardwareStore.Infrastructure.Common
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;

    public interface IRepository
    {
        DbSet<T> Set<T>() where T : class;

        IQueryable<T> All<T>() where T : class;

        IQueryable<T> All<T>(Expression<Func<T, bool>> search) where T : class;

        IQueryable<T> AddReadonly<T>() where T : class;
    }
}
