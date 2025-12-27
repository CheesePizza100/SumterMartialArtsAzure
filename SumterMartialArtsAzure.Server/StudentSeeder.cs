using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api;

public static class StudentSeeder
{
    public static void SeedStudents(AppDbContext context)
    {
        // Don't seed if students already exist
        if (context.Students.Any())
            return;

        // Make sure programs and instructors are seeded first
        if (!context.Programs.Any() || !context.Instructors.Any())
            throw new InvalidOperationException("Programs and Instructors must be seeded before Students");

        // Get references to programs (after DbSeeder has run)
        var kidsMartialArts = context.Programs.First(p => p.Name == "Kids Martial Arts");
        var americanJiuJitsu = context.Programs.First(p => p.Name == "American Jiu Jitsu");
        var judo = context.Programs.First(p => p.Name == "Judo");
        var kickboxing = context.Programs.First(p => p.Name == "Kickboxing");
        var submissionWrestling = context.Programs.First(p => p.Name == "Submission Wrestling");
        var competitionPrep = context.Programs.First(p => p.Name == "Competition Prep");

        // Get instructors
        var mikeStevens = context.Instructors.First(i => i.Name == "Professor Mike Stevens");
        var hiroMatsuda = context.Instructors.First(i => i.Name == "Sensei Hiro Matsuda");
        var sarahLee = context.Instructors.First(i => i.Name == "Coach Sarah Lee");
        var adrianBrooks = context.Instructors.First(i => i.Name == "Coach Adrian Brooks");

        // ========== STUDENT 1: Sarah Johnson - Advanced BJJ Student ==========
        var sarahJohnson = Student.Create(
            name: "Sarah Johnson",
            email: "sarah.j@email.com",
            phone: "(555) 123-4567"
        );

        // Enrolled in BJJ 2.5 years ago
        var bjjEnrollDate = DateTime.UtcNow.AddYears(-2).AddMonths(-6);
        sarahJohnson.EnrollInProgram(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt"
        );

        // Also in Kickboxing for cross-training (1 year ago)
        var kickboxingEnrollDate = DateTime.UtcNow.AddYears(-1);
        sarahJohnson.EnrollInProgram(
            kickboxing.Id,
            kickboxing.Name,
            "Beginner"
        );

        // Record attendance - 156 total classes, 18 in last 30 days
        for (int i = 0; i < 15; i++) // Simulate attendance over time
        {
            sarahJohnson.RecordAttendance(10);
        }
        sarahJohnson.RecordAttendance(6); // Recent classes

        // BJJ Test History - Progressive journey to Blue Belt
        var testDates = new[]
        {
            bjjEnrollDate.AddMonths(3),  // Stripe 1
            bjjEnrollDate.AddMonths(6),  // Stripe 2
            bjjEnrollDate.AddMonths(9),  // Stripe 3
            bjjEnrollDate.AddMonths(12), // Stripe 4
            bjjEnrollDate.AddMonths(18), // Blue Belt
        };

        sarahJohnson.RecordTestResult(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt - Stripe 1",
            passed: true,
            notes: "Good fundamentals. Keep working on guard retention.",
            testDate: testDates[0]
        );

        sarahJohnson.RecordTestResult(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt - Stripe 2",
            passed: true,
            notes: "Improving positional awareness. Escapes looking solid.",
            testDate: testDates[1]
        );

        sarahJohnson.RecordTestResult(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt - Stripe 3",
            passed: true,
            notes: "Good progress on submissions from guard.",
            testDate: testDates[2]
        );

        sarahJohnson.RecordTestResult(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt - Stripe 4",
            passed: true,
            notes: "Excellent fundamentals. Ready for blue belt testing soon.",
            testDate: testDates[3]
        );

        sarahJohnson.RecordTestResult(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "Blue Belt",
            passed: true,
            notes: "Strong performance in sparring and technique demonstration. Excellent guard passes.",
            testDate: testDates[4]
        );

        // Update program notes
        sarahJohnson.UpdateProgramNotes(
            americanJiuJitsu.Id,
            "Excellent technique on guard passes. Ready for purple belt within 6-8 months with continued progress."
        );

        // ========== STUDENT 2: Marcus Chen - Advanced Kickboxing Student ==========
        var marcusChen = Student.Create(
            name: "Marcus Chen",
            email: "mchen@email.com",
            phone: "(555) 234-5678"
        );

        var muayThaiEnrollDate = DateTime.UtcNow.AddYears(-3).AddMonths(-2);
        marcusChen.EnrollInProgram(
            kickboxing.Id,
            kickboxing.Name,
            "Beginner"
        );

        // High attendance - 312 total, 24 in last 30 days
        for (int i = 0; i < 29; i++)
        {
            marcusChen.RecordAttendance(10);
        }
        marcusChen.RecordAttendance(22);

        // Kickboxing progression
        var kickboxingTests = new[]
        {
            muayThaiEnrollDate.AddMonths(4),  // Intermediate Level 1
            muayThaiEnrollDate.AddMonths(10), // Intermediate Level 2
            muayThaiEnrollDate.AddMonths(18), // Intermediate Level 3
            muayThaiEnrollDate.AddMonths(30), // Advanced
        };

        marcusChen.RecordTestResult(
            kickboxing.Id,
            kickboxing.Name,
            "Intermediate Level 1",
            passed: true,
            notes: "Good power generation. Work on combinations.",
            testDate: kickboxingTests[0]
        );

        marcusChen.RecordTestResult(
            kickboxing.Id,
            kickboxing.Name,
            "Intermediate Level 2",
            passed: true,
            notes: "Excellent technique refinement. Footwork improving.",
            testDate: kickboxingTests[1]
        );

        marcusChen.RecordTestResult(
            kickboxing.Id,
            kickboxing.Name,
            "Intermediate Level 3",
            passed: true,
            notes: "Strong striking. Ready for advanced training.",
            testDate: kickboxingTests[2]
        );

        marcusChen.RecordTestResult(
            kickboxing.Id,
            kickboxing.Name,
            "Advanced",
            passed: true,
            notes: "Exceptional striking. Clinch work needs refinement. Ready for instructor training program.",
            testDate: kickboxingTests[3]
        );

        marcusChen.UpdateProgramNotes(
            kickboxing.Id,
            "Exceptional striking. Clinch work needs refinement."
        );

        // ========== STUDENT 3: Emma Rodriguez - New Student ==========
        var emmaRodriguez = Student.Create(
            name: "Emma Rodriguez",
            email: "e.rodriguez@email.com",
            phone: "(555) 345-6789"
        );

        var emmaEnrollDate = DateTime.UtcNow.AddMonths(-3);
        emmaRodriguez.EnrollInProgram(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt"
        );

        // New student - 42 classes total, 14 in last 30 days
        emmaRodriguez.RecordAttendance(28);
        emmaRodriguez.RecordAttendance(14);

        // No tests yet - still building fundamentals

        // ========== STUDENT 4: David Kim - Multi-Program Student ==========
        var davidKim = Student.Create(
            name: "David Kim",
            email: "david.kim@email.com",
            phone: "(555) 456-7890"
        );

        // BJJ for 4+ years
        var davidBjjEnrollDate = DateTime.UtcNow.AddYears(-4).AddMonths(-3);
        davidKim.EnrollInProgram(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt"
        );

        // Wrestling for cross-training (1.5 years ago)
        var davidWrestlingEnrollDate = DateTime.UtcNow.AddYears(-1).AddMonths(-4);
        davidKim.EnrollInProgram(
            submissionWrestling.Id,
            submissionWrestling.Name,
            "Beginner"
        );

        // High attendance - 428 total, 20 in last 30 days
        for (int i = 0; i < 40; i++)
        {
            davidKim.RecordAttendance(10);
        }
        davidKim.RecordAttendance(28);

        // BJJ Journey to Purple Belt
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

        // White belt stripes
        davidKim.RecordTestResult(americanJiuJitsu.Id, americanJiuJitsu.Name, "White Belt - Stripe 1", true, "Good start", davidBjjTests[0]);
        davidKim.RecordTestResult(americanJiuJitsu.Id, americanJiuJitsu.Name, "White Belt - Stripe 2", true, "Solid progress", davidBjjTests[1]);
        davidKim.RecordTestResult(americanJiuJitsu.Id, americanJiuJitsu.Name, "White Belt - Stripe 3", true, "Technique improving", davidBjjTests[2]);
        davidKim.RecordTestResult(americanJiuJitsu.Id, americanJiuJitsu.Name, "White Belt - Stripe 4", true, "Ready for blue", davidBjjTests[3]);

        // Blue belt
        davidKim.RecordTestResult(americanJiuJitsu.Id, americanJiuJitsu.Name, "Blue Belt", true, "Strong performance", davidBjjTests[4]);

        // Blue belt stripes
        davidKim.RecordTestResult(americanJiuJitsu.Id, americanJiuJitsu.Name, "Blue Belt - Stripe 1", true, "Good guard game developing", davidBjjTests[5]);
        davidKim.RecordTestResult(americanJiuJitsu.Id, americanJiuJitsu.Name, "Blue Belt - Stripe 2", true, "Passing improving", davidBjjTests[6]);
        davidKim.RecordTestResult(americanJiuJitsu.Id, americanJiuJitsu.Name, "Blue Belt - Stripe 3", true, "Advanced techniques", davidBjjTests[7]);
        davidKim.RecordTestResult(americanJiuJitsu.Id, americanJiuJitsu.Name, "Blue Belt - Stripe 4", true, "Ready for purple belt promotion", davidBjjTests[8]);

        // Purple belt
        davidKim.RecordTestResult(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "Purple Belt",
            true,
            "Solid all-around performance. Strong guard game, needs work on passing from top position.",
            DateTime.UtcNow.AddMonths(-6)
        );

        // Wrestling progression
        davidKim.RecordTestResult(
            submissionWrestling.Id,
            submissionWrestling.Name,
            "Intermediate",
            true,
            "Natural talent for takedowns. Excellent technique refinement.",
            DateTime.UtcNow.AddMonths(-2)
        );

        davidKim.UpdateProgramNotes(
            americanJiuJitsu.Id,
            "Strong guard game. Needs work on passing from top position."
        );

        davidKim.UpdateProgramNotes(
            submissionWrestling.Id,
            "Natural talent for takedowns."
        );

        // ========== STUDENT 5: Jessica Patel - Very New Student ==========
        var jessicaPatel = Student.Create(
            name: "Jessica Patel",
            email: "j.patel@email.com",
            phone: "(555) 567-8901"
        );

        jessicaPatel.EnrollInProgram(
            kickboxing.Id,
            kickboxing.Name,
            "Beginner"
        );

        // Just started - 16 classes, 8 in last 30 days
        jessicaPatel.RecordAttendance(8);
        jessicaPatel.RecordAttendance(8);

        // No test history yet

        // ========== STUDENT 6: Michael O'Brien - Student with Failed Test ==========
        var michaelOBrien = Student.Create(
            name: "Michael O'Brien",
            email: "mobrien@email.com",
            phone: "(555) 678-9012"
        );

        var michaelEnrollDate = DateTime.UtcNow.AddYears(-1).AddMonths(-2);
        michaelOBrien.EnrollInProgram(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt"
        );

        // Moderate attendance - 98 total, 12 in last 30 days
        for (int i = 0; i < 8; i++)
        {
            michaelOBrien.RecordAttendance(10);
        }
        michaelOBrien.RecordAttendance(18);

        // Progressive tests with one failure
        michaelOBrien.RecordTestResult(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt - Stripe 1",
            true,
            "Good fundamentals. Keep training consistently.",
            michaelEnrollDate.AddMonths(3)
        );

        michaelOBrien.RecordTestResult(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt - Stripe 2",
            false,
            "Need more work on escapes. Attend more classes and focus on defensive positions.",
            michaelEnrollDate.AddMonths(6)
        );

        michaelOBrien.RecordTestResult(
            americanJiuJitsu.Id,
            americanJiuJitsu.Name,
            "White Belt - Stripe 2",
            true,
            "Much better! Escapes are solid now. Good work.",
            michaelEnrollDate.AddMonths(8)
        );

        michaelOBrien.UpdateProgramNotes(
            americanJiuJitsu.Id,
            "Showing improvement after focusing on fundamentals. Keep up the attendance."
        );

        // ========== STUDENT 7: Aisha Williams - Judo Student ==========
        var aishaWilliams = Student.Create(
            name: "Aisha Williams",
            email: "awilliams@email.com",
            phone: "(555) 789-0123"
        );

        var aishaEnrollDate = DateTime.UtcNow.AddYears(-1).AddMonths(-8);
        aishaWilliams.EnrollInProgram(
            judo.Id,
            judo.Name,
            "White Belt"
        );

        // Good attendance - 164 total, 16 in last 30 days
        for (int i = 0; i < 14; i++)
        {
            aishaWilliams.RecordAttendance(10);
        }
        aishaWilliams.RecordAttendance(24);

        // Judo progression
        aishaWilliams.RecordTestResult(
            judo.Id,
            judo.Name,
            "Yellow Belt",
            true,
            "Excellent ukemi (falling) skills. Good hip movement on throws.",
            aishaEnrollDate.AddMonths(4)
        );

        aishaWilliams.RecordTestResult(
            judo.Id,
            judo.Name,
            "Orange Belt",
            true,
            "Strong technical throws. Competition ready if interested.",
            aishaEnrollDate.AddMonths(10)
        );

        aishaWilliams.UpdateProgramNotes(
            judo.Id,
            "Excellent technical execution. Natural talent for timing and balance."
        );

        // Add all students to context
        var students = new List<Student>
        {
            sarahJohnson,
            marcusChen,
            emmaRodriguez,
            davidKim,
            jessicaPatel,
            michaelOBrien,
            aishaWilliams
        };

        context.Students.AddRange(students);
        context.SaveChanges();
    }
}