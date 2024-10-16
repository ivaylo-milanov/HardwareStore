namespace HardwareStore.Infrastructure.Seed.Contracts
{
    using HardwareStore.Infrastructure.Data;

    public interface IDataSeeder
    {
        Task SeedCharacteristicsNames(HardwareStoreDbContext context);

        Task SeedManufacturers(HardwareStoreDbContext context);

        Task SeedCategories(HardwareStoreDbContext context);

        Task SeedProducts(HardwareStoreDbContext context);
    }
}
