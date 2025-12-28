using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Api;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Programs.Any() || context.Instructors.Any())
        {
            // Programs/Instructors already seeded, but check students
            if (!context.Students.Any())
            {
                StudentSeeder.SeedStudents(context);
                EventStoreSeeder.SeedEventStore(context);
            }
            return;
        }

        Instructor lauraKim = new Instructor()
        {
            Name = "Sensei Laura Kim",
            Rank = "3rd Degree Black Belt",
            Bio = "Specializes in children's martial arts development.",
            PhotoUrl = "https://placehold.co/300x300?text=Laura",
        };
        List<string> lauraKimAchievements = new List<string>()
        {
            "Certified Youth Martial Arts Instructor",
            "15+ Years Teaching Experience",
            "Developed Award-Winning Kids Curriculum",
            "Regional Champion 2015–2017"
        };
        lauraKim.AddAchievements(lauraKimAchievements);
        List<AvailabilityRule> lauraKimAvailabilityRules = new List<AvailabilityRule>()
        {
            new AvailabilityRule
            (
                [DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday],
                TimeSpan.FromHours(17),
                TimeSpan.FromMinutes(60)
            )
        };
        lauraKim.AddAvailabilityRules(lauraKimAvailabilityRules);

        Instructor danielRuiz = new Instructor()
        {
            Name = "Coach Daniel Ruiz",
            Rank = "1st Degree Black Belt",
            Bio = "Focuses on discipline and confidence building.",
            PhotoUrl = "https://placehold.co/300x300?text=Daniel",
        };
        List<string> danielRuizAchievements = new List<string>()
        {
            "Former Amateur Kickboxing Competitor",
            "Certified Personal Fitness Trainer",
            "Youth Leadership Mentor",
            "Assistant Coach for National Junior Team 2022"
        };
        danielRuiz.AddAchievements(danielRuizAchievements);
        List<AvailabilityRule> danielRuizAvailabilityRules = new List<AvailabilityRule>()
        {
            new AvailabilityRule
            (
                [DayOfWeek.Monday, DayOfWeek.Thursday],
                TimeSpan.FromHours(17),
                TimeSpan.FromMinutes(60)
            )
        };
        danielRuiz.AddAvailabilityRules(danielRuizAvailabilityRules);


        Instructor mikeStevens = new Instructor()
        {
            Name = "Professor Mike Stevens",
            Rank = "Black Belt",
            Bio = "25 years of grappling and competitive experience.",
            PhotoUrl = "https://placehold.co/300x300?text=Mike",
        };
        List<string> mikeStevensAchievements = new List<string>()
        {
            "Pan-American Grappling Bronze Medalist",
            "Multiple-Time State Champion",
            "Coach of 12 National-Level Competitors",
            "IBJJF Certified Instructor"
        };
        mikeStevens.AddAchievements(mikeStevensAchievements);
        List<AvailabilityRule> mikeStevensAvailabilityRules = new List<AvailabilityRule>()
        {
            new AvailabilityRule
            (
                new[] { DayOfWeek.Tuesday, DayOfWeek.Thursday },
                TimeSpan.FromHours(19),
                TimeSpan.FromMinutes(60)
            )
        };
        mikeStevens.AddAvailabilityRules(mikeStevensAvailabilityRules);

        Instructor hiroMatsuda = new Instructor()
        {
            Name = "Sensei Hiro Matsuda",
            Rank = "4th Dan",
            Bio = "Trained in Tokyo, 30 years teaching experience.",
            PhotoUrl = "https://placehold.co/300x300?text=Hiro",
        };
        List<string> hiroMatsudaAchievements = new List<string>()
        {
            "Trained at Kodokan Institute (Tokyo)",
            "30+ Years Traditional Judo Experience",
            "Former International Judo Federation Competitor",
            "Mentor to Over 40 Black Belts Worldwide"
        };
        hiroMatsuda.AddAchievements(hiroMatsudaAchievements);
        List<AvailabilityRule> hiroMatsudaAvailabilityRules = new List<AvailabilityRule>()
        {
            new AvailabilityRule
            (
                new[] { DayOfWeek.Wednesday, DayOfWeek.Saturday },
                TimeSpan.FromHours(19),
                TimeSpan.FromMinutes(60)
            )
        };
        hiroMatsuda.AddAvailabilityRules(hiroMatsudaAvailabilityRules);

        Instructor sarahLee = new Instructor()
        {
            Name = "Coach Sarah Lee",
            Rank = "Muay Thai Practitioner",
            Bio = "Known for technical precision and athletic conditioning.",
            PhotoUrl = "https://placehold.co/300x300?text=Sarah",
        };
        List<string> sarahLeeAchievements = new List<string>()
        {
            "Fought Professionally in Thailand (Chiang Mai Circuit)",
            "Certified Muay Thai Conditioning Coach",
            "Women's Division Regional Champion 2019",
            "Designed SMA's Kickboxing Conditioning Program"
        };
        sarahLee.AddAchievements(sarahLeeAchievements);
        List<AvailabilityRule> sarahLeeAvailabilityRules = new List<AvailabilityRule>()
        {
            new AvailabilityRule
            (
                new[] { DayOfWeek.Saturday },
                TimeSpan.FromHours(18),
                TimeSpan.FromMinutes(50)
            )
        };
        sarahLee.AddAvailabilityRules(sarahLeeAvailabilityRules);

        Instructor adrianBrooks = new Instructor()
        {
                Name = "Coach Adrian Brooks",
                Rank = "Brown Belt",
                Bio = "Competition-focused grappling specialist.",
                PhotoUrl = "https://placehold.co/300x300?text=Adrian",
        };
        List<string> adrianBrooksAchievements = new List<string>()
        {
            "NAGA Gold Medalist",
            "Active Submission-Only Competitor",
            "Expert in No-Gi Transitions & Leg Locks",
            "Assistant Coach for SMA Competition Team"
        };
        adrianBrooks.AddAchievements(adrianBrooksAchievements);
        List<AvailabilityRule> adrianBrooksAvailabilityRules = new List<AvailabilityRule>()
        {
            new AvailabilityRule
            (
                [DayOfWeek.Monday, DayOfWeek.Friday],
                TimeSpan.FromHours(17),
                TimeSpan.FromMinutes(60)
            )
        };
        adrianBrooks.AddAvailabilityRules(adrianBrooksAvailabilityRules);

        Instructor brianTurner = new Instructor()
        {
            Name = "Coach Brian Turner",
            Rank = "Black Belt",
            Bio = "National-level competitor with over 200 matches.",
            PhotoUrl = "https://placehold.co/300x300?text=Brian",
        };
        List<string> brianTurnerAchievements = new List<string>()
        {
            "Over 200 Competitive Matches",
            "National Grappling Silver Medalist",
            "Former Captain of SMA Competition Team",
            "Certified Referee for State-Level Tournaments"
        };
        brianTurner.AddAchievements(brianTurnerAchievements);
        List<AvailabilityRule> brianTurnerAvailabilityRules = new List<AvailabilityRule>()
        {
            new AvailabilityRule
            (
                [DayOfWeek.Tuesday, DayOfWeek.Friday],
                TimeSpan.FromHours(10),
                TimeSpan.FromMinutes(90)
            )
        };
        brianTurner.AddAvailabilityRules(brianTurnerAvailabilityRules);
        // --- Instructors ---
        List<Instructor> instructors = new List<Instructor>()
        {
            lauraKim, danielRuiz, mikeStevens, hiroMatsuda, sarahLee, adrianBrooks, brianTurner
        };

        context.Instructors.AddRange(instructors);

        // --- Programs ---
        var programs = new List<Domain.Program>
        {
            new Domain.Program
            {
                Name = "Kids Martial Arts",
                AgeGroup = "Ages 4–12",
                Description = "Fun, disciplined learning for young martial artists.",
                Details = "Our Kids Martial Arts program builds focus, confidence, and discipline while keeping classes fun and engaging. Students learn age-appropriate techniques, teamwork, and respect.",
                Duration = "45 minutes",
                Schedule = "Mon/Wed/Fri at 5:00 PM",
                ImageUrl = "https://placehold.co/800x400?text=Kids+Martial+Arts",
                Instructors = new List<Instructor> { instructors[0], instructors[1] }
            },
            new Domain.Program
            {
                Name = "American Jiu Jitsu",
                AgeGroup = "Teens & Adults",
                Description = "Strength, skill, and confidence through modern Jiu Jitsu.",
                Details = "American Jiu Jitsu combines traditional grappling with modern self-defense and sport techniques. Classes focus on practical skills, leverage, and confidence.",
                Duration = "60 minutes",
                Schedule = "Tue/Thu at 7:00 PM",
                ImageUrl = "https://placehold.co/800x400?text=American+Jiu+Jitsu",
                Instructors = new List<Instructor> { instructors[2] }
            },
            new Domain.Program
            {
                Name = "Judo",
                AgeGroup = "Teens & Adults",
                Description = "Powerful throws and real-world technique.",
                Details = "Traditional Judo focused on balance, timing, and powerful throws. Excellent for self-defense and improving athletic coordination.",
                Duration = "60 minutes",
                Schedule = "Mon/Wed at 7:00 PM",
                ImageUrl = "https://placehold.co/800x400?text=Judo",
                Instructors = new List<Instructor> { instructors[3] }
            },
            new Domain.Program
            {
                Name = "Kickboxing",
                AgeGroup = "Adults",
                Description = "High-energy striking and conditioning.",
                Details = "Combining fitness and striking, Kickboxing is perfect for anyone wanting to improve cardio, build confidence, and learn practical stand-up skills.",
                Duration = "50 minutes",
                Schedule = "Tue/Thu at 6:00 PM",
                ImageUrl = "https://placehold.co/800x400?text=Kickboxing",
                Instructors = new List<Instructor> { instructors[4] }
            },
            new Domain.Program
            {
                Name = "Submission Wrestling",
                AgeGroup = "Adults",
                Description = "Technical no-gi grappling for all levels.",
                Details = "A fast-paced grappling style emphasizing control, submissions, and positional strategy. Great for fitness or competitive goals.",
                Duration = "60 minutes",
                Schedule = "Fri at 7:00 PM",
                ImageUrl = "https://placehold.co/800x400?text=Submission+Wrestling",
                Instructors = new List<Instructor> { instructors[5] }
            },
            new Domain.Program
            {
                Name = "Competition Prep",
                AgeGroup = "Advanced Students",
                Description = "High-level training for tournaments.",
                Details = "Advanced training sessions designed to prepare serious martial artists for local and national competition. Includes strategy, conditioning, and live rounds.",
                Duration = "90 minutes",
                Schedule = "Sat at 10:00 AM",
                ImageUrl = "https://placehold.co/800x400?text=Competition+Prep",
                Instructors = new List<Instructor> { instructors[6] }
            }
        };

        context.Programs.AddRange(programs);
        context.SaveChanges();

        StudentSeeder.SeedStudents(context);
        EventStoreSeeder.SeedEventStore(context);
    }
}
