namespace HardwareStore.Tests.Mocking
{
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Data;
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Infrastructure.Models.Enums;
    using Microsoft.EntityFrameworkCore;

    public static class TestRepository
    {
        public static async Task<IRepository> GetRepository()
        {
            var options = new DbContextOptionsBuilder<HardwareStoreDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new HardwareStoreDbContext(options);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var repository = new Repository(context);

            await SeedDatabase(context);

            return repository;
        }

        private static async Task SeedDatabase(HardwareStoreDbContext context)
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
                            ProductId = i,
                            CharacteristicNameId = characteristicNames[(j - 1) % characteristicNames.Count].Id,
                            Value = $"Value{(i - 1) * 3 + j}"
                        }).ToList()
                }).ToList();

            products.Add(new Product
            {
                Id = 13,
                Name = "Product13",
                Price = 130,
                Quantity = 13,
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
                        CharacteristicNameId = 2,
                        Value = "Value37"
                    },
                    new Characteristic
                    {
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
                Quantity = 14,
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
                Quantity = 15,
                AddDate = new DateTime(2022, 11, 22),
                Warranty = 1,
                ManufacturerId = 2,
                ReferenceNumber = "Ref0015",
                CategoryId = 5,
                Model = "Model15",
                Characteristics = new[]
                {
                    new Characteristic
                    {
                        CharacteristicNameId = 2,
                        Value = "Value39"
                    },
                    new Characteristic
                    {
                        CharacteristicNameId = 4,
                        Value = "Value40, Value41"
                    }
                }
            });

            var users = new List<Customer>()
            {
                new Customer
                {
                    Id = "TestCustomer1",
                    UserName = "Customer1",
                    Email = "customer1@mail.com",
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    PhoneNumber = "Phone1",
                    Area = "Area1",
                    City = "City1",
                    Address = "Address1",
                    Favorites = new List<Favorite>()
                    {
                        new Favorite
                        {
                            ProductId = 13
                        },
                        new Favorite
                        {
                            ProductId = 14
                        }
                    },
                    ShoppingCartItems = new List<ShoppingCartItem>()
                    {
                        new ShoppingCartItem
                        {
                            ProductId = 13,
                            Quantity = 2
                        },
                        new ShoppingCartItem
                        {
                            ProductId = 14,
                            Quantity = 3
                        }
                    },
                    Orders = new List<Order>() {
                        new Order
                        {
                            OrderDate = DateTime.Now,
                            TotalAmount = 400,
                            OrderStatus = OrderStatus.Pending,
                            PaymentMethod = PaymentMethod.Cash,
                            AdditionalNotes = null,
                            FirstName = "FirstName1",
                            LastName = "LastName1",
                            Phone = "Phone1",
                            Area = "Area1",
                            City = "City1",
                            Address = "Address1",
                            ProductsOrders = new List<ProductOrder>()
                            {
                                new ProductOrder
                                {
                                    ProductId = 13,
                                    Quantity = 2
                                },
                                new ProductOrder
                                {
                                    ProductId = 14,
                                    Quantity = 1
                                }
                            }
                        }
                    }
                },
                new Customer
                {
                    Id = "TestCustomer2",
                    UserName = "Customer2",
                    Email = "customer2@mail.com",
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    PhoneNumber = "Phone2",
                    Area = "Area2",
                    City = "City2",
                    Address = "Address2"
                }
            };

            context.Manufacturers.AddRange(manufacturers);
            context.CharacteristicsNames.AddRange(characteristicNames);
            context.Categories.AddRange(categories);
            context.Products.AddRange(products);
            context.Users.AddRange(users);

            await context.SaveChangesAsync();
        }
    }
}