using MediatR;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;
using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.CreateStudent;

public class CreateStudentHandler
    : IRequestHandler<CreateStudentCommand, GetStudentByIdResponse>
{
    private readonly AppDbContext _dbContext;

    public CreateStudentHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetStudentByIdResponse> Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        // Use aggregate factory method
        var student = Student.Create(
            name: request.Name,
            email: request.Email,
            phone: request.Phone
        );

        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Return created student
        return new GetStudentByIdResponse(
            student.Id,
            student.Name,
            student.Email,
            student.Phone,
            new List<ProgramEnrollmentDto>(),
            new AttendanceDto(0, 0, 0),
            new List<TestHistoryDto>()
        );
    }
}