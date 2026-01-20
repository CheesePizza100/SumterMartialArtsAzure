# Sumter Martial Arts Management System

A full-stack web application for managing martial arts programs, instructors, student progression, and private lesson bookings, built with .NET 8 and Angular, deployed on Microsoft Azure. **Features Event Sourcing for belt progression tracking, CQRS architecture, and advanced OOP design patterns.**

🔗 **Live Demo:** [https://jolly-smoke-0f6352e10.4.azurestaticapps.net](https://jolly-smoke-0f6352e10.4.azurestaticapps.net)  

---

## Table of Contents

- [Overview](#overview)
- [Advanced Design Patterns](#advanced-design-patterns)
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

This project demonstrates **enterprise-level software engineering practices** with a focus on **Domain-Driven Design**, **Event Sourcing**, **CQRS**, and **advanced OOP patterns**. The application manages martial arts programs, instructor schedules, student belt progression tracking with complete audit trails, and a sophisticated private lesson booking workflow.

### Why This Project Stands Out

- **🎯 Event Sourcing with Projectors** - Complete audit trail with Strategy + Template Method patterns
- **🎨 Visitor Pattern for Analytics** - Type-safe aggregation of heterogeneous results
- **🏗️ Domain-Driven Design** - Rich domain models with proper encapsulation
- **⚡ CQRS with MediatR** - Clean separation with no switch statements
- **🔧 Advanced Refactoring** - Eliminated all procedural anti-patterns (switch statements, private helper methods)
- **☁️ Cloud-Native** - Production deployment on Azure with automated migrations
- **📊 Polymorphic Queries** - Filter strategies for composable query behavior

---

## Advanced Design Patterns

### Event Projectors (Strategy + Template Method)

Student belt progression uses **event projectors** that eliminate switch statements through polymorphism:
```csharp
// Base projector with Template Method
public abstract class EventProjectorBase<TEventData> : IEventProjector
{
    public StudentProgressionState Project(StudentProgressionEventRecord evt, StudentProgressionState currentState)
    {
        var data = JsonSerializer.Deserialize<TEventData>(evt.EventData);
        return data == null 
            ? currentState 
            : ProjectEvent(data, evt, currentState);
    }

    protected abstract StudentProgressionState ProjectEvent(
        TEventData data, 
        StudentProgressionEventRecord evt, 
        StudentProgressionState currentState);
}

// Concrete projectors are simple and focused
public class PromotionEventProjector : EventProjectorBase<PromotionEventData>
{
    protected override StudentProgressionState ProjectEvent(...)
    {
        return currentState with
        {
            CurrentRank = data.ToRank,
            LastTestDate = evt.OccurredAt,
            LastTestNotes = data.Reason
        };
    }
}
```

**Benefits:**
- ✅ No switch statements (Open/Closed Principle)
- ✅ Each event type has its own class (Single Responsibility)
- ✅ DRY - Shared deserialization logic in base class
- ✅ Easy to test - Each projector independently testable
- ✅ Easy to extend - Add new event types without modifying existing code

### Visitor Pattern for Analytics (Visitor as Accumulator)

Analytics calculations use the **Visitor pattern** to accumulate heterogeneous results:
```csharp
// Create a type hierarchy for results
public interface IAnalyticsResult
{
    void Accept(IAnalyticsResultVisitor visitor);
}

public record TestStatisticsResult(int TotalTests, int PassedTests, ...) : IAnalyticsResult
{
    public void Accept(IAnalyticsResultVisitor visitor) => visitor.Visit(this);
}

// Visitor accumulates results into final response
public class AnalyticsResponseBuilder : IAnalyticsResultVisitor
{
    public void Visit(TestStatisticsResult result)
    {
        _totalTests = result.TotalTests;
        _passedTests = result.PassedTests;
        // ...
    }
    
    public void Visit(EnrollmentCountResult result) { /* ... */ }
    
    public GetProgressionAnalyticsResponse ProduceResult() { /* ... */ }
}

// Clean handler
var builder = new AnalyticsResponseBuilder();
foreach (var calculator in _calculators)
{
    var result = await calculator.Calculate(eventsQuery, request.ProgramId, cancellationToken);
    result.Accept(builder); // Double dispatch!
}
return builder.Build();
```

**Why This is Powerful:**
- ✅ Type-safe - No `object`, `dynamic`, or type checking
- ✅ Created a type hierarchy where none existed naturally
- ✅ Each calculator returns its own strongly-typed result
- ✅ Visitor accumulates results without knowing concrete types
- ✅ Easy to add new analytics without modifying existing code

### Query Filter Strategies

Query filtering uses **Strategy pattern** to eliminate switch statements:
```csharp
public interface IPrivateLessonFilter
{
    string FilterName { get; }
    IQueryable<PrivateLessonRequest> Apply(IQueryable<PrivateLessonRequest> query);
}

public class PendingLessonsFilter : IPrivateLessonFilter
{
    public string FilterName => "Pending";
    public IQueryable<PrivateLessonRequest> Apply(IQueryable<PrivateLessonRequest> query)
        => query.Where(r => r.Status == RequestStatus.Pending);
}

// Handler is clean and extensible
var filter = _filters.TryGetValue(request.Filter, out var found) 
    ? found 
    : _defaultFilter;
query = filter.Apply(query);
```

**Benefits:**
- ✅ Works with `IQueryable` - composable before database execution
- ✅ Each filter is independently testable
- ✅ No switch statement on filter type
- ✅ Dependency injection ready

### Domain Services for Clean Calculators

Complex analytics use **domain services** to encapsulate query logic:
```csharp
// Domain service handles deserialization and query complexity
public class StudentProgressionEventService
{
    public async Task<List<PromotionEvent>> GetPromotionEvents(int? programId, ...)
    {
        var events = await _dbContext.StudentProgressionEvents
            .AsNoTracking()
            .Where(e => e.EventType == nameof(PromotionEventData))
            .Where(e => !programId.HasValue || e.ProgramId == programId.Value)
            .ToListAsync(cancellationToken);

        return events
            .Select(e => new PromotionEvent(
                e.StudentId, e.ProgramId, e.OccurredAt,
                JsonSerializer.Deserialize<PromotionEventData>(e.EventData)!
            ))
            .Where(e => e.Data != null)
            .ToList();
    }
}

// Calculator becomes simple orchestration
public class AverageTimeToRankCalculator : IProgressionAnalyticsCalculator
{
    public async Task<IAnalyticsResult> Calculate(...)
    {
        var enrollments = await _eventService.GetEnrollmentEvents(programId, cancellationToken);
        var promotions = await _eventService.GetPromotionEvents(programId, cancellationToken);

        var averagesByRank = promotions
            .GroupBy(p => p.Data.ToRank)
            .Select(rankGroup => _rankCalculator.Calculate(rankGroup, enrollments))
            .ToList();

        return new AverageTimeToRankResult(averagesByRank);
    }
}
```

**What This Shows:**
- ✅ No private helper methods - each responsibility is a class
- ✅ Clear separation of concerns
- ✅ Every piece is independently testable
- ✅ Complex LINQ queries hidden behind clean interfaces

---

## Event Sourcing Showcase

### What Makes This Special

The student belt progression system uses **Event Sourcing** to maintain a complete, immutable history of every student's martial arts journey. Events are processed using the **Strategy pattern** and **Template Method pattern** for clean, extensible projections.

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

**⏰ Time-Travel Queries with Event Projectors**
```csharp
// "What rank was Sarah on June 1, 2024?"
GET /api/students/1/programs/1/rank-at-date?asOfDate=2024-06-01

// Backend uses polymorphic projectors (no switch statements!)
var state = new StudentProgressionState();
foreach (var evt in events)
{
    if (_eventProjectors.TryGetValue(evt.EventType, out var projector))
    {
        state = projector.Project(evt, state); // Strategy pattern!
    }
}

Response:
{
  "rank": "White Belt - Stripe 2",
  "enrolledDate": "2023-06-27",
  "lastTestDate": "2023-12-27",
  "totalEventsProcessed": 6
}
```

**📊 Analytics Dashboard (Visitor Pattern)**

Powered by **Visitor pattern** aggregating heterogeneous calculator results:
```csharp
// Each calculator returns its own strongly-typed result
var enrollmentCount = await enrollmentCalculator.Calculate(...); // → EnrollmentCountResult
var testStats = await testStatsCalculator.Calculate(...);       // → TestStatisticsResult
var rankProgress = await rankCalculator.Calculate(...);         // → AverageTimeToRankResult

// Visitor accumulates them into final response
var builder = new AnalyticsResponseBuilder();
foreach (var result in results)
{
    result.Accept(builder); // Double dispatch!
}
return builder.Build();
```

**Analytics Include:**
- Pass/fail rates (from test attempt events)
- Average time to each belt rank (enrollment → promotion events)
- Most active testing months (temporal aggregation)
- Current rank distribution (projection from events)
- Total promotions across all programs

### Pattern Composition in Action

**The beauty of this architecture:**

1. **Event Projectors** (Strategy + Template Method) - Process individual events
2. **Analytics Calculators** (Strategy) - Compute specific metrics
3. **Visitor Pattern** - Aggregate heterogeneous results
4. **Domain Services** - Encapsulate complex queries
5. **Dependency Injection** - Wire everything together

**No switch statements. No private helper methods. Pure OOP.**

---

## Architecture

### Backend Architecture

**Vertical Slice Architecture + CQRS + Event Sourcing + Advanced OOP Patterns**

Each feature is organized as a self-contained vertical slice with:
- **Commands** - Write operations (MediatR handlers)
- **Queries** - Read operations with polymorphic filtering
- **Domain Events** - Cross-cutting concerns and audit trail
- **Event Store** - Immutable event history
- **Event Projectors** - Strategy pattern for event processing
- **Read Models** - Projections from events
- **Visitors** - Type-safe aggregation of heterogeneous results

### Design Patterns Used

| Pattern | Purpose | Example |
|---------|---------|---------|
| **Strategy** | Polymorphic behavior selection | Event projectors, query filters, analytics calculators |
| **Template Method** | Shared algorithm with variable steps | `EventProjectorBase<T>` |
| **Visitor** | Operations on heterogeneous objects | `AnalyticsResponseBuilder` visiting calculator results |
| **Factory** | Controlled object creation | `Student.Create()`, `LessonTime.Create()` |
| **Repository** | Data access abstraction | EF Core DbContext |
| **CQRS** | Command/Query separation | MediatR handlers |
| **Event Sourcing** | Immutable event log | `StudentProgressionEventRecord` |
| **Domain Events** | Decoupled cross-cutting concerns | MediatR notifications |

### Refactoring Achievements

**Before:**
```csharp
// Switch statement on string event type
query = request.Filter switch
{
    "Pending" => query.Where(r => r.Status == RequestStatus.Pending),
    "Recent" => query.Where(r => r.CreatedAt >= DateTime.UtcNow.AddDays(-30)),
    "All" => query,
    _ => query.Where(r => r.Status == RequestStatus.Pending)
};

// Complex nested loops with private helper methods
foreach (var evt in events)
{
    switch (evt.EventType)
    {
        case "EnrollmentEventData":
            var data = JsonSerializer.Deserialize<EnrollmentEventData>(evt.EventData);
            // ... complex logic
            break;
        // More cases...
    }
}
```

**After:**
```csharp
// Polymorphic filter strategy
var filter = _filters.TryGetValue(request.Filter, out var found) ? found : _defaultFilter;
query = filter.Apply(query);

// Polymorphic event projectors
foreach (var evt in events)
{
    if (_eventProjectors.TryGetValue(evt.EventType, out var projector))
    {
        state = projector.Project(evt, state);
    }
}
```

**Result:**
- ✅ Zero switch statements
- ✅ Zero private helper methods
- ✅ Every class has single responsibility
- ✅ Open/Closed Principle throughout
- ✅ All components independently testable

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

**Immutable Value Objects:**
```csharp
// State for event replay - immutable record
public record StudentProgressionState
{
    public string CurrentRank { get; init; } = "Not Enrolled";
    public DateTime? EnrolledDate { get; init; }
    public DateTime? LastTestDate { get; init; }
    public string? LastTestNotes { get; init; }
}

// Projectors return new instances (functional style)
return currentState with
{
    CurrentRank = data.ToRank,
    LastTestDate = evt.OccurredAt
};
```

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

// Time-travel by replaying events with projectors
public async Task<StudentRankAtDate> GetRankAsOf(DateTime date)
{
    var events = await _dbContext.StudentProgressionEvents
        .Where(e => e.StudentId == studentId && e.OccurredAt <= date)
        .OrderBy(e => e.Version)
        .ToListAsync();
    
    var state = new StudentProgressionState();
    foreach (var evt in events)
    {
        if (_projectors.TryGetValue(evt.EventType, out var projector))
        {
            state = projector.Project(evt, state);
        }
    }
    return state;
}
```

### Frontend Architecture

**Angular 18** with:
- **Reactive Forms** - Complex validation
- **Material Design** - Professional UI components
- **Service Layer** - API abstraction with event sourcing endpoints
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
│  │  • Belt Progression Analytics (Visitor Pattern)       │  │
│  │  • Private Lesson Management (Strategy Filters)       │  │
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
│  │  │   • Polymorphic filters (Strategy pattern)       │   │
│  │  └─ Event Handlers: StudentPromoted, TestRecorded   │   │
│  │                                                       │   │
│  │  Event Projectors (Strategy + Template Method)       │   │
│  │  ├─ EnrollmentEventProjector                         │   │
│  │  ├─ PromotionEventProjector                          │   │
│  │  └─ TestAttemptEventProjector                        │   │
│  │                                                       │   │
│  │  Analytics Calculators (Visitor Pattern)             │   │
│  │  └─ AnalyticsResponseBuilder (accumulator)           │   │
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
│  │     • Time-travel via event projection               │   │
│  │     • Analytics via Visitor pattern                  │   │
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

- ✅ **Event Projectors with Strategy Pattern** - Zero switch statements, polymorphic event processing
- ✅ **Template Method Pattern** - DRY event deserialization and projection
- ✅ **Complete Event History** - Every test, promotion, enrollment stored as immutable events
- ✅ **Time-Travel Queries** - Reconstruct student rank at any point in history
- ✅ **Analytics with Visitor Pattern** - Type-safe aggregation of heterogeneous calculator results
  - Pass/fail rates
  - Average time to each belt rank (not hardcoded!)
  - Most active testing months
  - Rank distribution
- ✅ **Event Store** - Dedicated table with versioning and optimistic concurrency
- ✅ **Audit Trail** - Complete compliance-ready history
- ✅ **Domain Services** - Clean separation of query complexity

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
- ✅ **Admin Dashboard with Filter Strategies** - Polymorphic filtering (no switch statements)
  - Pending, Recent, All filters
  - Each filter is independently testable
  - Easy to add new filters
- ✅ **Intelligent Availability** - Checks business hours, class schedule conflicts, existing bookings
- ✅ **Domain Events** - Audit trail for approvals/rejections

### 🏗️ Architecture & Patterns

- ✅ **Domain-Driven Design** - Rich entities, value objects, aggregates
- ✅ **CQRS Pattern** - Commands and queries separated
- ✅ **Event Sourcing** - Immutable event stream for belt progression
- ✅ **Strategy Pattern** - Event projectors, query filters, analytics calculators
- ✅ **Template Method Pattern** - Shared event processing logic
- ✅ **Visitor Pattern** - Type-safe aggregation of analytics results
- ✅ **Vertical Slice Architecture** - Features organized by business capability
- ✅ **Domain Events with MediatR** - Decoupled event handling
- ✅ **Proper Encapsulation** - Private backing fields, controlled mutation, immutable state
- ✅ **Zero Switch Statements** - Pure polymorphism throughout
- ✅ **No Private Helper Methods** - Each responsibility is a testable class

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

### Immutable Value Objects
```csharp
// StudentProgressionState - Immutable record for event replay
public record StudentProgressionState
{
    public string CurrentRank { get; init; } = "Not Enrolled";
    public DateTime? EnrolledDate { get; init; }
    public DateTime? LastTestDate { get; init; }
    public string? LastTestNotes { get; init; }
}

// Event projectors use 'with' expressions for immutable updates
return currentState with
{
    CurrentRank = data.ToRank,
    LastTestDate = evt.OccurredAt,
    LastTestNotes = data.Reason
};

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

### Code Organization
```
/Features/
  /ProgressionAnalytics/
    ├── GetProgressionAnalyticsQuery.cs
    ├── GetProgressionAnalyticsHandler.cs
    ├── Calculators/
    │   ├── IProgressionAnalyticsCalculator.cs
    │   ├── EnrollmentCountCalculator.cs
    │   ├── TestStatisticsCalculator.cs
    │   ├── AverageTimeToRankCalculator.cs
    │   └── ... (each calculator is independently testable)
    ├── Results/
    │   ├── IAnalyticsResult.cs
    │   ├── IAnalyticsResultVisitor.cs
    │   ├── EnrollmentCountResult.cs
    │   └── ... (strongly-typed results)
    └── AnalyticsResponseBuilder.cs (Visitor accumulator)
    
  /StudentProgression/
    ├── Queries/
    │   └── GetStudentRankAtDate/
    │       ├── GetStudentRankAtDateQuery.cs
    │       ├── GetStudentRankAtDateHandler.cs
    │       └── Projectors/
    │           ├── IEventProjector.cs
    │           ├── EventProjectorBase.cs
    │           ├── EnrollmentEventProjector.cs
    │           ├── PromotionEventProjector.cs
    │           └── TestAttemptEventProjector.cs
    
  /PrivateLessons/
    └── Queries/
        └── GetPrivateLessons/
            ├── GetPrivateLessonsQuery.cs
            ├── GetPrivateLessonsHandler.cs
            └── Filters/
                ├── IPrivateLessonFilter.cs
                ├── PendingLessonsFilter.cs
                ├── RecentLessonsFilter.cs
                └── AllLessonsFilter.cs
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
- **Eliminating switch statements with event projectors**
- **Using Strategy + Template Method for DRY event processing**

**CQRS:**
- Separating read models from write models
- Command handlers that enforce business rules
- Query handlers optimized for specific UI needs
- Projections from event store to read models
- **Polymorphic query filters (no switch statements)**

**Design Patterns in Practice:**
- **Strategy Pattern** - Event projectors, query filters, analytics calculators
- **Template Method Pattern** - Shared event deserialization logic
- **Visitor Pattern** - Type-safe aggregation of heterogeneous results ("Visitor as Accumulator")
- **Factory Pattern** - Controlled object creation with validation
- **Domain Services** - Encapsulating complex query logic

**Domain-Driven Design:**
- Aggregate design with proper boundaries
- Encapsulation using private setters and backing fields
- Value objects for domain concepts (immutable records)
- Domain events for cross-cutting concerns
- Factory methods to prevent invalid state
- **No private helper methods - each responsibility is a class**

### Refactoring & Code Quality

**Eliminating Anti-Patterns:**
- ✅ Replaced all switch statements with polymorphism
- ✅ Removed all private helper methods (Single Responsibility)
- ✅ Achieved zero cyclomatic complexity violations
- ✅ Applied Open/Closed Principle throughout
- ✅ Made every component independently testable

**Pattern Composition:**
- Understanding how patterns work together (Strategy + Template Method + Visitor)
- Creating type hierarchies to enable patterns (Visitor pattern for analytics)
- Knowing when inheritance is appropriate vs. composition
- Balancing SOLID principles with pragmatism

### Technical Depth

**Backend:**
- MediatR for clean CQRS implementation
- EF Core migrations in CI/CD pipeline
- Minimal APIs with proper organization
- Domain event handling and dispatching
- JSON serialization for event data
- **Dependency injection for polymorphic strategies**
- **Generic base classes with Template Method**

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
- ✅ **Refactoring switch statements to polymorphic strategies**
- ✅ **Using Visitor pattern for heterogeneous result aggregation**
- ✅ **Extracting complex LINQ into domain services**
- ✅ EF Core configuration for value objects and encapsulated collections
- ✅ Automated migrations in CI/CD pipeline
- ✅ Time-travel query implementation with projectors

---

## Links

- **Live Application:** [https://jolly-smoke-0f6352e10.4.azurestaticapps.net](https://jolly-smoke-0f6352e10.4.azurestaticapps.net)
- **API Health Check:** [https://sumter-martial-arts-api.azurewebsites.net/health](https://sumter-martial-arts-api.azurewebsites.net/health)
- **GitHub Repository:** [https://github.com/CheesePizza100/SumterMartialArtsAzure](https://github.com/CheesePizza100/SumterMartialArtsAzure)

---

## Author

- GitHub: [@CheesePizza100](https://github.com/CheesePizza100)

---

## License

This project is for portfolio purposes.

---

**Built with ❤️ using .NET 8, Angular 18, Event Sourcing, CQRS, Advanced Design Patterns, and Microsoft Azure**

*Showcasing: Strategy Pattern • Template Method • Visitor Pattern • Domain-Driven Design • Event Sourcing • CQRS • Zero Switch Statements • Pure OOP*
