using System;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IReadUserRepository
    {
        Task<Student> GetAsync(Guid id);
    }
}
