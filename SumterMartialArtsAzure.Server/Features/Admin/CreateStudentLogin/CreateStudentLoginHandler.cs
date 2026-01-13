using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.CreateStudentLogin;

public class CreateStudentLoginHandler
    : IRequestHandler<CreateStudentLoginCommand, CreateStudentLoginCommandResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public CreateStudentLoginHandler(
        AppDbContext dbContext,
        IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<CreateStudentLoginCommandResponse> Handle(
        CreateStudentLoginCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
            throw new InvalidOperationException("Student not found");

        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.StudentId == request.StudentId, cancellationToken);

        if (existingUser != null)
            throw new InvalidOperationException("Student already has a login account");

        var usernameTaken = await _dbContext.Users
            .AnyAsync(u => u.Username == request.Username, cancellationToken);

        if (usernameTaken)
            throw new InvalidOperationException("UserName is already taken");

        // Generate password if not provided or empty
        var tempPassword = string.IsNullOrWhiteSpace(request.Password)
            ? GenerateTemporaryPassword()
            : request.Password;

        var passwordHash = _passwordHasher.Hash(tempPassword);

        var user = User.CreateForStudent(
            request.Username,
            student.Email,
            passwordHash,
            student.Id,
            student.Name,
            tempPassword
        );

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateStudentLoginCommandResponse(
            true,
            "Student login created successfully",
            request.Username,
            tempPassword,
            user.Id.ToString()
        );
    }
    private static string GenerateTemporaryPassword()
    {
        // Generate a random 12-character password
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789!@#$%";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}