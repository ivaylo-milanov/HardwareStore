namespace HardwareStore.Infrastructure.Seed
{
    using HardwareStore.Common;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.DTOs;
    using HardwareStore.Infrastructure.Models;

    public class DataSeeder
    {
        private readonly IRepository repository;
        private readonly IFileReader fileReader;

        public DataSeeder(IRepository repository, IFileReader fileReader)
        {
            this.repository = repository;
            this.fileReader = fileReader;
        }

        public async Task SeedData()
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                try
                {
                    if (!await this.repository.AnyAsync<CharacteristicName>())
                    {
                        await SeedCharacteristicsNames();
                    }

                    if (!await this.repository.AnyAsync<Manufacturer>())
                    {
                        await SeedManufacturers();
                    }

                    if (!await this.repository.AnyAsync<Category>())
                    {
                        await SeedCategories();
                    }

                    if (!await this.repository.AnyAsync<Product>())
                    {
                        await SeedProducts();
                    }

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
            var characteristicsNamesFiles = this.fileReader.GetFilesNames("characteristics-names.json");

            foreach (var file in characteristicsNamesFiles)
            {
                var characteristicsNamesDtos = this.fileReader.LoadJson<CharacteristicNameDto>(file);

                var characteristicsNames = characteristicsNamesDtos
                    .Select(chn => new CharacteristicName
                    {
                        Name = chn.Name
                    });

                this.repository.AddRange(characteristicsNames);
            }

            await this.repository.SaveChangesAsync();
        }

        private async Task SeedManufacturers()
        {
            var manufacturersFiles = this.fileReader.GetFilesNames("manufacturers.json");

            foreach (var file in manufacturersFiles)
            {
                var manufacturersDtos = this.fileReader.LoadJson<ManufacturerDto>(file);

                var manufacturers = manufacturersDtos
                    .Select(m => new Manufacturer
                    {
                        Name = m.Name
                    });

                this.repository.AddRange(manufacturers);
            }

            await this.repository.SaveChangesAsync();
        }

        private async Task SeedCategories()
        {
            var categoriesFiles = this.fileReader.GetFilesNames("categories.json");

            foreach (var file in categoriesFiles)
            {
                var categoriesDtos = this.fileReader.LoadJson<CategoryDto>(file);

                var categories = categoriesDtos
                    .Select(c => new Category
                    {
                        Name = c.Name
                    });

                this.repository.AddRange(categories);
            }

            await this.repository.SaveChangesAsync();
        }

        private async Task SeedProducts()
        {
            var productFiles = this.fileReader.GetFilesNames("*-products.json");

            foreach (var file in productFiles)
            {
                var productDtos = this.fileReader.LoadJson<ProductDto>(file);
                var products = await GetCategoryProducts(productDtos);

                this.repository.AddRange(products);
            }

            await this.repository.SaveChangesAsync();
        }

        private async Task<IEnumerable<Product>> GetCategoryProducts(IEnumerable<ProductDto> productDtos)
        {
            var products = new List<Product>();

            foreach (var productDto in productDtos)
            {
                var category = await this.repository.FirstOrDefaultAsync<Category>(c => c.Name == productDto.Name);

                if (category == null)
                {
                    throw new ArgumentNullException(ExceptionMessages.CategoryNotFound);
                }

                var manufacturer = await this.repository.FirstOrDefaultAsync<Manufacturer>(m => m.Name == productDto.Name);

                if (manufacturer == null)
                {
                    throw new ArgumentNullException(ExceptionMessages.ManufacturerNotFound);
                }

                if (await this.repository.AnyAsync<Product>(p => p.Name == productDto.Name))
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
                    var characteristicName = this.repository.FirstOrDefault<CharacteristicName>(chn => chn.Name == characteristicDto.Name);

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
    }
}