﻿namespace HardwareStore.Common
{
    public class GlobalConstants
    {
        //Category
        public const int CategoryNameMinLength = 5;
        public const int CategoryNameMaxLength = 30;

        //Order
        public const int OrderAdditionalNotesMaxLength = 2000;
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
        public const int ProductModelMaxLength = 150;
        public const int ProductReferenceNumberMinLength = 8;
        public const int ProductReferenceNumberMaxLength = 100;

        //Characteristic
        public const int CharacteristicValueMinLength = 1;
        public const int CharacteristicValueMaxLength = 120;

        //Customer
        public const int CustomerUserNameMinLength = 3;
        public const int CustomerUserNameMaxLength = 30;
        public const int CustomerEmailMinLength = 10;
        public const int CustomerEmailMaxLength = 100;
        public const int CustomerPasswordMinLength = 6;
        public const int CustomerPasswordMaxLength = 30;
        public const int CustomerFirstNameMaxLength = 60;
        public const int CustomerLastNameMaxLength = 60;
        public const int CustomerCityMaxLength = 20;
        public const int CustomerAreaMaxLength = 20;
        public const int CustomerAddressMaxLength = 50;

        //Manufacturer
        public const int ManufacturerNameMinLength = 3;
        public const int ManufacturerNameMaxLength = 30;

        //CharacteristicName
        public const int CharacteristicNameMinLength = 3;
        public const int CharacteristicNameMaxLength = 60;
    }
}
