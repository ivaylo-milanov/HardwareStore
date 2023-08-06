namespace HardwareStore.Tests
{
    using HardwareStore.Core.Attributes;
    using HardwareStore.Core.Services;
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Data;
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Tests.Mocking;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Moq;

    public class Tests
    {
        private HardwareStoreDbContext context;
        private ProductService productService;

        [SetUp]
        public async Task SetUp()
        {
            var options = new DbContextOptionsBuilder<HardwareStoreDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new HardwareStoreDbContext(options);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync(); 

            var repository = new Repository(context);

            await SeedDatabase();

            var memoryCacheMock = new Mock<IMemoryCache>();
            var cacheEntry = new Mock<ICacheEntry>();

            memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny)).Returns(false);
            memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntry.Object);

            productService = new ProductService(repository, memoryCacheMock.Object);
        }

        private async Task SeedDatabase()
        {
            var manufacturers = Enumerable.Range(1, 3)
                .Select(i => new Manufacturer { Id = i, Name = $"Manufacturer{i}" }).ToList();

            var characteristicNames = Enumerable.Range(1, 4)
                .Select(i => new CharacteristicName { Id = i, Name = $"CharacteristicName{i}" }).ToList();

            var categories = Enumerable.Range(1, 5)
                .Select(i => new Category { Id = i, Name = $"Category{i}" }).ToList();

            var products = Enumerable.Range(1, 12)
                .Select(i => new Product
                {
                    Id = i,
                    Name = $"Product{i}",
                    Price = 10 * i,
                    Quantity = 100 * i,
                    AddDate = DateTime.Now,
                    Warranty = 1,
                    ManufacturerId = manufacturers[i % manufacturers.Count].Id,
                    ReferenceNumber = $"Ref00{i}",
                    CategoryId = categories[i % categories.Count].Id,
                    Model = $"Model{i}",
                    Characteristics = Enumerable.Range(1, 3)
                        .Select(j => new Characteristic
                        {
                            Id = (i - 1) * 3 + j,
                            CharacteristicNameId = characteristicNames[(j - 1) % characteristicNames.Count].Id,
                            Value = $"Value{(i - 1) * 3 + j}"
                        }).ToList()
                }).ToList();

            products.Add(new Product
            {
                Id = 13,
                Name = "Product13",
                Price = 130,
                Quantity = 100 * 13,
                AddDate = DateTime.Now,
                Warranty = 1,
                ManufacturerId = null,
                ReferenceNumber = "Ref0013",
                CategoryId = 4,
                Model = "Model13",
                Characteristics = new[]
                {
                    new Characteristic
                    {
                        Id = 37,
                        CharacteristicNameId = 2,
                        Value = "Value37"
                    },
                    new Characteristic
                    {
                        Id = 38,
                        CharacteristicNameId = 4,
                        Value = "1"
                    }
                }
            });

            products.Add(new Product
            {
                Id = 14,
                Name = "Product14",
                Price = 140,
                Quantity = 100 * 14,
                AddDate = DateTime.Now,
                Warranty = 1,
                ManufacturerId = 2,
                ReferenceNumber = "Ref0014",
                Model = "Model14",
                CategoryId = 2
            });

            products.Add(new Product
            {
                Id = 15,
                Name = "Product15",
                Price = 150,
                Quantity = 100 * 15,
                AddDate = DateTime.Now,
                Warranty = 1,
                ManufacturerId = 2,
                ReferenceNumber = "Ref0015",
                CategoryId = 5,
                Model = "Model15",
                Characteristics = new[]
                {
                    new Characteristic
                    {
                        Id = 39,
                        CharacteristicNameId = 2,
                        Value = "Value39"
                    },
                    new Characteristic
                    {
                        Id = 40,
                        CharacteristicNameId = 4,
                        Value = "Value40, Value41"
                    }
                }
            });

            context.Manufacturers.AddRange(manufacturers);
            context.CharacteristicsNames.AddRange(characteristicNames);
            context.Categories.AddRange(categories);
            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }

        [Test]
        public void ThrowExceptionIfTheProductViewModelDontHaveCategoryAttribute()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var resultWithoutAttribute = await productService.GetModel<MoqProductModelWithoutAttribute>();
            }, "The model type MoqProductModelWithoutAttribute does not have a Category attribute");
        }

        [Test]
        public async Task GetsEmptyCollectionOfProcutsFromTheDatabase()
        {
            //Act
            var resultWithAttribute = await productService.GetModel<MoqProductModelWithAttribute>();

            //Assert
            Assert.That(resultWithAttribute.Products, Is.Empty);
        }

        [Test]
        public async Task GetsSomeItemsWithCategory1FromTheDatabase()
        {
            //Act
            var resultWithAttribute = await productService.GetModel<MoqProductModelWithProperCategory>();

            //Assert
            Assert.AreEqual(2, resultWithAttribute.Products.Count());
            foreach (var model in resultWithAttribute.Products)
            {
                Assert.IsTrue(model.Name.StartsWith("Product"));
                Assert.IsTrue(model.Price != 0);
                Assert.IsTrue(model.AddDate != null);
                Assert.IsTrue(model.Id > 0);
                Assert.IsTrue(model.CharacteristicName1.StartsWith("Value"));
                Assert.IsTrue(model.CharacteristicName2.StartsWith("Value"));
                Assert.IsTrue(model.CharacteristicName3.StartsWith("Value"));
            }
        }

        [Test]
        public async Task GetItemWithNoManufacturerFromTheDatabase()
        {
            //Arrange
            var result = await productService.GetModel<MoqProductModelWIthCategory4>();

            //Act
            var model = result.Products.FirstOrDefault(p => p.Manufacturer == null);

            //Assert
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task GetItemWithNoCharacteristicsFromTheDatabase()
        {
            //Arrange
            var result = await productService.GetModel<MoqProductModelWithoutCharacteristics>();

            //Act
            var propertiesWithCharacteristicAttribute = typeof(MoqProductModelWithoutCharacteristics)
                .GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(CharacteristicAttribute)));

            //Assert
            Assert.IsTrue(propertiesWithCharacteristicAttribute.Count() == 1);
        }

        [Test]
        public async Task GetItemWithMisMatchBetweenCharacteristicValueAndProperty()
        {
            //Arrange
            var result = await productService.GetModel<MoqProductModelWIthMismatchPropertyValue>();

            //Act
            var product = result.Products.FirstOrDefault(p => p.Id == 13);

            var characteristicPropertyWithTypeInt = typeof(MoqProductModelWIthMismatchPropertyValue)
                .GetProperties()
                .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(CharacteristicAttribute))
                    && prop.PropertyType == typeof(int));

            var characteristicValue = characteristicPropertyWithTypeInt.GetValue(product);

            //Assert
            Assert.That((int)characteristicValue == 1);
        }

        [Test]
        public async Task GetItemWithCharacteristicWithValueNull()
        {
            //Arrange
            var result = await productService.GetModel<MoqProductWIthInvalidCharacteristicName>();

            //Act
            var product = result.Products.FirstOrDefault(p => p.Id == 13);

            //Assert
            Assert.That(product.CharacteristicName5 == null);
        }

        [Test]
        public async Task GetItemWithCharacteristicNameMatchingNonCharacteristicPropertyName()
        {
            //Arrange
            var result = await productService.GetModel<MoqProductModelWithCharacteristicNameCollision>();

            //Act
            var product = result.Products.First();

            //Assert
            Assert.AreNotEqual(product.Id, product.MockId);
        }

        [Test]
        public async Task Get0ProductsIfTheKeywordIsNullInGetProductsByKeyword()
        {
            //Act
            var result = await productService.GetSearchModel(null);

            //Assert
            Assert.AreEqual(15, result.Products.Count());
        }

        [Test]
        public async Task Get0ProductsIfTheKeywordIsEmptySpaceInGetProductsByKeyword()
        {
            //Act
            var result = await productService.GetSearchModel("");

            //Assert
            Assert.AreEqual(15, result.Products.Count());
        }

        [Test]
        public async Task Get0ProductsIfTheKeywordIsWhiteSpaceInGetProductsByKeyword()
        {
            //Act
            var result = await productService.GetSearchModel(" ");

            //Assert
            Assert.AreEqual(15, result.Products.Count());
        }

        [Test]
        public async Task Get0ProductsIfTheKeywordIsContinuousWhiteSpaceInGetProductsByKeyword()
        {
            //Act
            var result = await productService.GetSearchModel("   ");

            //Assert
            Assert.AreEqual(15, result.Products.Count());
        }

        [Test]
        public async Task GetEmptyFilterCategoriesWhenTheProductsCollectionIsEmpty()
        {
            //Act
            var result = await productService.GetModel<MoqProductModelWithInvalidCategory>();

            //Assert
            Assert.AreEqual(1, result.Filters.Count());
            Assert.AreEqual(0, result.Filters.First().Values.Count());
        }

        [Test]
        public async Task GetPropertiesWithCharacteristicAttributeWithDifferentValues()
        {
            //Act
            var result = await productService.GetModel<MoqProductModelWithProperCategory>();

            //Assert
            Assert.AreEqual(4, result.Filters.Count());
            foreach (var filter in result.Filters)
            {
                Assert.AreEqual(2, filter.Values.Count());
            }
        }

        [Test]
        public async Task GetPropertiesWithCharacteristicAttributeWithEqualValues()
        {
            //Act
            var result = await productService.GetModel<MoqProductModelWithMoreThenOneValueInProperty>();

            //Assert
            Assert.IsTrue(result.Filters.Any(p => p.Values.Count() > 1));
        }

        [Test]
        public async Task ThePropertiesInFiltersAreOnlyWithHaveCharacteristicAttribute()
        {
            //Arrange
            var result = await productService.GetModel<MoqProductModelWithMoreThenOneValueInProperty>();

            //Act
            var propertyNames = result.Filters.Select(p => p.Name);

            //Assert
            Assert.That(propertyNames.Contains("Manufacturer"));
            Assert.That(propertyNames.Contains("CharacteristicName2"));
            Assert.That(propertyNames.Contains("CharacteristicName4"));
        }

        [Test]
        public async Task TheCharacteristicAttributeNameIsSetAndTitleIsEqualToTheAttributeName()
        {
            //Act
            var result = await productService.GetModel<MoqProductModelWithMoreThenOneValueInProperty>();

            var propertiesWithCharacteristicAttributeAndNameSet = typeof(MoqProductModelWithMoreThenOneValueInProperty).GetProperties()
                .Where(p =>
                {
                    var attribute = Attribute.GetCustomAttribute(p, typeof(CharacteristicAttribute)) as CharacteristicAttribute;
                    return attribute != null && !string.IsNullOrEmpty(attribute.Name);
                })
                .Select(a => ((CharacteristicAttribute)Attribute.GetCustomAttribute(a, typeof(CharacteristicAttribute))).Name);

            //Assert
            foreach (var item in result.Filters.Where(p => p.Title != p.Name))
            {
                Assert.That(propertiesWithCharacteristicAttributeAndNameSet.Contains(item.Title));
            }
        }

        [Test]
        public async Task TheCharacteristicAttributeNameIsNotSetAndTitleIsEqualToThePropertyName()
        {
            //Act
            var result = await productService.GetModel<MoqProductModelWithMoreThenOneValueInProperty>();

            var propertiesWithCharacteristicAttributeWithoutNameSet = typeof(MoqProductModelWithMoreThenOneValueInProperty).GetProperties()
                .Where(p =>
                {
                    var attribute = Attribute.GetCustomAttribute(p, typeof(CharacteristicAttribute)) as CharacteristicAttribute;
                    return attribute != null && string.IsNullOrEmpty(attribute.Name);
                })
                .Select(p => p.Name);

            //Assert
            foreach (var item in result.Filters.Where(p => p.Title != p.Name))
            {
                Assert.That(propertiesWithCharacteristicAttributeWithoutNameSet.Contains(item.Title));
            }
        }

        [Test]
        public async Task TheProductModelHasNullValuesAndTheseValuesAreNotIncludedInTheFilterModel()
        {
            var result = await productService.GetModel<MoqProductModelWIthCategory4>();

            var manufacturerModel = result.Filters.First(f => f.Name == "Manufacturer");

            Assert.That(manufacturerModel.Values.Count() == 2 && result.Products.Count() == 3);
        }

        [Test]
        public async Task TheProductDetialsReturnsDetailsAboutValidProduct()
        {
            var result = await productService.GetProductDetails(13);

            Assert.That(result.Id == 13);
            Assert.That(result.Name == "Product13");
            Assert.That(result.Manufacturer == null);
            Assert.That(result.Description == null);
            Assert.That(result.Warranty == 1);
            Assert.That(result.Attributes.Count() == 2);
            Assert.That(result.IsFavorite == false);
            Assert.That(result.Price == 130);
        }

        [Test]
        public async Task TheProductDetailsThrowsExceptionAboutInvalidProduct()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await productService.GetProductDetails(0);
            }, "The product does not exist.");
        }

        [Test]
        public async Task TheProductDetailsThrowsExceptionAboutInvalidProductWithProductIdMoreThanTheProductsCount()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var result = await productService.GetProductDetails(20);
            }, "The product does not exist.");
        }

        public async Task TheProducIsNotFromFavorite()
        {
            
        }
    }
}