# Sumter Martial Arts Management System

A full-stack web application for managing martial arts programs, instructors, and private lesson bookings, built with .NET 8 and Angular, deployed on Microsoft Azure.

🔗 **Live Demo:** [https://jolly-smoke-0f6352e10.4.azurestaticapps.net](https://jolly-smoke-0f6352e10.4.azurestaticapps.net)  

---

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Key Features](#key-features)
- [Domain Modeling](#domain-modeling)
- [Azure Infrastructure](#azure-infrastructure)
- [CI/CD Pipeline](#cicd-pipeline)
- [Local Development](#local-development)
- [Project Structure](#project-structure)
- [What I Learned](#what-i-learned)

---

## 🎯 Overview

This project demonstrates enterprise-level software engineering practices in a real-world martial arts management system. The application handles program information, instructor profiles, and a complete private lesson request/approval workflow with intelligent availability checking.

### Why This Project Stands Out

- **Domain-Driven Design:** Rich domain models with proper encapsulation and business rule enforcement
- **Vertical Slices:** Vertical slice architecture with CQRS pattern
- **Cloud-Native:** Deployed on Azure with proper separation of concerns (Static Web App + App Service)
- **Production-Ready:** CI/CD pipelines, health checks, monitoring, and secure credential management

---

## 🏗️ Architecture

### Backend Architecture

**Vertical Slice Architecture + CQRS**

Each feature is organized as a self-contained vertical slice with its own:
- Command/Query handlers (MediatR)
- Domain logic
- Data access
- API endpoints

**Domain-Driven Design Principles:**
- ✅ Rich domain entities with encapsulation
- ✅ Value objects for domain concepts
- ✅ Domain events for cross-cutting concerns
- ✅ Aggregates with consistency boundaries
- ✅ Domain services for complex business logic

### Frontend Architecture

**Angular 18** with:
- Reactive Forms for complex validation
- Material Design components
- Service-based architecture
- Environment-based configuration
- Responsive design

### System Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    Azure Static Web Apps                     │
│                  (Angular Frontend - SPA)                    │
│              jolly-smoke-0f6352e10.4.azurestaticapps.net    │
└────────────────────────┬────────────────────────────────────┘
                         │ HTTPS
                         │ CORS configured
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                   Azure App Service (Linux)                  │
│                    .NET 8 Web API (F1 Free)                 │
│              sumter-martial-arts-api.azurewebsites.net      │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  • Health Checks                                     │   │
│  │  • Application Logging                               │   │
│  │  • HTTPS Only                                        │   │
│  │  • Managed Identity (future)                         │   │
│  └─────────────────────────────────────────────────────┘   │
└────────────────────────┬────────────────────────────────────┘
                         │ Connection String (secured)
                         │ EF Core
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                    Azure SQL Database                        │
│                    (Basic Tier - 2GB)                        │
│     sumter-martial-arts-sql2.database.windows.net          │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  • Instructors                                       │   │
│  │  • Programs                                          │   │
│  │  • PrivateLessonRequests                            │   │
│  │  • ClassSchedules                                    │   │
│  │  • Firewall Rules configured                         │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                      GitHub Actions                          │
│                    (CI/CD Pipelines)                         │
│  ┌──────────────────────┐  ┌──────────────────────┐        │
│  │  Backend Pipeline    │  │  Frontend Pipeline   │        │
│  │  • Build .NET        │  │  • Build Angular     │        │
│  │  • Run Tests         │  │  • Deploy to Static  │        │
│  │  • Deploy to App Svc │  │    Web Apps          │        │
│  └──────────────────────┘  └──────────────────────┘        │
└─────────────────────────────────────────────────────────────┘
```

---

## 🛠️ Tech Stack

### Backend
- **.NET 8** - Latest LTS version
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core 8** - ORM with migrations
- **MediatR** - CQRS implementation and domain events
- **Azure SQL Database** - Managed database service

### Frontend
- **Angular 18** - Modern SPA framework
- **Angular Material** - UI component library
- **TypeScript** - Type-safe JavaScript
- **RxJS** - Reactive programming

### DevOps & Infrastructure
- **Azure App Service** - API hosting (Linux, F1 Free tier)
- **Azure Static Web Apps** - Frontend hosting (Free tier)
- **Azure SQL Database** - Database (Basic tier)
- **GitHub Actions** - CI/CD pipelines
- **Azure CLI** - Infrastructure management
- **Service Principal** - Secure Azure authentication

### Development Tools
- **Visual Studio 2022** - IDE
- **SQL Server Management Studio** - Database management
- **Git/GitHub** - Version control
- **PowerShell** - Scripting and automation

---

## ✨ Key Features

### Architecture & Patterns

- ✅ **Vertical Slice Architecture** - Features organized by business capability
- ✅ **Domain-Driven Design** - Rich entities, value objects, aggregates
- ✅ **Domain Events with MediatR** - Decoupled event handling
- ✅ **CQRS Pattern** - Separation of reads and writes
- ✅ **Proper Encapsulation** - Private backing fields, controlled mutation
- ✅ **Domain Services** - Complex business logic in domain layer

### Domain Modeling

**Value Objects:**
- `LessonTime` - Represents a time slot with start/end and validation
- `RequestStatus` - Type-safe status with state transitions
- `BusinessHours` - Operating hours with slot generation logic
- `AvailabilityRule` - Weekly schedule patterns

**Entities:**
- `Instructor` - Rich entity with schedules and business rules
- `PrivateLessonRequest` - Request aggregate with approval workflow
- `Program` - Martial arts program information
- `ClassSchedule` - Recurring class schedules

**Business Rules:**
- State machine (Pending → Approved/Rejected) with validation
- Availability checking (business hours + class conflicts + existing bookings)
- Timezone handling (UTC storage, Eastern Time display)
- Duration and overlap validation

### Full-Stack Features

- ✅ **Private Lesson Request/Approval Workflow** - Complete booking system
- ✅ **Admin Dashboard** - Tabbed interface with status filtering
- ✅ **Intelligent Availability** - Checks business hours, class schedule conflicts, and existing bookings
- ✅ **Domain Events** - Audit trail and extensibility
- ✅ **Material Design UI** - Modern, responsive interface
- ✅ **Real-time Validation** - Client and server-side validation

### Enterprise Qualities

- ✅ **Testable Design** - Dependency injection, interfaces, separation of concerns
- ✅ **Security** - CORS configured, HTTPS enforced, secrets in Azure
- ✅ **Proper Error Handling** - Validation, null checks, meaningful messages
- ✅ **Clean Code** - SOLID principles, DRY, meaningful names
- ✅ **Health Checks** - `/health` endpoint for monitoring

---

## 🎨 Domain Modeling

### Value Objects

```csharp
// LessonTime - Immutable value object with validation
public record LessonTime(DateTime Start, DateTime End)
{
    public TimeSpan Duration => End - Start;
    
    public bool Overlaps(LessonTime other) =>
        Start < other.End && End > other.Start;
}

// RequestStatus - Type-safe enum with behavior
public enum RequestStatus
{
    Pending,
    Approved,
    Rejected
}
```

### Rich Entities

```csharp
public class Instructor
{
    private readonly List<ClassSchedule> _classSchedule = new();
    
    public IReadOnlyList<ClassSchedule> ClassSchedule => _classSchedule.AsReadOnly();
    
    // Business logic encapsulated in entity
    public IEnumerable<LessonTime> GenerateNextOccurrences(DateTime from, int days)
    {
        // Complex scheduling logic...
    }
}
```

### Domain Events

```csharp
public class PrivateLessonRequestApproved : INotification
{
    public int RequestId { get; init; }
    public DateTime ApprovedAt { get; init; }
    // Handled by audit/notification handlers
}
```

---

## ☁️ Azure Infrastructure

### Resources Deployed

| Resource | Service | Tier | Purpose |
|----------|---------|------|---------|
| **App Service** | Azure App Service | F1 (Free) | Hosts .NET 8 Web API |
| **Static Web App** | Azure Static Web Apps | Free | Hosts Angular SPA |
| **SQL Database** | Azure SQL Database | Basic | Persistent data storage |
| **Resource Group** | Azure Resource Manager | N/A | Logical container |

### Security & Configuration

- **HTTPS Enforced** - All traffic encrypted
- **CORS Configured** - Specific origin allowlist
- **Connection Strings** - Stored in Azure App Service configuration (not in code)
- **Firewall Rules** - Azure SQL restricted to Azure services + admin IP
- **Service Principal** - GitHub Actions authentication to Azure
- **Health Checks** - `/health` endpoint monitored by Azure

### Cost Breakdown

**Monthly Operating Cost: ~$5**

- App Service (F1): **FREE** (750 hours/month free tier)
- Static Web Apps: **FREE** (100 GB bandwidth included)
- Azure SQL (Basic): **~$5/month**

*Note: With Azure for Students, all services are FREE*

---

## 🚀 CI/CD Pipeline

### Automated Workflows

**Backend Pipeline** (`deploy.yml`)
```yaml
Trigger: Push to main branch
Steps:
  1. Checkout code
  2. Set up .NET 8
  3. Restore dependencies
  4. Build (Release configuration)
  5. Publish
  6. Login to Azure (Service Principal)
  7. Deploy to App Service via Azure CLI
```

**Frontend Pipeline** (`deploy-frontend.yml`)
```yaml
Trigger: Push to main branch (frontend changes)
Steps:
  1. Checkout code
  2. Build Angular app (production)
  3. Deploy to Static Web Apps
```

### Deployment Process

```
Developer → Git Push → GitHub Actions → Azure
                           ↓
                    ┌──────┴──────┐
                    │             │
              Build & Test    Build Angular
                    │             │
              Deploy API    Deploy Frontend
                    │             │
                    └──────┬──────┘
                           ↓
                    Live in Azure
```

### Key Features of CI/CD

- ✅ **Automatic Deployment** - Every push to `main` triggers deployment
- ✅ **Build Validation** - Won't deploy if build fails
- ✅ **Separate Pipelines** - Frontend and backend deploy independently
- ✅ **Secure Credentials** - Service principal and deployment tokens in GitHub Secrets
- ✅ **Azure CLI Integration** - Direct deployment via Azure tooling

---

## 💻 Local Development

### Prerequisites

- .NET 8 SDK
- Node.js 18+ and npm
- SQL Server (LocalDB or Express)
- Visual Studio 2022 or VS Code
- Git

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/CheesePizza100/SumterMartialArtsAzure.git
   cd SumterMartialArtsAzure
   ```

2. **Backend Setup**
   ```bash
   cd SumterMartialArtsAzure.Server
   
   # Update connection string in appsettings.json
   # Default: Server=(localdb)\\mssqllocaldb;Database=SumterMartialArts;Trusted_Connection=True;
   
   # Run migrations
   dotnet ef database update
   
   # Run the API
   dotnet run
   ```
   
   API runs on: `https://localhost:5036` (or your configured port)

3. **Frontend Setup**
   ```bash
   cd sumtermartialartsazure.client
   
   # Install dependencies
   npm install
   
   # Update environment file (if needed)
   # src/environments/environment.development.ts
   
   # Run the Angular app
   npm start
   ```
   
   App runs on: `https://localhost:4200`

### Database Seeding

The application automatically seeds sample data on startup:
- Programs (Brazilian Jiu-Jitsu, Muay Thai, MMA)
- Instructors with schedules

---

## 📁 Project Structure

```
SumterMartialArtsAzure/
├── .github/
│   └── workflows/
│       ├── deploy.yml              # Backend CI/CD
│       └── deploy-frontend.yml     # Frontend CI/CD
├── sumtermartialartsazure.client/  # Angular Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── admin/              # Admin dashboard
│   │   │   ├── programs/           # Program features
│   │   │   ├── instructors/        # Instructor features
│   │   │   └── services/           # API services
│   │   └── environments/           # Environment config
│   ├── angular.json
│   └── package.json
├── SumterMartialArtsAzure.Server/  # .NET Backend
│   ├── Features/                   # Vertical slices
│   │   ├── Programs/
│   │   ├── Instructors/
│   │   └── PrivateLessons/
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   └── DbSeeder.cs
│   ├── Endpoints/                  # Minimal API endpoints
│   └── Program.cs                  # App configuration
├── SumterMartialArtsAzure.Server.Domain/
│   ├── Entities/
│   │   ├── Instructor.cs
│   │   ├── Program.cs
│   │   └── PrivateLessonRequest.cs
│   └── ValueObjects/
│       ├── LessonTime.cs
│       ├── BusinessHours.cs
│       └── AvailabilityRule.cs
├── SumterMartialArtsAzure.Server.DataAccess/
│   └── Configurations/             # EF Core configurations
└── global.json
```

---

## 🎓 What I Learned

### Technical Skills

**Backend Development:**
- Implementing Domain-Driven Design in a real application
- CQRS pattern with MediatR for clean separation of concerns
- Complex business logic with value objects and domain services
- Entity Framework Core migrations and seeding strategies
- Minimal APIs and endpoint organization

**Frontend Development:**
- Angular 18 features and best practices
- Reactive Forms with complex validation
- Material Design component library
- Environment-based configuration management
- Handling timezone conversions between backend and frontend

**Cloud & DevOps:**
- Azure App Service configuration and deployment
- Azure Static Web Apps for SPAs
- Azure SQL Database management and firewall rules
- GitHub Actions for CI/CD pipelines
- Service principal authentication for secure deployments
- Managing secrets and connection strings in Azure

**Architecture:**
- Vertical slice architecture vs layered architecture
- When to use CQRS and when it's overkill
- Proper domain modeling with value objects and entities
- Encapsulation and information hiding in domain models
- Cross-cutting concerns with domain events

### Problem-Solving

**Challenges Overcome:**
- ✅ Azure quota limits (free tier restrictions) - solved with region switching
- ✅ Timezone handling between UTC storage and Eastern Time display
- ✅ CORS configuration for separate frontend/backend deployments
- ✅ GitHub Actions deployment authentication (publish profile → service principal)
- ✅ Entity Framework design-time DbContext configuration
- ✅ Complex availability checking with multiple conflict sources

### Professional Practices

- Git workflow with feature branches and pull requests
- Writing meaningful commit messages
- Comprehensive README documentation
- Separation of development and production configurations
- Security best practices (no secrets in code)
- Code organization for maintainability

---

## 🔗 Links

- **Live Application:** [https://jolly-smoke-0f6352e10.4.azurestaticapps.net](https://jolly-smoke-0f6352e10.4.azurestaticapps.net)
- **API Health Check:** [https://sumter-martial-arts-api.azurewebsites.net/health](https://sumter-martial-arts-api.azurewebsites.net/health)
- **GitHub Repository:** [https://github.com/CheesePizza100/SumterMartialArtsAzure](https://github.com/CheesePizza100/SumterMartialArtsAzure)

---

## 👤 Author

**Your Name**
- GitHub: [@CheesePizza100](https://github.com/CheesePizza100)

---

## 📝 License

This project is for portfolio purposes.

---

## 🙏 Acknowledgments

- Martial arts domain knowledge from real-world requirements
- Azure documentation and community resources
- Angular and .NET communities for excellent documentation

---

**Built with ❤️ using .NET 8, Angular 18, and Microsoft Azure**
