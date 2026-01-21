using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using System.Text.Json;

namespace SumterMartialArtsAzure.Server.Api;

public static class EventStoreSeeder
{
    public static void SeedEventStore(AppDbContext context)
    {
        // Don't seed if events already exist
        if (context.StudentProgressionEvents.Any())
            return;

        // Make sure students are seeded first
        if (!context.Students.Any())
            throw new InvalidOperationException("Students must be seeded before Event Store");

        var events = new List<StudentProgressionEvent>();

        // Get students and programs
        var sarahJohnson = context.Students.First(s => s.Email == "sarah.j@email.com");
        var marcusChen = context.Students.First(s => s.Email == "mchen@email.com");
        var emmaRodriguez = context.Students.First(s => s.Email == "e.rodriguez@email.com");
        var davidKim = context.Students.First(s => s.Email == "david.kim@email.com");
        var jessicaPatel = context.Students.First(s => s.Email == "j.patel@email.com");
        var michaelOBrien = context.Students.First(s => s.Email == "mobrien@email.com");
        var aishaWilliams = context.Students.First(s => s.Email == "awilliams@email.com");

        var americanJiuJitsu = context.Programs.First(p => p.Name == "American Jiu Jitsu");
        var kickboxing = context.Programs.First(p => p.Name == "Kickboxing");
        var judo = context.Programs.First(p => p.Name == "Judo");
        var submissionWrestling = context.Programs.First(p => p.Name == "Submission Wrestling");

        var mikeStevens = context.Instructors.First(i => i.Name == "Professor Mike Stevens");
        var sarahLee = context.Instructors.First(i => i.Name == "Coach Sarah Lee");
        var hiroMatsuda = context.Instructors.First(i => i.Name == "Sensei Hiro Matsuda");
        var adrianBrooks = context.Instructors.First(i => i.Name == "Coach Adrian Brooks");

        int eventVersion = 1;

        // ========== SARAH JOHNSON EVENT STREAM ==========
        var bjjEnrollDate = DateTime.UtcNow.AddYears(-2).AddMonths(-6);

        // Sarah enrolled in BJJ
        events.Add(CreateEvent<EnrollmentEventData>(
            sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name, bjjEnrollDate, eventVersion++,
            new EnrollmentEventData
            {
                InitialRank = "White Belt",
                InstructorId = mikeStevens.Id
            }
        ));

        // Sarah's BJJ test progression
        var sarahTestDates = new[]
        {
            bjjEnrollDate.AddMonths(3),  // Stripe 1
            bjjEnrollDate.AddMonths(6),  // Stripe 2
            bjjEnrollDate.AddMonths(9),  // Stripe 3
            bjjEnrollDate.AddMonths(12), // Stripe 4
            bjjEnrollDate.AddMonths(18), // Blue Belt
        };

        events.Add(CreateTestEvent(sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 1", true, "Good fundamentals. Keep working on guard retention.",
            sarahTestDates[0], mikeStevens.Id, eventVersion++));

        events.Add(CreatePromotionEvent(sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt", "White Belt - Stripe 1", "Good fundamentals",
            sarahTestDates[0], mikeStevens.Id, eventVersion++));

        events.Add(CreateTestEvent(sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 2", true, "Improving positional awareness. Escapes looking solid.",
            sarahTestDates[1], mikeStevens.Id, eventVersion++));

        events.Add(CreatePromotionEvent(sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 1", "White Belt - Stripe 2", "Improving positional awareness",
            sarahTestDates[1], mikeStevens.Id, eventVersion++));

        events.Add(CreateTestEvent(sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 3", true, "Good progress on submissions from guard.",
            sarahTestDates[2], mikeStevens.Id, eventVersion++));

        events.Add(CreatePromotionEvent(sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 2", "White Belt - Stripe 3", "Good progress",
            sarahTestDates[2], mikeStevens.Id, eventVersion++));

        events.Add(CreateTestEvent(sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 4", true, "Excellent fundamentals. Ready for blue belt testing soon.",
            sarahTestDates[3], mikeStevens.Id, eventVersion++));

        events.Add(CreatePromotionEvent(sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 3", "White Belt - Stripe 4", "Ready for blue belt",
            sarahTestDates[3], mikeStevens.Id, eventVersion++));

        events.Add(CreateTestEvent(sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "Blue Belt", true, "Strong performance in sparring and technique demonstration. Excellent guard passes.",
            sarahTestDates[4], mikeStevens.Id, eventVersion++));

        events.Add(CreatePromotionEvent(sarahJohnson.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 4", "Blue Belt", "Strong performance in sparring",
            sarahTestDates[4], mikeStevens.Id, eventVersion++));

        // Sarah enrolled in Kickboxing
        var kickboxingEnrollDate = DateTime.UtcNow.AddYears(-1);
        eventVersion = 1; // Reset for new program stream

        events.Add(CreateEvent<EnrollmentEventData>(
            sarahJohnson.Id, kickboxing.Id, kickboxing.Name, kickboxingEnrollDate, eventVersion++,
            new EnrollmentEventData
            {
                InitialRank = "Beginner",
                InstructorId = sarahLee.Id
            }
        ));

        // ========== MARCUS CHEN EVENT STREAM ==========
        eventVersion = 1;
        var muayThaiEnrollDate = DateTime.UtcNow.AddYears(-3).AddMonths(-2);

        events.Add(CreateEvent<EnrollmentEventData>(
            marcusChen.Id, kickboxing.Id, kickboxing.Name, muayThaiEnrollDate, eventVersion++,
            new EnrollmentEventData
            {
                InitialRank = "Beginner",
                InstructorId = sarahLee.Id
            }
        ));

        var marcusTestDates = new[]
        {
            muayThaiEnrollDate.AddMonths(4),
            muayThaiEnrollDate.AddMonths(10),
            muayThaiEnrollDate.AddMonths(18),
            muayThaiEnrollDate.AddMonths(30),
        };

        events.Add(CreateTestEvent(marcusChen.Id, kickboxing.Id, kickboxing.Name,
            "Intermediate Level 1", true, "Good power generation. Work on combinations.",
            marcusTestDates[0], sarahLee.Id, eventVersion++));
        events.Add(CreatePromotionEvent(marcusChen.Id, kickboxing.Id, kickboxing.Name,
            "Beginner", "Intermediate Level 1", "Good power generation",
            marcusTestDates[0], sarahLee.Id, eventVersion++));

        events.Add(CreateTestEvent(marcusChen.Id, kickboxing.Id, kickboxing.Name,
            "Intermediate Level 2", true, "Excellent technique refinement. Footwork improving.",
            marcusTestDates[1], sarahLee.Id, eventVersion++));
        events.Add(CreatePromotionEvent(marcusChen.Id, kickboxing.Id, kickboxing.Name,
            "Intermediate Level 1", "Intermediate Level 2", "Excellent technique",
            marcusTestDates[1], sarahLee.Id, eventVersion++));

        events.Add(CreateTestEvent(marcusChen.Id, kickboxing.Id, kickboxing.Name,
            "Intermediate Level 3", true, "Strong striking. Ready for advanced training.",
            marcusTestDates[2], sarahLee.Id, eventVersion++));
        events.Add(CreatePromotionEvent(marcusChen.Id, kickboxing.Id, kickboxing.Name,
            "Intermediate Level 2", "Intermediate Level 3", "Strong striking",
            marcusTestDates[2], sarahLee.Id, eventVersion++));

        events.Add(CreateTestEvent(marcusChen.Id, kickboxing.Id, kickboxing.Name,
            "Advanced", true, "Exceptional striking. Clinch work needs refinement.",
            marcusTestDates[3], sarahLee.Id, eventVersion++));
        events.Add(CreatePromotionEvent(marcusChen.Id, kickboxing.Id, kickboxing.Name,
            "Intermediate Level 3", "Advanced", "Exceptional striking",
            marcusTestDates[3], sarahLee.Id, eventVersion++));

        // ========== EMMA RODRIGUEZ EVENT STREAM ==========
        eventVersion = 1;
        var emmaEnrollDate = DateTime.UtcNow.AddMonths(-3);

        events.Add(CreateEvent<EnrollmentEventData>(
            emmaRodriguez.Id, americanJiuJitsu.Id, americanJiuJitsu.Name, emmaEnrollDate, eventVersion++,
            new EnrollmentEventData
            {
                InitialRank = "White Belt",
                InstructorId = mikeStevens.Id
            }
        ));
        // No tests yet for Emma

        // ========== DAVID KIM EVENT STREAM ==========
        eventVersion = 1;
        var davidBjjEnrollDate = DateTime.UtcNow.AddYears(-4).AddMonths(-3);

        events.Add(CreateEvent<EnrollmentEventData>(
            davidKim.Id, americanJiuJitsu.Id, americanJiuJitsu.Name, davidBjjEnrollDate, eventVersion++,
            new EnrollmentEventData
            {
                InitialRank = "White Belt",
                InstructorId = mikeStevens.Id
            }
        ));

        // David's extensive test history (abbreviated for brevity)
        var davidBjjTests = new[]
        {
            davidBjjEnrollDate.AddMonths(4),
            davidBjjEnrollDate.AddMonths(8),
            davidBjjEnrollDate.AddMonths(12),
            davidBjjEnrollDate.AddMonths(16),
            davidBjjEnrollDate.AddMonths(22), // Blue Belt
            davidBjjEnrollDate.AddMonths(28),
            davidBjjEnrollDate.AddMonths(34),
            davidBjjEnrollDate.AddMonths(40),
            davidBjjEnrollDate.AddMonths(46), // Purple Belt
        };

        // White belt progression
        for (int i = 0; i < 4; i++)
        {
            var stripe = i + 1;
            events.Add(CreateTestEvent(davidKim.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
                $"White Belt - Stripe {stripe}", true, "Progressing well",
                davidBjjTests[i], mikeStevens.Id, eventVersion++));
            events.Add(CreatePromotionEvent(davidKim.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
                i == 0 ? "White Belt" : $"White Belt - Stripe {i}",
                $"White Belt - Stripe {stripe}", "Progressing well",
                davidBjjTests[i], mikeStevens.Id, eventVersion++));
        }

        // Blue belt
        events.Add(CreateTestEvent(davidKim.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "Blue Belt", true, "Strong performance",
            davidBjjTests[4], mikeStevens.Id, eventVersion++));
        events.Add(CreatePromotionEvent(davidKim.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 4", "Blue Belt", "Strong performance",
            davidBjjTests[4], mikeStevens.Id, eventVersion++));

        // Blue belt stripes
        for (int i = 0; i < 4; i++)
        {
            var stripe = i + 1;
            events.Add(CreateTestEvent(davidKim.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
                $"Blue Belt - Stripe {stripe}", true, "Continuing progress",
                davidBjjTests[5 + i], mikeStevens.Id, eventVersion++));
            events.Add(CreatePromotionEvent(davidKim.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
                i == 0 ? "Blue Belt" : $"Blue Belt - Stripe {i}",
                $"Blue Belt - Stripe {stripe}", "Continuing progress",
                davidBjjTests[5 + i], mikeStevens.Id, eventVersion++));
        }

        // Purple belt
        var purpleBeltDate = DateTime.UtcNow.AddMonths(-6);
        events.Add(CreateTestEvent(davidKim.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "Purple Belt", true, "Solid all-around performance",
            purpleBeltDate, mikeStevens.Id, eventVersion++));
        events.Add(CreatePromotionEvent(davidKim.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "Blue Belt - Stripe 4", "Purple Belt", "Solid all-around performance",
            purpleBeltDate, mikeStevens.Id, eventVersion++));

        // David's wrestling enrollment
        eventVersion = 1; // Reset for new program
        var davidWrestlingEnrollDate = DateTime.UtcNow.AddYears(-1).AddMonths(-4);

        events.Add(CreateEvent<EnrollmentEventData>(
            davidKim.Id, submissionWrestling.Id, submissionWrestling.Name, davidWrestlingEnrollDate, eventVersion++,
            new EnrollmentEventData
            {
                InitialRank = "Beginner",
                InstructorId = adrianBrooks.Id
            }
        ));

        events.Add(CreateTestEvent(davidKim.Id, submissionWrestling.Id, submissionWrestling.Name,
            "Intermediate", true, "Natural talent for takedowns",
            DateTime.UtcNow.AddMonths(-2), adrianBrooks.Id, eventVersion++));
        events.Add(CreatePromotionEvent(davidKim.Id, submissionWrestling.Id, submissionWrestling.Name,
            "Beginner", "Intermediate", "Natural talent",
            DateTime.UtcNow.AddMonths(-2), adrianBrooks.Id, eventVersion++));

        // ========== JESSICA PATEL EVENT STREAM ==========
        eventVersion = 1;
        events.Add(CreateEvent<EnrollmentEventData>(
            jessicaPatel.Id, kickboxing.Id, kickboxing.Name, DateTime.UtcNow.AddMonths(-2), eventVersion++,
            new EnrollmentEventData
            {
                InitialRank = "Beginner",
                InstructorId = sarahLee.Id
            }
        ));

        // ========== MICHAEL O'BRIEN EVENT STREAM ==========
        eventVersion = 1;
        var michaelEnrollDate = DateTime.UtcNow.AddYears(-1).AddMonths(-2);

        events.Add(CreateEvent<EnrollmentEventData>(
            michaelOBrien.Id, americanJiuJitsu.Id, americanJiuJitsu.Name, michaelEnrollDate, eventVersion++,
            new EnrollmentEventData
            {
                InitialRank = "White Belt",
                InstructorId = mikeStevens.Id
            }
        ));

        // Test with failure and retry
        events.Add(CreateTestEvent(michaelOBrien.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 1", true, "Good fundamentals",
            michaelEnrollDate.AddMonths(3), mikeStevens.Id, eventVersion++));
        events.Add(CreatePromotionEvent(michaelOBrien.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt", "White Belt - Stripe 1", "Good fundamentals",
            michaelEnrollDate.AddMonths(3), mikeStevens.Id, eventVersion++));

        // Failed test - no promotion
        events.Add(CreateTestEvent(michaelOBrien.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 2", false, "Need more work on escapes",
            michaelEnrollDate.AddMonths(6), mikeStevens.Id, eventVersion++));

        // Successful retry
        events.Add(CreateTestEvent(michaelOBrien.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 2", true, "Much better! Escapes are solid now",
            michaelEnrollDate.AddMonths(8), mikeStevens.Id, eventVersion++));
        events.Add(CreatePromotionEvent(michaelOBrien.Id, americanJiuJitsu.Id, americanJiuJitsu.Name,
            "White Belt - Stripe 1", "White Belt - Stripe 2", "Much better",
            michaelEnrollDate.AddMonths(8), mikeStevens.Id, eventVersion++));

        // ========== AISHA WILLIAMS EVENT STREAM ==========
        eventVersion = 1;
        var aishaEnrollDate = DateTime.UtcNow.AddYears(-1).AddMonths(-8);

        events.Add(CreateEvent<EnrollmentEventData>(
            aishaWilliams.Id, judo.Id, judo.Name, aishaEnrollDate, eventVersion++,
            new EnrollmentEventData
            {
                InitialRank = "White Belt",
                InstructorId = hiroMatsuda.Id
            }
        ));

        events.Add(CreateTestEvent(aishaWilliams.Id, judo.Id, judo.Name,
            "Yellow Belt", true, "Excellent ukemi skills",
            aishaEnrollDate.AddMonths(4), hiroMatsuda.Id, eventVersion++));
        events.Add(CreatePromotionEvent(aishaWilliams.Id, judo.Id, judo.Name,
            "White Belt", "Yellow Belt", "Excellent ukemi",
            aishaEnrollDate.AddMonths(4), hiroMatsuda.Id, eventVersion++));

        events.Add(CreateTestEvent(aishaWilliams.Id, judo.Id, judo.Name,
            "Orange Belt", true, "Strong technical throws",
            aishaEnrollDate.AddMonths(10), hiroMatsuda.Id, eventVersion++));
        events.Add(CreatePromotionEvent(aishaWilliams.Id, judo.Id, judo.Name,
            "Yellow Belt", "Orange Belt", "Competition ready",
            aishaEnrollDate.AddMonths(10), hiroMatsuda.Id, eventVersion++));

        // Save all events
        context.StudentProgressionEvents.AddRange(events);
        context.SaveChanges();
    }

