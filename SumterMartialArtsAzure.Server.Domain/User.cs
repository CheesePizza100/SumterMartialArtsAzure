using SumterMartialArtsAzure.Server.Domain.Common;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Domain.Services;

namespace SumterMartialArtsAzure.Server.Domain;

public class User : Entity
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public bool MustChangePassword { get; private set; }

    public int? StudentId { get; private set; }
    public int? InstructorId { get; private set; }

    public Student? Student { get; private set; }
    public Instructor? Instructor { get; private set; }

    private User() { }

    // Factory methods take the HASH, not the password
    public static User CreateAdmin(string username, string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            MustChangePassword = false,
            Role = UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static User CreateForStudent(string username, string email, string passwordHash, int studentId, string studentName, string temporaryPassword)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            MustChangePassword = true,
            Role = UserRole.Student,
            StudentId = studentId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        user.AddDomainEvent(new StudentLoginCreated
        {
            StudentId = studentId,
            StudentName = studentName,
            StudentEmail = email,
            UserName = username,
            TemporaryPassword = temporaryPassword,
            CreatedAt = DateTime.UtcNow
        });

        return user;
    }

    public static User CreateForInstructor(
        string username,
        string email,
        string passwordHash,
        int instructorId,
        string instructorName,
        string temporaryPassword)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            Role = UserRole.Instructor,
            InstructorId = instructorId,
            IsActive = true,
            MustChangePassword = true,
            CreatedAt = DateTime.UtcNow
        };

        user.AddDomainEvent(new InstructorLoginCreated
        {
            InstructorId = instructorId,
            InstructorName = instructorName,
            InstructorEmail = email,
            Username = username,
            TemporaryPassword = temporaryPassword,
        });

        return user;
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    // Just verify - don't know HOW (strategy pattern)
    public bool VerifyPassword(string plainTextPassword, IPasswordHasher passwordHasher)
    {
        return passwordHasher.Verify(plainTextPassword, PasswordHash);
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        MustChangePassword = false;
    }
}

public enum UserRole
{
    Admin = 1,
    Student = 2,
    Instructor = 3
}