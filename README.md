# Coffee Shops API

A .NET Core API for accessing coffee shop data across different cities and states in the United States.

## Features

- 🗄️ MySQL database integration with Entity Framework Core
- 🚀 24-hour caching for improved performance
- 📄 Pagination, sorting, and filtering support
- 📚 Swagger/OpenAPI documentation
- 🔄 CORS enabled for cross-origin requests

## Prerequisites

- .NET 10.0 SDK
- MySQL Server
- Database: `CoffeeMap`
- Table: `CoffeeShops`

## Configuration

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "CoffeeMapConnection": "Server=localhost;Port=3306;Database=CoffeeMap;User=your_username;Password=your_password;"
  }
}
```

## Running the Application

```bash
dotnet restore
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:5001/swagger`

## API Endpoints

### 1. Get Coffee Shops by City
```
GET /api/coffeeshops/city/{city}
```

**Query Parameters:**
- `page` (default: 1) - Page number
- `pageSize` (default: 50) - Items per page
- `sortBy` - Sort field (name, rating, city, state, reviews)
- `sortDescending` (default: false) - Sort direction

**Example:**
```
GET /api/coffeeshops/city/New York?page=1&pageSize=20&sortBy=rating&sortDescending=true
```

### 2. Get Coffee Shops by State
```
GET /api/coffeeshops/state/{state}
```

**Query Parameters:**
- Same as city endpoint

**Example:**
```
GET /api/coffeeshops/state/California?page=1&pageSize=50
```

### 3. Get All Coffee Shops (with filters)
```
GET /api/coffeeshops
```

**Query Parameters:**
- `city` - Filter by city
- `state` - Filter by state
- `minRating` - Minimum rating filter
- `page` (default: 1)
- `pageSize` (default: 50)
- `sortBy` - Sort field
- `sortDescending` (default: false)

**Example:**
```
GET /api/coffeeshops?city=Seattle&minRating=4.0&sortBy=rating&sortDescending=true
```

### 4. Get Coffee Shop by ID
```
GET /api/coffeeshops/{placeId}
```

**Example:**
```
GET /api/coffeeshops/ChIJ__-rlZFZwokRy7x-tA4XgzY
```

## Caching

All endpoints implement 24-hour caching to improve performance:
- Each unique query combination is cached separately
- Cache keys include all query parameters
- Results are cached for exactly 24 hours
- Reduces database load for frequently accessed queries

## Project Structure

```
CoffeeShops.API/
├── Controllers/
│   └── CoffeeShopsController.cs    # API endpoints
├── Data/
│   └── CoffeeShopsDbContext.cs     # Entity Framework DbContext
├── Models/
│   └── CoffeeShop.cs               # Entity model
├── Services/
│   ├── ICacheService.cs            # Cache interface
│   └── CacheService.cs             # Cache implementation
├── Program.cs                       # Application startup
└── appsettings.json                # Configuration
```

## Data Model

The `CoffeeShop` entity includes 60+ properties:
- Basic info (name, category, type)
- Location (address, city, state, coordinates)
- Contact (phone, website)
- Ratings & reviews
- Photos & media
- Business hours
- Owner information
- And much more...

## Technologies Used

- .NET 10.0
- Entity Framework Core 9.0
- Pomelo.EntityFrameworkCore.MySql 9.0
- ASP.NET Core Memory Cache
- Swagger/Swashbuckle

## Next Steps

1. Update the connection string in `appsettings.json`
2. Ensure your MySQL database is running
3. Run the application with `dotnet run`
4. Test endpoints using Swagger UI at `/swagger`
5. Monitor logs for cache hits and database queries

## Notes

- The API uses the existing database (no migrations needed)
- DbContext is used directly without an additional repository layer
- All endpoints return JSON responses
- CORS is configured to allow all origins (adjust for production)
