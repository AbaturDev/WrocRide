# WrocRide
WrocRide is a ride-handling web application designed to facilitate communication in Wrocław. The project was made for database course on University.

<p align="center">
  <img src="Logo.png" alt="WrocRide Logo">
</p>

## Features
### For everyone
* Register as a driver or a client
* Personalize profile
* View drivers ratings

### For Clients
* Order a ride
* Ride reservations
* Create schedules for regular rides
* Report system
* Option to rate driver
* Ride history

### For Drivers
* View and accept/reject ride requests
* Set pricing
* Report system

### For Admins
* User data management
* Driver verification
* Handle reports

## Technologies
### Backend:
- C#/.NET
- ASP.NET Core Web API
- Entity Framework Core

### Frontend
- C#/.NET
- Blazor WASM

### Database
- MS SQL Server

### Libraries & Tools
- FluentValidation
- Serilog
- Background service
- Swagger(Swashbuckle)
- Bogus
- JwtBearer

## Installation & Setup
1. **.NET SDK**
* Download and install .NET 8 SDK  from [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) 
* Ensure you installed it by typing `dotnet` in terminal
2. **Clone the repository**
```
git clone https://github.com/AbaturDev/WrocRide.git
```
3. **Configure the database**
* Ensure the MS SQL Server is installed and running
* Modify the connection string in `appsettings.json` if necessary.
4. **Apply migrations**
```
dotnet ef database update
```
5. **Run backend**
```
cd WrocRide.API
dotnet run
```
6. **Run frontend**
```
cd --
cd WrocRide.Client
dotnet run
```

## API Documentation
The API is documented using Swagger.
Once the backend is running, visit:
```{bash}
http://localhost:5000/swagger
```

## Authors
* **AbaturDev** - Project Manager, Backend developer, Frontend developer
* **Havkarr** - Backend developer
