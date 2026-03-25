# README

## Overview

This is my project which is a e-commerce hardware platform.

## Features

### Product Catalogue
- **Product Types**: Our vast range of products include:
  - Mouses, Keyboards, Headsets, Monitors, Mouse Pads
  - Processors, Motherboards, RAM, Power Suppliers, Cases
  - Coolers, Video Cards, Internal Drives
  
- **Page Functionalities**:
  - **Filter Products**: Easily narrow down your choices.
  - **Sort Products**: Arrange products as per various criteria.
  - **State Maintenance**: User-friendly interface and state management
  
### Shopping Experience
- **Shopping Cart**:
  - Add products swiftly to your cart.
  - Update the quantity, or remove items as you wish.
  - The items are stored in the session if the customer is not logged in.
  
- **Favorites**:
  - Bookmark products you love.
  - Session storage for visitors, database storage for logged-in users.

### User Profile
- **Logged-In User Features**:
  - Access and modify your profile.
  - View order history.
  - Peruse your favorites list.

- **Ordering**:
  - On successful order placement, you'll be redirected to an order confirmation page.

### Search Functionality
- **Full-Text Search Bar**: Get precise results with indexed search on product Name, Description, Model, and Manufacturer Name.
- **Search Page**: Equipped with product page functionalities for an efficient search experience.

## Database Schema

1. **Product**:
   - Id, Name, Price, Quantity, Description
   - ManufacturerId, Model, Warranty
   - AddDate, CategoryId, ReferenceNumber
   - **Options** (JSON): key/value pairs for filters and product details (replaces former characteristic rows)

2. **Category**:
   - Id, Name
   
3. **Manufacturer**:
   - Id, Name
   
4. **Favorites Mapping Table**:
   - ProductId, CustomerId
   
5. **Shopping Cart Item Mapping Table**:
   - ProductId, CustomerId, Quantity
   
6. **ProductOrder Mapping Table**:
   - ProductId, OrderId, Quantity
   
7. **Order**:
   - Id (GUID type), OrderDate, TotalAmount
   - OrderStatus, PaymentMethod, AdditionalNotes
   - Personal and Address details: FirstName, LastName, Phone, City, Area, Address, CustomerId

8. **Customer (Inherited from IdentityUser)**:
   - FirstName, LastName, City, Area, Address
   
## Tech Stack

- **Database**: SQL Server
- **Authentication**: ASP.NET Core Identity
- **User Interface**: JavaScript, CSS, HTML
- **Backend**: C#

## Indexes for full-text search
```
CREATE FULLTEXT CATALOG product_catalog;

CREATE FULLTEXT INDEX ON Products(Name, Description, Model) 
KEY INDEX PK_Products
ON product_catalog;

CREATE FULLTEXT INDEX ON Manufacturers(Name) 
KEY INDEX PK_Manufacturers
ON product_catalog;
```

## Getting Started

1. **Setup**: Ensure SQL Server is up and running and connection strings are correctly configured.
2. **Migration**: On startup the web app applies pending EF Core migrations (`MigrateDatabaseAsync`). You can also run `dotnet ef database update` from the Infrastructure project if you prefer.
3. **Search-Functionality**: Create the necessary indexes to make the full-text search works.
4. **Run**: Launch the website and begin exploring the hardware collection.
