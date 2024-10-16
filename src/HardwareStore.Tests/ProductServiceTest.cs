namespace HardwareStore.Tests
{
    using HardwareStore.Core.Infrastructure.Attributes;
    using HardwareStore.Core.Infrastructure.Enum;
    using HardwareStore.Core.Services;
    using HardwareStore.Core.ViewModels.Product;
    using HardwareStore.Tests.Mocking;
    using HardwareStore.Tests.Mocking.MoqViewModels.MoqProduct;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    [TestFixture]
    public class ProductServiceTest
    {
        private ProductService productService;

        [SetUp]
        public async Task SetUp()
        {
            var repository = await TestRepository.GetRepository();

            var cacheOptions = new MemoryCacheOptions();
            var cache = new MemoryCache(cacheOptions);

            productService = new ProductService(repository, cache);
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
            Assert.That(resultWithAttribute.Products.Count(), Is.EqualTo(2));
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
            Assert.That(product.MockId, Is.Not.EqualTo(product.Id));
        }

        [Test]
        public async Task Get0ProductsIfTheKeywordIsNullInGetProductsByKeyword()
        {
            //Act
            var result = await productService.GetSearchModel(null);

            //Assert
            Assert.That(result.Products.Count(), Is.EqualTo(15));
        }

        [Test]
        public async Task Get0ProductsIfTheKeywordIsEmptySpaceInGetProductsByKeyword()
        {
            //Act
            var result = await productService.GetSearchModel("");

            //Assert
            Assert.That(result.Products.Count(), Is.EqualTo(15));
        }

        [Test]
        public async Task Get0ProductsIfTheKeywordIsWhiteSpaceInGetProductsByKeyword()
        {
            //Act
            var result = await productService.GetSearchModel(" ");

            //Assert
            Assert.That(result.Products.Count(), Is.EqualTo(15));
        }

        [Test]
        public async Task Get0ProductsIfTheKeywordIsContinuousWhiteSpaceInGetProductsByKeyword()
        {
            //Act
            var result = await productService.GetSearchModel("   ");

            //Assert
            Assert.That(result.Products.Count(), Is.EqualTo(15));
        }

        [Test]
        public async Task GetEmptyFilterCategoriesWhenTheProductsCollectionIsEmpty()
        {
            //Act
            var result = await productService.GetModel<MoqProductModelWithInvalidCategory>();

            //Assert
            Assert.That(result.Filters.Count(), Is.EqualTo(1));
            Assert.That(result.Filters.First().Values.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetPropertiesWithCharacteristicAttributeWithDifferentValues()
        {
            //Act
            var result = await productService.GetModel<MoqProductModelWithProperCategory>();

            //Assert
            Assert.That(result.Filters.Count(), Is.EqualTo(4));
            foreach (var filter in result.Filters)
            {
                Assert.That(filter.Values.Count(), Is.EqualTo(2));
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
        public async Task FilterProductsWhenCacheContainsProductsReturnsCorrectProducts()
        {
            // Arrange
            var products = await this.productService.GetModel<MoqProductModelWIthCategory4>();

            var filter = new ProductFilterOptions { Order = (int)ProductOrdering.LowestPrice };

            // Act
            var result = productService.FilterProducts<MoqProductModelWIthCategory4, ProductFilterOptions>(filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count() == 3);
            Assert.That(result.First().Price == 30);
        }

        [Test]
        public async Task FilterProductsReturnsEmptyCollection()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWithInvalidCategory>();

            var filter = new ProductFilterOptions { Order = (int)ProductOrdering.LowestPrice };

            //Act
            var result = productService.FilterProducts<MoqProductModelWithInvalidCategory, ProductFilterOptions>(filter);

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count() == 0);
        }

        [Test]
        public async Task FilterProductsSuccessfullyByManufacturer()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWIthCategory4>();

            var filter = new ProductFilterOptions
            {
                Manufacturer = new List<string>() { "Manufacturer1" },
                Order = (int)ProductOrdering.Default
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWIthCategory4, ProductFilterOptions>(filter);

            //Assert
            Assert.IsTrue(result.Any());

            foreach (var item in result)
            {
                Assert.IsTrue(item.Manufacturer == "Manufacturer1");
            }
        }

        [Test]
        public async Task FilterProductsReturnsOneProductByCharacteristicName1AndCharacteristicName2()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWithProperCategory>();

            var filter = new MoqProductFilterOptionsCategory1
            {
                CharacteristicName1 = new List<string>() { "Value13" },
                CharacteristicName2 = new List<string>() { "Value14" },
                Order = (int)ProductOrdering.Default
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWithProperCategory, MoqProductFilterOptionsCategory1>(filter);

            //Assert
            Assert.That(result.Count() == 1);
        }

        [Test]
        public async Task FilterProductsRDontReturnAllProductsIfOneOfTheProductHaveNullValueInThisFilterCategory()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWIthCategory4>();

            var filter = new ProductFilterOptions
            {
                Manufacturer = new List<string>() { "Manufacturer1", "Manufacturer3" },
                Order = (int)ProductOrdering.Default
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWIthCategory4, ProductFilterOptions>(filter);

            //Assert
            Assert.That(result.Count() == 2);
        }

        [Test]
        public async Task FilterProductsReturnsAllProductsIfTheFilterHasAllValuesOfFilterCategory()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWithProperCategory>();

            var filter = new MoqProductFilterOptionsCategory1
            {
                CharacteristicName1 = new List<string>() { "Value13", "Value28" },
                Order = (int)ProductOrdering.Default
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWithProperCategory, MoqProductFilterOptionsCategory1>(filter);

            //Assert
            Assert.That(result.Count() == products.Products.Count());
        }

        [Test]
        public async Task FilterProductsReturnsOneProductIfTheProductCharacteristicValueHasMoreThenOneValues()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWithMoreThenOneValueInProperty>();

            var filter = new MoqProductFilterOptionsCategory5
            {
                CharacteristicName4 = new List<string>() { "Value40", "Value41" },
                Order = (int)ProductOrdering.Default
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWithMoreThenOneValueInProperty, MoqProductFilterOptionsCategory5>(filter);

            //Assert
            Assert.That(result.Count() == 1);
        }

        [Test]
        public async Task FilterProductsReturnsNoProductsIfItDoesNotMatchAnyOfTheValues()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWithMoreThenOneValueInProperty>();

            var filter = new MoqProductFilterOptionsCategory5
            {
                CharacteristicName4 = new List<string>() { "Value100" },
                Order = (int)ProductOrdering.Default
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWithMoreThenOneValueInProperty, MoqProductFilterOptionsCategory5>(filter);

            //Assert
            Assert.That(result.Count() == 0);
        }

        [Test]
        public async Task FilterProductsReturnsOrderedProductsByPriceAscending()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWithProperCategory>();

            var filter = new MoqProductFilterOptionsCategory1
            {
                Order = (int)ProductOrdering.LowestPrice
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWithProperCategory, MoqProductFilterOptionsCategory1>(filter).ToList();

            //Assert
            Assert.That(result[0].Price, Is.EqualTo(50));
            Assert.That(result[1].Price, Is.EqualTo(100));
        }

        [Test]
        public async Task FilterProductsReturnsOrderedProductsByPriceDescending()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWithProperCategory>();

            var filter = new MoqProductFilterOptionsCategory1
            {
                Order = (int)ProductOrdering.HighestPrice
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWithProperCategory, MoqProductFilterOptionsCategory1>(filter).ToList();

            //Assert
            Assert.That(result[0].Price, Is.EqualTo(100));
            Assert.That(result[1].Price, Is.EqualTo(50));
        }

        [Test]
        public async Task FilterProductsReturnsProductsAsTheyAreRetrievedByTheDatabase()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWithProperCategory>();

            var filter = new MoqProductFilterOptionsCategory1
            {
                Order = (int)ProductOrdering.Default
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWithProperCategory, MoqProductFilterOptionsCategory1>(filter).ToList();

            //Assert
            Assert.That(result, Is.EqualTo(products.Products));
        }

        [Test]
        public async Task FilterProductsReturnsOrderedProductsByAddDateAscending()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWithMoreThenOneValueInProperty>();

            var filter = new MoqProductFilterOptionsCategory5
            {
                Order = (int)ProductOrdering.Oldest
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWithMoreThenOneValueInProperty, MoqProductFilterOptionsCategory5>(filter).ToList();

            //Assert
            Assert.That(result[0].AddDate < result[1].AddDate);
        }

        [Test]
        public async Task FilterProductsReturnsOrderedProductsByAddDateDescending()
        {
            //Arrange
            var products = await this.productService.GetModel<MoqProductModelWithMoreThenOneValueInProperty>();

            var filter = new MoqProductFilterOptionsCategory5
            {
                Order = (int)ProductOrdering.Newest
            };

            //Act
            var result = productService.FilterProducts<MoqProductModelWithMoreThenOneValueInProperty, MoqProductFilterOptionsCategory5>(filter).ToList();

            //Assert
            Assert.That(result[1].AddDate > result[2].AddDate);
        }
    }
}