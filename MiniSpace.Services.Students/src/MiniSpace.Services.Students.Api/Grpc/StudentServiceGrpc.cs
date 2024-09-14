using Grpc.Core;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Application.Dto;
using Convey.CQRS.Queries;
using System;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Grpc;

namespace MiniSpace.Services.Students.Api.Grpc
{
    public class StudentServiceGrpc : StudentService.StudentServiceBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public StudentServiceGrpc(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        // Implement the GetStudent gRPC method
        public override async Task<StudentResponse> GetStudent(GetStudentRequest request, ServerCallContext context)
        {
            var studentDto = await _queryDispatcher.QueryAsync<GetStudent, StudentDto>(new GetStudent
            {
                StudentId = Guid.Parse(request.StudentId)
            });

            if (studentDto == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Student not found"));
            }

            return new StudentResponse
            {
                StudentId = studentDto.Id.ToString(),
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                ProfileImageUrl = studentDto.ProfileImageUrl,
                Description = studentDto.Description
            };
        }

        // Implement the GetPaginatedStudents gRPC method
        public override async Task<GetPaginatedStudentsResponse> GetPaginatedStudents(GetPaginatedStudentsRequest request, ServerCallContext context)
        {
            // Fetch paginated students using the dispatcher
            var paginatedStudents = await _queryDispatcher.QueryAsync<GetStudents, MiniSpace.Services.Students.Application.Queries.PagedResult<StudentDto>>(new GetStudents
            {
                Page = request.Page,
                ResultsPerPage = request.PageSize // Use ResultsPerPage instead of PageSize
            });

            if (paginatedStudents == null || !paginatedStudents.Results.Any())
            {
                return new GetPaginatedStudentsResponse
                {
                    TotalCount = 0
                };
            }

            // Create the response with pagination details
            var response = new GetPaginatedStudentsResponse
            {
                TotalCount = paginatedStudents.Total,
                PageSize = paginatedStudents.PageSize,
                CurrentPage = paginatedStudents.Page,
                NextPageUrl = paginatedStudents.NextPage ?? string.Empty,
                PrevPageUrl = paginatedStudents.PrevPage ?? string.Empty
            };

            // Map students to the response
            response.Students.AddRange(paginatedStudents.Results.Select(s => new StudentResponse
            {
                StudentId = s.Id.ToString(),
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                ProfileImageUrl = s.ProfileImageUrl,
                Description = s.Description
            }));

            return response;
        }
    }
}
