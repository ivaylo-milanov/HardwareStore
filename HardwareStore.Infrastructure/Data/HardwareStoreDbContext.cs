namespace HardwareStore.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;
    using Infrastructure.Configurations;
    using System;
    using HardwareStore.Infrastructure.DTOs;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
    using HardwareStore.Common;

    public class HardwareStoreDbContext : IdentityDbContext<Customer>
    {
        public HardwareStoreDbContext(DbContextOptions<HardwareStoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Characteristic> Characteristics { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<Manufacturer> Manufacturers { get; set; } = null!;

        public DbSet<CharacteristicName> CharacteristicsNames { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProductOrderConfiguration());
            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new ShoppingCartItemConfiguration());
            builder.ApplyConfiguration(new FavoriteConfiguration());
            
            base.OnModelCreating(builder);
        }

        public async Task SeedData()
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                try
                {
                    if (!this.CharacteristicsNames.Any())
                    {
                        await SeedCharacteristicsNames();
                    }

                    if (!this.Manufacturers.Any())
                    {
                        await SeedManufacturers();
                    }

                    if (!this.Categories.Any())
                    {
                        await SeedCategories();
                    }

                    //if (!this.Products.Any())
                    //{
                    //    SeedProducts();
                    //}

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        private async Task SeedCharacteristicsNames()
        {
            var characteristicsNamesFiles = GetJsonFilesNames("characteristics-names.json");

            foreach (var file in characteristicsNamesFiles)
            {
                var characteristicsNamesDtos = JsonFileReader.LoadJson<CharacteristicNameDto>(file);

                var characteristicsNames = characteristicsNamesDtos
                    .Select(chn => new CharacteristicName
                    {
                        Name = chn.Name
                    });

                this.CharacteristicsNames.AddRange(characteristicsNames);
            }

            await this.SaveChangesAsync();
        }

        private async Task SeedManufacturers()
        {
            var manufacturersFiles = GetJsonFilesNames("manufacturers.json");

            foreach (var file in manufacturersFiles)
            {
                var manufacturersDtos = JsonFileReader.LoadJson<ManufacturerDto>(file);

                var manufacturers = manufacturersDtos
                    .Select(m => new Manufacturer
                    {
                        Name = m.Name
                    });

                this.Manufacturers.AddRange(manufacturers);
            }

            await this.SaveChangesAsync();
        }

        private async Task SeedCategories()
        {
            var categoriesFiles = GetJsonFilesNames("categories.json");

            foreach (var file in categoriesFiles)
            {
                var categoriesDtos = JsonFileReader.LoadJson<CategoryDto>(file);

                var categories = categoriesDtos
                    .Select(c => new Category
                    {
                        Name = c.Name
                    });

                this.Categories.AddRange(categories);
            }

            await this.SaveChangesAsync();
        }

        private async Task SeedProducts()
        {
            var productFiles = GetJsonFilesNames("*-products.json");

            foreach (var file in productFiles)
            {
                var productDtos = JsonFileReader.LoadJson<ProductDto>(file);
                var products = await GetCategoryProducts(productDtos);

                this.Products.AddRange(products);
            }

            await this.SaveChangesAsync();
        }

        private async Task<IEnumerable<Product>> GetCategoryProducts(IEnumerable<ProductDto> productDtos)
        {
            var products = new List<Product>();

            foreach (var productDto in productDtos)
            {
                var category = await this.Categories.FirstOrDefaultAsync(c => c.Name == productDto.Name);

                if (category == null)
                {
                    throw new ArgumentNullException(ExceptionMessages.CategoryNotFound);
                }

                var manufacturer = await this.Manufacturers.FirstOrDefaultAsync(m => m.Name == productDto.Name);

                if (manufacturer == null)
                {
                    throw new ArgumentNullException(ExceptionMessages.ManufacturerNotFound);
                }

                if (await this.Products.AnyAsync(p => p.Name == productDto.Name))
                {
                    continue;
                }

                var product = new Product
                {
                    Name = productDto.Name,
                    ReferenceNumber = productDto.ReferenceNumber,
                    AddDate = DateTime.Now,
                    CategoryId = category.Id,
                    ManufacturerId = manufacturer.Id,
                    Description = productDto.Description,
                    Quantity = productDto.Quantity,
                    Warranty = productDto.Warranty,
                    Model = productDto.Model,
                    Price = productDto.Price 
                };

                ICollection<Characteristic> characteristics = new List<Characteristic>();
                foreach (var characteristicDto in productDto.Characteristics)
                {
                    var characteristicName = this.CharacteristicsNames.FirstOrDefault(chn => chn.Name == characteristicDto.Name);

                    if (characteristicName == null)
                    {
                        throw new ArgumentNullException(ExceptionMessages.CharacteristicNameNotFound);
                    }

                    var characteristic = new Characteristic
                    {
                        CharacteristicNameId = characteristicName.Id,
                        Value = characteristicDto.Value
                    };

                    characteristics.Add(characteristic);
                }

                product.Characteristics = characteristics;
            }

            return products;
        }

        private string[] GetJsonFilesNames(string jsonFileTemplate)
        {
            var fileNames = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Imports"), jsonFileTemplate);

            return fileNames.ToArray();
        }
    }
}