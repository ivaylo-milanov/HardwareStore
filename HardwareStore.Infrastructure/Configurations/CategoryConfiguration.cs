namespace HardwareStore.Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .HasData(
                    new Category
                    {
                        Id = 1,
                        Name = "Processor"
                    },
                    new Category
                    {
                        Id = 2,
                        Name = "Mother Board"
                    },
                    new Category
                    {
                        Id = 3,
                        Name = "Power Supply"
                    },
                    new Category
                    {
                        Id = 4,
                        Name = "Case"
                    },
                    new Category
                    {
                        Id = 5,
                        Name = "Hard Disk"
                    },
                    new Category
                    {
                        Id = 6,
                        Name = "SSD"
                    },
                    new Category
                    {
                        Id = 7,
                        Name = "RAM"
                    },
                    new Category
                    {
                        Id = 8,
                        Name = "Processor Cooler"
                    },
                    new Category
                    {
                        Id = 9,
                        Name = "Video Card"
                    },
                    new Category
                    {
                        Id = 10,
                        Name = "Computer"
                    });

        }
    }
}
