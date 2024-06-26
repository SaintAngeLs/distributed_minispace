﻿using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Core.Repositories;
using MiniSpace.Services.Reports.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Repositories
{
    public class StudentMongoRepository : IStudentRepository
    {
        private readonly IMongoRepository<StudentDocument, Guid> _repository;

        public StudentMongoRepository(IMongoRepository<StudentDocument, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<Student> GetAsync(Guid id)
        {
            var student = await _repository.GetAsync(s => s.Id == id);

            return student?.AsEntity();
        }
        public Task<bool> ExistsAsync(Guid id) => _repository.ExistsAsync(s => s.Id == id);
        public Task AddAsync(Student student) => _repository.AddAsync(student.AsDocument());
    }
}