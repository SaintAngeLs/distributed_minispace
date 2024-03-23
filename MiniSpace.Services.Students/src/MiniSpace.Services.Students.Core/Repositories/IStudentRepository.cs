using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Core.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetAsync(Guid id);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
    }
}
