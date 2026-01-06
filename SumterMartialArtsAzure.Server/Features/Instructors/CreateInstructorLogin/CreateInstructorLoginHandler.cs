using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.CreateInstructorLogin;

public class CreateInstructorLoginHandler
    : IRequestHandler<CreateInstructorLoginCommand, CreateInstructorLoginResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public CreateInstructorLoginHandler(
        AppDbContext dbContext,
        IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<CreateInstructorLoginResponse> Handle(
        CreateInstructorLoginCommand request,
        CancellationToken cancellationToken)
    {
        // Check if instructor exists
        var instructor = await _dbContext.Instructors
            .FirstOrDefaultAsync(i => i.Id == request.InstructorId, cancellationToken);

        if (instructor == null)
            throw new InvalidOperationException("Instructor not found");

        // Check if instructor already has a login
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.InstructorId == request.InstructorId, cancellationToken);

        if (existingUser != null)
            throw new InvalidOperationException("Instructor already has a login account");

        // Check if username is already taken
        var usernameTaken = await _dbContext.Users
            .AnyAsync(u => u.Username == request.Username, cancellationToken);

        if (usernameTaken)
            throw new InvalidOperationException("Username is already taken");

        var tempPassword = string.IsNullOrWhiteSpace(request.Password)
            ? GenerateTemporaryPassword()
            : request.Password;

        var passwordHash = _passwordHasher.Hash(tempPassword);

        var user = User.CreateForInstructor(
            request.Username,
            instructor.Email,
            passwordHash,
            instructor.Id,
            instructor.Name,
            tempPassword
        );

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateInstructorLoginResponse(
            true,
            "Instructor login created successfully",
            request.Username,
            tempPassword,
            user.Id.ToString()
        );
    }

    private static string GenerateTemporaryPassword()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789!@#$%";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}