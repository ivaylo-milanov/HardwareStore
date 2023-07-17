namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Core.ViewModels.ShoppingCard;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ShoppingCardService : IShoppingCardService
    {
        private readonly IRepository repository;

        public ShoppingCardService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<ShoppingCardModel>> AddToShoppingCardAsync(List<ShoppingCardModel> shoppings, int id, int quantity)
        {
            var product = await this.repository.FindAsync<Product>(id);

            if (product == null)
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var shoppingModel = shoppings.FirstOrDefault(p => p.ProductId == id);

            if (shoppingModel == null)
            {
                var model = new ShoppingCardModel
                {
                    ProductId = id,
                    Quantity = quantity,
                };

                shoppings.Add(model);
            }
            else
            {
                shoppingModel.Quantity += quantity;
            }

            return shoppings;
        }

        public async Task<List<ShoppingCardModel>> RemoveFromShoppingCardAsync(List<ShoppingCardModel> shoppings, int id)
        {
            var product = await this.repository.FindAsync<Product>(id);

            if (product == null)
            {
                throw new ArgumentNullException("The product does not exist");
            }

            var shoppingModel = shoppings.FirstOrDefault(p => p.ProductId == id);

            if (shoppingModel != null)
            {
                shoppings.Remove(shoppingModel);
            }

            return shoppings;
        }
    }
}