    // Helper methods for creating events
    private static StudentProgressionEvent CreateEvent<T>(
        int studentId, int programId, string programName, DateTime occurredAt, int version,
        T eventData) where T : class
    {
        var eventType = typeof(T).Name;
        var eventJson = JsonSerializer.Serialize(eventData);

        return new StudentProgressionEvent
        {
            EventId = Guid.NewGuid(),
            StudentId = studentId,
            ProgramId = programId,
            EventType = eventType,
            EventData = eventJson,
            OccurredAt = occurredAt,
            Version = version
        };
    }

    private static StudentProgressionEvent CreateTestEvent(
        int studentId, int programId, string programName, string rankTested,
        bool passed, string notes, DateTime testDate, int instructorId, int version)
    {
        var testEvent = new TestAttemptEventData
        {
            RankTested = rankTested,
            Passed = passed,
            TestingInstructorId = instructorId,
            InstructorNotes = notes,
            TechniqueScores = new Dictionary<string, int>() // Could add specific scores
        };

        return CreateEvent(studentId, programId, programName, testDate, version, testEvent);
    }

    private static StudentProgressionEvent CreatePromotionEvent(
        int studentId, int programId, string programName,
        string fromRank, string toRank, string reason,
        DateTime promotedAt, int instructorId, int version)
    {
        var promotionEvent = new PromotionEventData
        {
            FromRank = fromRank,
            ToRank = toRank,
            PromotingInstructorId = instructorId,
            Reason = reason
        };

        return CreateEvent(studentId, programId, programName, promotedAt, version, promotionEvent);
    }
}