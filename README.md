# Electronic Store API

- Programing language: C#
- Framework: Entity Framework
- Database: SQL Server
- Repository-Controller pattern

Backend part of the electronic store application. The following core dependencies were used:
- Microsoft Authentication JWT Bearer
- Microsoft Identity Model Tokens JWT
- Microsoft Entity Framework SQL Server
- Mailkit

#Authentication Strategy

- JWT and Refresh Token are generated in the backend
- JWT contains only the necessary information for authentication and authorization
- JWT is validated by Microsoft JWT dependencies
- Refresh token is saved in Users table for validation
- Refresh token is set to front-end as HTTP-only

#Email

Mailkit dependecy allows:
- User's email verification after registration
- Password recovery (email template needs to be created)

#Cart

- User can only add items to cart when registered
- Cart is automatically created for new users
- Payment preocess to empty cart must be implemented

#User

Users can perform the following actions:
- Add/update/delete product reviews
- Add/update/delete products from cart
- See products
- Filter prodcuts by category and brand
- Search products by name

#Admin

The following operations can only be performed by users with role "admin":
- Add/update/delete products
- Add/update/delete categories
- Add/update/delete brands
- Update/delete users
