## Work Architecture Overview

This solution follows a **3-Layer Architecture**:

1.  **SampleAPI.API (Presentation)**: Handles HTTP requests, controllers, and DTO mappings
2.  **SampleAPI.BLL (Business Logic)**: Contains core business rules and service interfaces
3.  **SampleAPI.DAL (Data Access)**: Manages database connections, entities, and repositories

---
```
SampleAPI/
│
├── SampleAPI.sln
│
├── SampleAPI.API/                
│   │
│   ├── Controllers/
│   │
│   ├── DTOs/
│   │
│   ├── Mapping/
│   │   └── MappingProfile.cs
│   │
│   ├── Properties/
│   │   └── launchSettings.json
│   │
│   ├── appsettings.json
│   ├── Program.cs
│   └── SampleAPI.API.csproj
│
├── SampleAPI.BLL/                
│   │
│   ├── Interfaces/
│   │   ├── Repositories/
│   │   │
│   │   └── Services/
│   │
│   ├── Services/
│   │
│   └── SampleAPI.BLL.csproj
│
├── SampleAPI.DAL/                
│   │
│   ├── Data/
│   │   └── AppDbContext.cs
│   │
│   ├── Entities/
│   │
│   ├── Interfaces/
│   │
│   ├── Repositories/
│   │
│   ├── Migrations/
│   │   └── (EF Core migrations)
│   │
│   └── SampleAPI.DAL.csproj
│
└── .vs/
```