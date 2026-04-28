# Greggs.Products

## My Implementation

### What I changed

**Story 1 - Wired up the data access layer**

Introduced a `ProductService` (behind `IProductService`) that takes `IDataAccess<Product>` via constructor injection. The controller delegates entirely to the service, no business logic in the controller. Both `ProductAccess` and `ProductService` are registered as scoped dependencies in `Startup.cs`.

**Story 2 - Euro pricing as a separate endpoint**

Rather than modifying the existing `/product` endpoint, I added a dedicated `GET /product/euros` endpoint that returns a `ProductEuroResponse` DTO with prices converted at the 1 GBP = 1.11 EUR rate. Keeping the endpoints separate preserves the existing contract and makes the currency concern explicit. The exchange rate is a named constant (`GbpToEurRate`) in the service, no magic numbers.

### Project structure

```
Greggs.Products.Api/
├── Controllers/
│   └── ProductController.cs       # Two endpoints: GET /product and GET /product/euros
├── DataAccess/
│   ├── IDataAccess.cs             # Unchanged
│   └── ProductAccess.cs           # Unchanged
├── Models/
│   ├── Product.cs                 # Unchanged
│   └── ProductEuroResponse.cs     # New DTO for euro responses
└── Services/
    ├── IProductService.cs         # New interface
    └── ProductService.cs          # New — handles retrieval and currency conversion

Greggs.Products.UnitTests/
└── ProductServiceTests.cs         # 8 unit tests using xUnit + Moq
```

### Running the API

```
dotnet run --project Greggs.Products.Api
```

Swagger UI available at: `https://localhost:{port}/swagger`

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/product?pageStart=0&pageSize=5` | Returns products with prices in GBP |
| GET | `/product/euros?pageStart=0&pageSize=5` | Returns products with prices in EUR |

### Running the tests

```
dotnet test
```

---

## Original Brief

### Introduction
Hello and welcome to the Greggs Products repository, thanks for finding it!

### The Solution
So at the moment the api is currently returning a random selection from a fixed set of Greggs products directly 
from the controller itself. We currently have a data access class and it's interface but 
it's not plugged in (please ignore the class itself, we're pretending it hits a database),
we're also going to pretend that the data access functionality is fully tested so we don't need 
to worry about testing those lines of functionality.

We're mainly looking for the way you work, your code structure and how you would approach tackling the following 
scenarios.

### User Stories
Our product owners have asked us to implement the following stories, we'd like you to have 
a go at implementing them. You can use whatever patterns you're used to using or even better 
whatever patterns you would like to use to achieve the goal. Anyhow, back to the 
user stories:

#### User Story 1
**As a** Greggs Fanatic<br/>
**I want to** be able to get the latest menu of products rather than the random static products it returns now<br/>
**So that** I get the most recently available products.

**Acceptance Criteria**<br/>
**Given** a previously implemented data access layer<br/>
**When** I hit a specified endpoint to get a list of products<br/>
**Then** a list or products is returned that uses the data access implementation rather than the static list it current utilises

#### User Story 2
**As a** Greggs Entrepreneur<br/>
**I want to** get the price of the products returned to me in Euros<br/>
**So that** I can set up a shop in Europe as part of our expansion

**Acceptance Criteria**<br/>
**Given** an exchange rate of 1GBP to 1.11EUR<br/>
**When** I hit a specified endpoint to get a list of products<br/>
**Then** I will get the products and their price(s) returned
