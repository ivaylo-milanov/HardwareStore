namespace HardwareStore.Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;
    using HardwareStore.Common;

    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
               .Property(e => e.UserName)
               .HasMaxLength(GlobalConstants.CustomerUserNameMaxLength)
               .IsRequired();

            builder
                .Property(e => e.Email)
                .HasMaxLength(GlobalConstants.CustomerEmailMaxLength)
                .IsRequired();
        }
    }
}
