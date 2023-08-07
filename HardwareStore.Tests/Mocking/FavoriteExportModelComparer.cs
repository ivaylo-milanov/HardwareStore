namespace HardwareStore.Tests.Mocking
{
    using HardwareStore.Core.ViewModels.Favorite;

    public class FavoriteExportModelComparer : IEqualityComparer<FavoriteExportModel>
    {
        public bool Equals(FavoriteExportModel x, FavoriteExportModel y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id
                && x.Name == y.Name
                && x.Price == y.Price;
        }

        public int GetHashCode(FavoriteExportModel obj)
        {
            return obj.Id.GetHashCode()
                ^ obj.Name.GetHashCode()
                ^ obj.Price.GetHashCode();
        }
    }
}
