# Sumter Martial Arts Management System

A full-stack web application for managing martial arts programs, instructors, student progression, and private lesson bookings, built with .NET 8 and Angular, deployed on Microsoft Azure. **Features Event Sourcing for belt progression tracking and CQRS architecture.**

🔗 **Live Demo:** [https://jolly-smoke-0f6352e10.4.azurestaticapps.net](https://jolly-smoke-0f6352e10.4.azurestaticapps.net)  

---

## Table of Contents

- [Overview](#overview)
- [Event Sourcing Showcase](#event-sourcing-showcase)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Key Features](#key-features)
- [Domain Modeling](#domain-modeling)
- [Azure Infrastructure](#azure-infrastructure)
- [CI/CD Pipeline](#cicd-pipeline)
- [Local Development](#local-development)
- [What I Learned](#what-i-learned)

---

## Overview

This project demonstrates **enterprise-level software engineering practices** with a focus on **Domain-Driven Design**, **Event Sourcing**, and **CQRS**. The application manages martial arts programs, instructor schedules, student belt progression tracking, and a complete private lesson booking workflow.

### Why This Project Stands Out

- **🎯 Event Sourcing Architecture** - Complete audit trail of student belt progressions with time-travel queries
- **📊 Analytics from Events** - Real-time analytics dashboard powered by event store data
- **🏗️ Domain-Driven Design** - Rich domain models with proper encapsulation and business rule enforcement
- **⚡ CQRS with MediatR** - Clean separation of commands and queries
- **☁️ Cloud-Native** - Production deployment on Azure with CI/CD pipelines
- **🔒 Production-Ready** - Automated migrations, health checks, monitoring, and secure credential management

---

## Event Sourcing Showcase

### What Makes This Special

The student belt progression system uses **Event Sourcing** to maintain a complete, immutable history of every student's martial arts journey. This demonstrates advanced architectural patterns rarely seen in portfolio projects.

### Event Sourcing Features

**📅 Complete Event History**
```
Student: Sarah Johnson → BJJ Program
├─ 2023-06-27: Enrolled (White Belt)
├─ 2023-09-27: Test Attempted (Stripe 1) → Pass
├─ 2023-09-27: Promoted (White Belt → Stripe 1)
├─ 2023-12-27: Test Attempted (Stripe 2) → Pass
├─ 2023-12-27: Promoted (Stripe 1 → Stripe 2)
└─ 2024-06-27: Test Attempted (Blue Belt) → Pass
    └─ 2024-06-27: Promoted (Stripe 2 → Blue Belt)
```

**⏰ Time-Travel Queries**
```csharp
// "What rank was Sarah on June 1, 2024?"
GET /api/students/1/programs/1/rank-at-date?asOfDate=2024-06-01

Response:
{
  "rank": "White Belt - Stripe 2",
  "enrolledDate": "2023-06-27",
  "lastTestDate": "2023-12-27",
  "totalEventsProcessed": 6
}
```

**📊 Analytics Dashboard**

Powered entirely by replaying events from the event store:
- Pass/fail rates computed from test events
- Average time to blue belt (calculated by finding enrollment → promotion events)
- Most active testing months
- Current rank distribution
- Total promotions across all programs

### Event Store Architecture

**Event Types:**
- `EnrollmentEventData` - When a student joins a program
- `TestAttemptEventData` - Every test (pass AND fail) 
- `PromotionEventData` - Rank advancements

**Storage:**
```sql
StudentProgressionEventRecords
├─ EventId (GUID)
├─ StudentId (FK)
├─ ProgramId (FK)
├─ EventType (EnrollmentEventData | TestAttemptEventData | PromotionEventData)
├─ EventData (JSON - full event details)
├─ OccurredAt (Timestamp)
└─ Version (Optimistic concurrency)
```

**Why This Matters:**
- ✅ **Complete Audit Trail** - Never lose history, even if entities are deleted
- ✅ **Temporal Queries** - Reconstruct state at any point in time
- ✅ **Debugging** - Replay events to understand how current state was reached
- ✅ **Analytics** - Derive insights from immutable event history
- ✅ **Compliance** - Meets requirements for record keeping in professional settings

---

## Architecture

### Backend Architecture

**Vertical Slice Architecture + CQRS + Event Sourcing**

Each feature is organized as a self-contained vertical slice with:
- **Commands** - Write operations (MediatR handlers)
- **Queries** - Read operations (optimized for presentation)
- **Domain Events** - Cross-cutting concerns and audit trail
- **Event Store** - Immutable event history
- **Read Models** - Projections from events

### Domain-Driven Design Implementation

**Student Management Aggregate:**
```csharp
public class Student : BaseEntity
{
    // Encapsulated state - private setters
    public string Name { get; private set; }
    public string Email { get; private set; }
    
    // Encapsulated collections
    private readonly List<StudentProgramEnrollment> _programEnrollments = new();
    public IReadOnlyCollection<StudentProgramEnrollment> ProgramEnrollments 
        => _programEnrollments.AsReadOnly();
    
    // Business methods enforce invariants
    public void RecordTestResult(int programId, string rank, bool passed, string notes)
    {
        var enrollment = _programEnrollments
            .FirstOrDefault(e => e.ProgramId == programId && e.IsActive);
        
        if (enrollment == null)
            throw new InvalidOperationException("Must be enrolled to test");
        
        // Record test event
        var testResult = TestResult.Create(programId, rank, passed, notes);
        _testHistory.Add(testResult);
        
        // Raise domain event → saved to event store
        AddDomainEvent(new StudentTestRecorded { ... });
        
        // Auto-promote on pass
        if (passed)
        {
            enrollment.PromoteToRank(rank);
            AddDomainEvent(new StudentPromoted { ... });
        }
    }
}
```

**Value Objects:**
- `StudentAttendance` - Owned entity with computed properties
- `LessonTime` - Immutable time slot with validation
- `RequestStatus` - Type-safe status with state transitions

**Event Sourcing Pattern:**
```csharp
// Event store persists domain events
public class StudentProgressionEventRecord
{
    public Guid EventId { get; set; }
    public int StudentId { get; set; }
    public string EventType { get; set; }
    public string EventData { get; set; } // JSON
    public DateTime OccurredAt { get; set; }
    public int Version { get; set; }
}

// Time-travel by replaying events
public StudentRankAtDate GetRankAsOf(DateTime date)
{
    var events = _eventStore
        .GetEvents(studentId, programId)
        .Where(e => e.OccurredAt <= date)
        .OrderBy(e => e.Version);
    
    // Rebuild state by replaying
    foreach (var evt in events) { /* apply event */ }
}
```

### Frontend Architecture

**Angular 18** with:
- **Reactive Forms** - Complex validation
- **Material Design** - Professional UI components
- **Service Layer** - API abstraction
- **Event Sourcing Service** - Dedicated service for analytics and time-travel queries
- **Responsive Design** - Mobile-first approach

### System Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    Azure Static Web Apps                     │
│                  (Angular 18 Frontend - SPA)                 │
│              jolly-smoke-0f6352e10.4.azurestaticapps.net    │
│  ┌───────────────────────────────────────────────────────┐  │
│  │  • Student Management Dashboard                       │  │
│  │  • Belt Progression Analytics (Event Sourcing)        │  │
│  │  • Private Lesson Management                          │  │
│  │  • Time-Travel Queries UI                             │  │
│  └───────────────────────────────────────────────────────┘  │
└────────────────────────┬────────────────────────────────────┘
                         │ HTTPS
                         │ CORS configured
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                   Azure App Service (Linux)                  │
│                    .NET 8 Web API (F1 Free)                 │
│              sumter-martial-arts-api.azurewebsites.net      │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  CQRS Handlers (MediatR)                            │   │
│  │  ├─ Commands: RecordTest, EnrollStudent             │   │
│  │  ├─ Queries: GetStudents, GetAnalytics              │   │
│  │  └─ Event Handlers: StudentPromoted, TestRecorded   │   │
│  │                                                       │   │
│  │  Domain Events → Event Store                         │   │
│  └─────────────────────────────────────────────────────┘   │
└────────────────────────┬────────────────────────────────────┘
                         │ EF Core + Migrations
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                    Azure SQL Database                        │
│                    (Basic Tier - 2GB)                        │
│     sumter-martial-arts-sql2.database.windows.net          │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Read Models (CQRS):                                 │   │
│  │  ├─ Students, Programs, Instructors                  │   │
│  │  ├─ PrivateLessonRequests                           │   │
│  │  └─ StudentProgramEnrollments                        │   │
│  │                                                       │   │
│  │  Event Store (Event Sourcing):                       │   │
│  │  └─ StudentProgressionEventRecords                   │   │
│  │     • Complete immutable event history               │   │
│  │     • Time-travel capable                            │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│                      GitHub Actions                          │
│                 (CI/CD with EF Migrations)                   │
│  ┌──────────────────────┐  ┌──────────────────────┐        │
│  │  Backend Pipeline    │  │  Frontend Pipeline   │        │
│  │  • Build .NET        │  │  • Build Angular     │        │
│  │  • Run Migrations    │  │  • Deploy to Static  │        │
│  │  • Deploy to App Svc │  │    Web Apps          │        │
│  └──────────────────────┘  └──────────────────────┘        │
└─────────────────────────────────────────────────────────────┘
```

---

## Tech Stack

### Backend
- **.NET 8** - Latest LTS version
- **ASP.NET Core Web API** - RESTful API with Minimal APIs
- **Entity Framework Core 8** - ORM with migrations
- **MediatR** - CQRS implementation and domain events
- **Azure SQL Database** - Managed database service

### Frontend
- **Angular 18** - Modern SPA framework with signals
- **Angular Material** - UI component library
- **TypeScript** - Type-safe JavaScript
- **RxJS** - Reactive programming

### DevOps & Infrastructure
- **Azure App Service** - API hosting (Linux, F1 Free tier)
- **Azure Static Web Apps** - Frontend hosting (Free tier)
- **Azure SQL Database** - Database (Basic tier)
- **GitHub Actions** - CI/CD pipelines with automated migrations
- **Azure CLI** - Infrastructure management
- **Service Principal** - Secure Azure authentication

---

## Key Features

### 🎯 Event Sourcing & Analytics

- ✅ **Complete Event History** - Every test, promotion, enrollment stored as immutable events
- ✅ **Time-Travel Queries** - Reconstruct student rank at any point in history
- ✅ **Analytics Dashboard** - Real-time metrics from event store
  - Pass/fail rates
  - Average time to blue belt
  - Most active testing months
  - Rank distribution
- ✅ **Event Store** - Dedicated table with versioning and optimistic concurrency
- ✅ **Audit Trail** - Complete compliance-ready history

### 👨‍🎓 Student Management

- ✅ **Student Profiles** - Contact info, program enrollments, attendance tracking
- ✅ **Belt Progression Tracking** - Visual timeline of rank advancements
- ✅ **Test Result Recording** - Modal dialog with validation
  - Select program
  - Enter rank being tested
  - Mark pass/fail
  - Add instructor notes
  - Automatic promotion on pass
- ✅ **Test History** - Complete record of all test attempts (including failures)
- ✅ **Attendance Tracking** - Last 30 days, total classes, attendance rate
- ✅ **Program Notes** - Instructor feedback per program enrollment

### 🥋 Private Lesson Management

- ✅ **Request/Approval Workflow** - Complete booking system
- ✅ **Admin Dashboard** - Tabbed interface with status filtering
- ✅ **Intelligent Availability** - Checks business hours, class schedule conflicts, existing bookings
- ✅ **Domain Events** - Audit trail for approvals/rejections

### 🏗️ Architecture & Patterns

- ✅ **Domain-Driven Design** - Rich entities, value objects, aggregates
- ✅ **CQRS Pattern** - Commands and queries separated
- ✅ **Event Sourcing** - Immutable event stream for belt progression
- ✅ **Vertical Slice Architecture** - Features organized by business capability
- ✅ **Domain Events with MediatR** - Decoupled event handling
- ✅ **Proper Encapsulation** - Private backing fields, controlled mutation

---

## Domain Modeling

### Student Aggregate (Event Sourced)

```csharp
public class Student : BaseEntity
{
    // Factory method - only way to create valid student
    public static Student Create(string name, string email, string phone)
    {
        // Validation logic
        var student = new Student { Name = name, Email = email, Phone = phone };
        student.AddDomainEvent(new StudentCreated { ... });
        return student;
    }
    
    // Business methods enforce invariants
    public void EnrollInProgram(int programId, string programName, string initialRank)
    {
        if (_programEnrollments.Any(e => e.ProgramId == programId && e.IsActive))
            throw new InvalidOperationException("Already enrolled");
        
        var enrollment = StudentProgramEnrollment.Create(...);
        _programEnrollments.Add(enrollment);
        
        AddDomainEvent(new StudentEnrolledInProgram { ... });
    }
    
    public void RecordTestResult(int programId, string rank, bool passed, string notes)
    {
        // Validate enrollment
        // Record test
        // Raise events → persisted to event store
        // Auto-promote on pass
    }
}
```

### Value Objects

```csharp
// StudentAttendance - Value object (no identity)
public class StudentAttendance
{
    public int Last30Days { get; private set; }
    public int Total { get; private set; }
    public int AttendanceRate { get; private set; }
    
    public static StudentAttendance Create(int last30Days, int total)
    {
        // Validation and calculation
        return new StudentAttendance { /* ... */ };
    }
    
    // Immutable updates
    public StudentAttendance RecordAttendance(int additionalClasses)
    {
        return Create(
            Math.Min(Last30Days + additionalClasses, 30),
            Total + additionalClasses
        );
    }
}

// LessonTime - Immutable value object
public record LessonTime(DateTime Start, DateTime End)
{
    public TimeSpan Duration => End - Start;
    public bool Overlaps(LessonTime other) => ...;
}
```

### Event Store Schema

```sql
CREATE TABLE StudentProgressionEventRecords (
    EventId UNIQUEIDENTIFIER PRIMARY KEY,
    StudentId INT NOT NULL,
    ProgramId INT NOT NULL,
    EventType NVARCHAR(100) NOT NULL,
    EventData NVARCHAR(MAX) NOT NULL, -- JSON
    OccurredAt DATETIME2 NOT NULL,
    Version INT NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    
    INDEX IX_Student_Program (StudentId, ProgramId, Version),
    INDEX IX_OccurredAt (OccurredAt)
);
```

---

## Azure Infrastructure

### Resources Deployed

| Resource | Service | Tier | Purpose |
|----------|---------|------|---------|
| **App Service** | Azure App Service | F1 (Free) | Hosts .NET 8 Web API |
| **Static Web App** | Azure Static Web Apps | Free | Hosts Angular SPA |
| **SQL Database** | Azure SQL Database | Basic | Persistent data + event store |
| **Resource Group** | Azure Resource Manager | N/A | Logical container |

### Database Schema

**Read Models (Current State):**
- Students
- Programs
- Instructors
- StudentProgramEnrollments
- TestResults
- PrivateLessonRequests

**Event Store (History):**
- StudentProgressionEventRecords

### CI/CD with Automated Migrations

**Backend Pipeline** includes EF Core migrations:
```yaml
- name: Install and run EF migrations
  run: |
    dotnet new tool-manifest || true
    dotnet tool install dotnet-ef --version 8.0.0
    cd SumterMartialArtsAzure.Server.DataAccess
    dotnet ef database update --connection "${{ secrets.AZURE_SQL_CONNECTION_STRING }}"
```

Every deployment automatically applies pending migrations to Azure SQL!

---

## CI/CD Pipeline

### Automated Workflows

**Backend Pipeline** (`deploy.yml`)
```yaml
Trigger: Push to master branch
Steps:
  1. Checkout code
  2. Set up .NET 8
  3. Restore dependencies
  4. Build (Release)
  5. Install EF Core tools
  6. Run database migrations  ← Automatic schema updates!
  7. Login to Azure
  8. Deploy to App Service
```

**Frontend Pipeline** (`deploy-frontend.yml`)
```yaml
Trigger: Push to master branch
Steps:
  1. Checkout code
  2. Build Angular (production)
  3. Deploy to Static Web Apps
```

---

## Local Development

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
   
   # Run migrations
   dotnet ef database update
   
   # Run the API
   dotnet run
   ```
   
   API runs on: `https://localhost:5036`

3. **Frontend Setup**
   ```bash
   cd sumtermartialartsazure.client
   
   # Install dependencies
   npm install
   
   # Run the Angular app
   npm start
   ```
   
   App runs on: `https://localhost:4200`

### Database Seeding

The application automatically seeds:
- 6 Programs (BJJ, Judo, Kickboxing, etc.)
- 7 Instructors with schedules
- 7 Students with belt progression history
- 100+ event store records showing complete progression journeys

**Seed Data Includes:**
- Sarah Johnson: BJJ Blue Belt (2.5 year journey, 5 tests)
- David Kim: BJJ Purple Belt (4+ years, multi-program student)
- Michael O'Brien: Student with failed test then successful retry
- And more realistic scenarios!

---

## What I Learned

### Advanced Architectural Patterns

**Event Sourcing:**
- Designing event schemas for domain events
- Implementing time-travel queries by replaying events
- Building analytics from event streams
- Versioning and migration strategies for events
- Optimistic concurrency with event versions

**CQRS:**
- Separating read models from write models
- Command handlers that enforce business rules
- Query handlers optimized for specific UI needs
- Projections from event store to read models

**Domain-Driven Design:**
- Aggregate design with proper boundaries
- Encapsulation using private setters and backing fields
- Value objects for domain concepts
- Domain events for cross-cutting concerns
- Factory methods to prevent invalid state

### Technical Depth

**Backend:**
- MediatR for clean CQRS implementation
- EF Core migrations in CI/CD pipeline
- Minimal APIs with proper organization
- Domain event handling and dispatching
- JSON serialization for event data

**Frontend:**
- Angular Material dialogs for complex workflows
- Reactive forms with validation
- Service layer for event sourcing APIs
- Data visualization with CSS (charts without libraries)
- Responsive design patterns

**Cloud & DevOps:**
- Automated database migrations in deployment pipeline
- Managing event store alongside traditional tables
- GitHub Actions with EF Core tools
- Connection string management in Azure

### Problem-Solving

**Challenges Overcome:**
- ✅ Event schema design for belt progression domain
- ✅ Balancing event sourcing with CRUD operations
- ✅ Building analytics by aggregating events
- ✅ EF Core configuration for value objects and encapsulated collections
- ✅ Automated migrations in CI/CD pipeline
- ✅ Time-travel query implementation

---

## Links

- **Live Application:** [https://jolly-smoke-0f6352e10.4.azurestaticapps.net](https://jolly-smoke-0f6352e10.4.azurestaticapps.net)
- **API Health Check:** [https://sumter-martial-arts-api.azurewebsites.net/health](https://sumter-martial-arts-api.azurewebsites.net/health)
- **GitHub Repository:** [https://github.com/CheesePizza100/SumterMartialArtsAzure](https://github.com/CheesePizza100/SumterMartialArtsAzure)

---

## Author

**Your Name**
- GitHub: [@CheesePizza100](https://github.com/CheesePizza100)

---

## License

This project is for portfolio purposes.

---

**Built with ❤️ using .NET 8, Angular 18, Event Sourcing, CQRS, and Microsoft Azure**
