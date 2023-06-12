namespace HardwareStore.Common
{
    public class GlobalConstants
    {
        //Category
        public const int CategoryNameMinLength = 5;
        public const int CategoryNameMaxLength = 30;

        //Order
        public const int OrderBillingAddressMinLength = 10;
        public const int OrderBillingAddressMaxLength = 50;
        public const int OrderShippingAddressMinLength = 10;
        public const int OrderShippingAddressMaxLength = 50;

        //Product
        public const int ProductNameMinLength = 10;
        public const int ProductNameMaxLength = 60;
        public const int ProductDescriptionMinLength = 5;
        public const int ProductDescriptionMaxLength = 5000;
        public const int ProductModelMinLength = 3;
        public const int ProductModelMaxLength = 40;
        public const int ProductReferenceNumberMinLength = 8;
        public const int ProductReferenceNumberMaxLength = 50;

        //ProductAttribute
        public const int ProductAttributeNameMinLength = 1;
        public const int ProductAttributeNameMaxLength = 30;
        public const int ProductAttributeValueMinLength = 1;
        public const int ProductAttributeValueMaxLength = 30;

        //Customer
        public const int CustomerUserNameMinLength = 3;
        public const int CustomerUserNameMaxLength = 30;
        public const int CustomerEmailMinLength = 10;
        public const int CustomerEmailMaxLength = 100;
        public const int CustomerPasswordMinLength = 6;
        public const int CustomerPasswordMaxLength = 30;

        //Manufacturer
        public const int ManufacturerNameMinLength = 3;
        public const int ManufacturerNameMaxLength = 30;
    }
}
