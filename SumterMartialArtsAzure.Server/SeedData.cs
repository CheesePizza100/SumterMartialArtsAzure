using SumterMartialArtsAzure.Server.Api;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api
{
    public static class SeedData
    {
        //public static readonly List<Program> Programs = new List<Program>
        //{
        //    new Program
        //    {
        //        Id = 1,
        //        Name = "Kids Martial Arts",
        //        AgeGroup = "Ages 4–12",
        //        Description = "Fun, disciplined learning for young martial artists.",
        //        Details = "Our Kids Martial Arts program builds focus, confidence, and discipline while keeping classes fun and engaging. Students learn age-appropriate techniques, teamwork, and respect.",
        //        Duration = "45 minutes",
        //        Schedule = "Mon/Wed/Fri at 5:00 PM",
        //        ImageUrl = "https://placehold.co/800x400?text=Kids+Martial+Arts",
        //        InstructorIds = [1, 2]
        //    },

        //    new Program
        //    {
        //        Id = 2,
        //        Name = "American Jiu Jitsu",
        //        AgeGroup = "Teens & Adults",
        //        Description = "Strength, skill, and confidence through modern Jiu Jitsu.",
        //        Details = "American Jiu Jitsu combines traditional grappling with modern self-defense and sport techniques. Classes focus on practical skills, leverage, and confidence.",
        //        Duration = "60 minutes",
        //        Schedule = "Tue/Thu at 7:00 PM",
        //        ImageUrl = "https://placehold.co/800x400?text=American+Jiu+Jitsu",
        //        InstructorIds = [3]
        //    },
        //    new Program
        //    {
        //        Id = 3,
        //        Name = "Judo",
        //        AgeGroup = "Teens & Adults",
        //        Description = "Powerful throws and real-world technique.",
        //        Details = "Traditional Judo focused on balance, timing, and powerful throws. Excellent for self-defense and improving athletic coordination.",
        //        Duration = "60 minutes",
        //        Schedule = "Mon/Wed at 7:00 PM",
        //        ImageUrl = "https://placehold.co/800x400?text=Judo",
        //        InstructorIds = [4]
        //    },
        //    new Program
        //    {
        //        Id = 4,
        //        Name = "Kickboxing",
        //        AgeGroup = "Adults",
        //        Description = "High-energy striking and conditioning.",
        //        Details = "Combining fitness and striking, Kickboxing is perfect for anyone wanting to improve cardio, build confidence, and learn practical stand-up skills.",
        //        Duration = "50 minutes",
        //        Schedule = "Tue/Thu at 6:00 PM",
        //        ImageUrl = "https://placehold.co/800x400?text=Kickboxing",
        //        InstructorIds = [5]
        //    },
        //    new Program
        //    {
        //        Id = 5,
        //        Name = "Submission Wrestling",
        //        AgeGroup = "Adults",
        //        Description = "Technical no-gi grappling for all levels.",
        //        Details = "A fast-paced grappling style emphasizing control, submissions, and positional strategy. Great for fitness or competitive goals.",
        //        Duration = "60 minutes",
        //        Schedule = "Fri at 7:00 PM",
        //        ImageUrl = "https://placehold.co/800x400?text=Submission+Wrestling",
        //        InstructorIds = [6]
        //    },
        //    new Program
        //    {
        //        Id = 6,
        //        Name = "Competition Prep",
        //        AgeGroup = "Advanced Students",
        //        Description = "High-level training for tournaments.",
        //        Details = "Advanced training sessions designed to prepare serious martial artists for local and national competition. Includes strategy, conditioning, and live rounds.",
        //        Duration = "90 minutes",
        //        Schedule = "Sat at 10:00 AM",
        //        ImageUrl = "https://placehold.co/800x400?text=Competition+Prep",
        //        InstructorIds = [7]
        //    }
        //};

        //public static readonly List<Instructor> Instructors = new List<Instructor>
        //{
        //    new Instructor
        //    {
        //        Id = 1,
        //        Name = "Sensei Laura Kim",
        //        Rank = "3rd Degree Black Belt",
        //        Bio = "Specializes in children's martial arts development.",
        //        PhotoUrl = "https://placehold.co/300x300?text=Laura",
        //        ProgramIds = new List<int>() { 1 },
        //        Achievements = new List<string>
        //        {
        //            "Certified Youth Martial Arts Instructor",
        //            "15+ Years Teaching Experience",
        //            "Developed Award-Winning Kids Curriculum",
        //            "Regional Champion 2015–2017"
        //        },
        //        AvailabilityRules = new List<AvailabilityRule>
        //        {
        //            new AvailabilityRule(
        //                new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
        //                TimeSpan.FromHours(17),    // 5 PM
        //                TimeSpan.FromMinutes(60)   // 1-hour lessons
        //            )
        //        }
        //    },

        //    new Instructor
        //    {
        //        Id = 2,
        //        Name = "Coach Daniel Ruiz",
        //        Rank = "1st Degree Black Belt",
        //        Bio = "Focuses on discipline and confidence building.",
        //        PhotoUrl = "https://placehold.co/300x300?text=Daniel",
        //        ProgramIds = new List<int>() { 1 },
        //        Achievements = new List<string>
        //        {
        //            "Former Amateur Kickboxing Competitor",
        //            "Certified Personal Fitness Trainer",
        //            "Youth Leadership Mentor",
        //            "Assistant Coach for National Junior Team 2022"
        //        },
        //        AvailabilityRules = new List<AvailabilityRule>
        //        {
        //            new AvailabilityRule(
        //                new[] { DayOfWeek.Monday, DayOfWeek.Thursday },
        //                TimeSpan.FromHours(17),
        //                TimeSpan.FromMinutes(60)
        //            )
        //        }
        //    },

        //    new Instructor
        //    {
        //        Id = 3,
        //        Name = "Professor Mike Stevens",
        //        Rank = "Black Belt",
        //        Bio = "25 years of grappling and competitive experience.",
        //        PhotoUrl = "https://placehold.co/300x300?text=Mike",
        //        ProgramIds = new List<int>() { 2 },
        //        Achievements = new List<string>
        //        {
        //            "Pan-American Grappling Bronze Medalist",
        //            "Multiple-Time State Champion",
        //            "Coach of 12 National-Level Competitors",
        //            "IBJJF Certified Instructor"
        //        },
        //        AvailabilityRules = new List<AvailabilityRule>
        //        {
        //            new AvailabilityRule(
        //                new[] { DayOfWeek.Tuesday, DayOfWeek.Thursday },
        //                TimeSpan.FromHours(19), // 7 PM
        //                TimeSpan.FromMinutes(60)
        //            )
        //        }
        //    },

        //    new Instructor
        //    {
        //        Id = 4,
        //        Name = "Sensei Hiro Matsuda",
        //        Rank = "4th Dan",
        //        Bio = "Trained in Tokyo, 30 years teaching experience.",
        //        PhotoUrl = "https://placehold.co/300x300?text=Hiro",
        //        ProgramIds = new List<int>() { 3 },
        //        Achievements = new List<string>
        //        {
        //            "Trained at Kodokan Institute (Tokyo)",
        //            "30+ Years Traditional Judo Experience",
        //            "Former International Judo Federation Competitor",
        //            "Mentor to Over 40 Black Belts Worldwide"
        //        },
        //        AvailabilityRules = new List<AvailabilityRule>
        //        {
        //            new AvailabilityRule(
        //                new[] { DayOfWeek.Wednesday, DayOfWeek.Saturday },
        //                TimeSpan.FromHours(19), // 7 PM
        //                TimeSpan.FromMinutes(60)
        //            )
        //        }
        //    },

        //    new Instructor
        //    {
        //        Id = 5,
        //        Name = "Coach Sarah Lee",
        //        Rank = "Muay Thai Practitioner",
        //        Bio = "Known for technical precision and athletic conditioning.",
        //        PhotoUrl = "https://placehold.co/300x300?text=Sarah",
        //        ProgramIds = new List<int>() { 4 },
        //        Achievements = new List<string>
        //        {
        //            "Fought Professionally in Thailand (Chiang Mai Circuit)",
        //            "Certified Muay Thai Conditioning Coach",
        //            "Women's Division Regional Champion 2019",
        //            "Designed SMA's Kickboxing Conditioning Program"
        //        },
        //        AvailabilityRules = new List<AvailabilityRule>
        //        {
        //            new AvailabilityRule(
        //                new[] { DayOfWeek.Saturday },
        //                TimeSpan.FromHours(18), // 6 PM
        //                TimeSpan.FromMinutes(50)
        //            )
        //        }
        //    },

        //    new Instructor
        //    {
        //        Id = 6,
        //        Name = "Coach Adrian Brooks",
        //        Rank = "Brown Belt",
        //        Bio = "Competition-focused grappling specialist.",
        //        PhotoUrl = "https://placehold.co/300x300?text=Adrian",
        //        ProgramIds = new List<int>() { 5 },
        //        Achievements = new List<string>
        //        {
        //            "NAGA Gold Medalist",
        //            "Active Submission-Only Competitor",
        //            "Expert in No-Gi Transitions & Leg Locks",
        //            "Assistant Coach for SMA Competition Team"
        //        },
        //        AvailabilityRules = new List<AvailabilityRule>
        //        {
        //            new AvailabilityRule(
        //                new[] { DayOfWeek.Monday, DayOfWeek.Friday },
        //                TimeSpan.FromHours(17),
        //                TimeSpan.FromMinutes(60)
        //            )
        //        }
        //    },

        //    new Instructor
        //    {
        //        Id = 7,
        //        Name = "Coach Brian Turner",
        //        Rank = "Black Belt",
        //        Bio = "National-level competitor with over 200 matches.",
        //        PhotoUrl = "https://placehold.co/300x300?text=Brian",
        //        ProgramIds = new List<int>() { 6 },
        //        Achievements = new List<string>
        //        {
        //            "Over 200 Competitive Matches",
        //            "National Grappling Silver Medalist",
        //            "Former Captain of SMA Competition Team",
        //            "Certified Referee for State-Level Tournaments"
        //        },
        //        AvailabilityRules = new List<AvailabilityRule>
        //        {
        //            new AvailabilityRule(
        //                new[] { DayOfWeek.Tuesday, DayOfWeek.Friday },
        //                TimeSpan.FromHours(10), // 10 AM for Competition Prep
        //                TimeSpan.FromMinutes(90)
        //            )
        //        }
        //    }
        //};

        //private static List<DateTime> GenerateDates(DayOfWeek[] days, int count)
        //{
        //    var results = new List<DateTime>();
        //    var date = DateTime.Today;

        //    while (results.Count < count)
        //    {
        //        if (days.Contains(date.DayOfWeek))
        //        {
        //            // Set all private lessons at 5 PM (adjustable)
        //            results.Add(date.AddHours(17));
        //        }

        //        date = date.AddDays(1);
        //    }

        //    return results;
        //}
    }
    //public class Program
    //{
    //    public int Id { get; set; }
    //    public required string Name { get; set; }
    //    public required string Description { get; set; }
    //    public string AgeGroup { get; set; }
    //    public string Details { get; set; }
    //    public string? ImageUrl { get; set; }

    //    // Optional properties used in the enhanced UI
    //    public string Duration { get; set; }      // e.g. "60 minutes"
    //    public string Schedule { get; set; }      // e.g. "Mon/Wed at 6pm"
    //    public List<int> InstructorIds { get; set; } = new();
    //}

    //public class Instructor
    //{
    //    public int Id { get; set; }
    //    public required string Name { get; set; }
    //    public string Rank { get; set; }
    //    public string Bio { get; set; }
    //    public string PhotoUrl { get; set; }
    //    public List<int> ProgramIds { get; set; } = new();
    //    public List<string> Achievements { get; set; } = new();
    //    public List<AvailabilityRule> AvailabilityRules { get; set; } = new List<AvailabilityRule>();
    //}
    //public sealed class AvailabilityRule
    //{
    //    public DayOfWeek[] DaysOfWeek { get; }
    //    public TimeSpan StartTime { get; }  // e.g., 17:00 = 5PM
    //    public TimeSpan Duration { get; }

    //    public AvailabilityRule(DayOfWeek[] daysOfWeek, TimeSpan startTime, TimeSpan duration)
    //    {
    //        DaysOfWeek = daysOfWeek;
    //        StartTime = startTime;
    //        Duration = duration;
    //    }

    //    public IEnumerable<TimeRange> GenerateNextOccurrences(DateTime from, int count)
    //    {
    //        var results = new List<TimeRange>();
    //        var date = from.Date;

    //        while (results.Count < count)
    //        {
    //            if (DaysOfWeek.Contains(date.DayOfWeek))
    //            {
    //                var start = date.Add(StartTime);
    //                var end = start.Add(Duration);
    //                results.Add(new TimeRange(start, end));
    //            }

    //            date = date.AddDays(1);
    //        }

    //        return results;
    //    }
    //}
    //public sealed class TimeRange
    //{
    //    public DateTime Start { get; }
    //    public DateTime End { get; }

    //    public TimeRange(DateTime start, DateTime end)
    //    {
    //        Start = start;
    //        End = end;
    //    }

    //    public bool Overlaps(TimeRange other)
    //        => Start < other.End && End > other.Start;

    //    public bool Contains(DateTime moment)
    //        => Start <= moment && moment < End;
    //}
}
//public class PrivateLessonRequest
//{
//    public int Id { get; set; }
//    public int InstructorId { get; set; }
//    public string StudentName { get; set; } = "";
//    public string Email { get; set; } = "";
//    public string Phone { get; set; } = "";
//    public DateTime PreferredDate { get; set; }
//    public string? Message { get; set; }
//    public string Status { get; set; } = "Pending";
//    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//    public TimeRange ScheduledTimeRange { get; set; }

//}

