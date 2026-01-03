using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api;

public static class UserSeeder
{
    public static void SeedAdminUser(AppDbContext context)
    {
        var passwordHasher = new BcryptPasswordHasher();
        var passwordHash = passwordHasher.Hash("Admin123!");
        var admin = User.CreateAdmin("admin", "admin@sumtermartialarts.com", passwordHash);

        context.Users.Add(admin);
        context.SaveChanges();

        Console.WriteLine("Seeded admin user - Username: admin, Password: Admin123!");
    }
}