using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;

public class StudentFriendsMongoRepository : IStudentFriendsRepository
{
    private readonly IMongoRepository<StudentFriendsDocument, Guid> _repository;

    public StudentFriendsMongoRepository(IMongoRepository<StudentFriendsDocument, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<StudentFriends> GetAsync(Guid studentId)
    {
        var document = await _repository.GetAsync(studentId);
        return document?.AsEntity();
    }

    public async Task<IEnumerable<StudentFriends>> GetAllAsync()
    {
        var documents = await _repository.FindAsync(_ => true);
        return documents.Select(doc => doc.AsEntity());
    }

    public async Task AddAsync(StudentFriends studentFriends)
    {
        var document = studentFriends.AsDocument();
        await _repository.AddAsync(document);
    }

    public async Task UpdateAsync(StudentFriends studentFriends)
    {
        var document = studentFriends.AsDocument();
        await _repository.UpdateAsync(document);
    }

    public async Task DeleteAsync(Guid studentId)
    {
        await _repository.DeleteAsync(studentId);
    }

    public async Task<bool> ExistsAsync(Guid studentId)
    {
        var document = await _repository.GetAsync(studentId);
        return document != null;
    }

    public async Task<IEnumerable<Friend>> GetFriendsAsync(Guid studentId)
    {
        var document = await _repository.GetAsync(studentId);
        return document?.Friends.Select(f => f.AsEntity()) ?? Enumerable.Empty<Friend>();
    }

    public async Task AddOrUpdateAsync(StudentFriends studentFriends)
    {
        var exists = await ExistsAsync(studentFriends.StudentId);
        if (exists)
        {
            await UpdateAsync(studentFriends);
        }
        else
        {
            await AddAsync(studentFriends);
        }
    }
}
