namespace HardwareStore.Infrastructure.Common
{
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// Used when the provider does not support transactions (e.g. InMemory in tests).
    /// </summary>
    public sealed class NoOpDbContextTransaction : IDbContextTransaction
    {
        public Guid TransactionId => Guid.Empty;

        public void Commit()
        {
        }

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Rollback()
        {
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
