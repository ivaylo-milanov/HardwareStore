namespace HardwareStore.Core.Services
{
    using HardwareStore.Core.Services.Contracts;
    using HardwareStore.Infrastructure.Common;
    using HardwareStore.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class FavoriteService : IFavoriteService
    {
        private readonly IRepository repository;

        public FavoriteService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<int>> AddToFavoriteAsync(List<int> favorites, int id)
        {
            var favorite = await this.repository.FindAsync<Product>(id);

            if (favorite == null)
            {
                throw new ArgumentNullException("The favorite does not exist");
            }

            if (!favorites.Contains(id))
            {
                favorites.Add(id);
            }

            return favorites;
        }

        public async Task<List<int>> RemoveFromFavoriteAsync(List<int> favorites, int id)
        {
            var favorite = await this.repository.FindAsync<Product>(id);

            if (favorite == null)
            {
                throw new ArgumentNullException("The favorite does not exist");
            }

            if (favorites.Contains(id))
            {
                favorites.Remove(id);
            }

            return favorites;
        }
    }
}
