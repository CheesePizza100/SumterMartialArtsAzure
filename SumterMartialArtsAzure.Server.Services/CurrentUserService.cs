using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SumterMartialArtsAzure.Server.Domain;
using Guid = System.Guid;

namespace SumterMartialArtsAzure.Server.Services;

public interface ICurrentUserService
{
    Guid GetUserId();
    Guid GetSessionId();
    string? GetUsername();
    UserRole? GetRole();
    bool IsAuthenticated();
    bool IsAdmin();
    bool IsStudent();
    bool IsInstructor();
    int? GetStudentId();
    int? GetInstructorId();
}
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }

    public Guid GetSessionId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("SessionId")?.Value;
        return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }

    public string? GetUsername()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public UserRole? GetRole()
    {
        var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
        return Enum.TryParse<UserRole>(roleClaim, out var role) ? role : null;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }

    public bool IsAdmin()
    {
        return GetRole() == UserRole.Admin;
    }

    public bool IsStudent()
    {
        return GetRole() == UserRole.Student;
    }

    public bool IsInstructor()
    {
        return GetRole() == UserRole.Instructor;
    }

    public int? GetStudentId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("StudentId")?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }

    public int? GetInstructorId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("InstructorId")?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }
}
