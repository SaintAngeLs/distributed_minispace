using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

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
        // Using a LINQ expression instead of a MongoDB filter
        var documents = await _repository.FindAsync(doc => doc.StudentId == studentId);
        if (documents == null || !documents.Any())
        {
            Console.WriteLine($"No document found for student ID: {studentId}");
            return Enumerable.Empty<Friend>();
        }

        var document = documents.First();  // Assuming you expect only one document per studentId or taking the first one
        Console.WriteLine($"Document found: {document.StudentId}, Friends Count: {document.Friends.Count}");
        return document.Friends.Select(doc => new Friend(
            doc.StudentId,
            doc.FriendId,
            doc.CreatedAt,
            doc.State)).ToList();
    }
    


    public async Task AddOrUpdateAsync(StudentFriends studentFriends)
{
    // Ensuring that the document ID (MongoDB _id) is explicitly set to StudentId
    var filter = Builders<StudentFriendsDocument>.Filter.Eq(doc => doc.StudentId, studentFriends.StudentId);
    var update = Builders<StudentFriendsDocument>.Update
        .SetOnInsert(doc => doc.StudentId, studentFriends.StudentId) // Ensuring the document _id is set to StudentId on insert
        .Set(doc => doc.Id, studentFriends.StudentId) // Setting the document _id field explicitly
        .AddToSetEach(doc => doc.Friends, studentFriends.Friends.Select(f => f.AsDocument())); // Use AddToSetEach to append new items to the list

    var options = new UpdateOptions { IsUpsert = true };
    var result = await _repository.Collection.UpdateOneAsync(filter, update, options);

    Console.WriteLine("********************************************************");
    // Check if the document was actually inserted or updated
    if (result.ModifiedCount > 0 || result.UpsertedId != null)
    {
        // Retrieve the updated or inserted document
        var updatedDocument = await _repository.GetAsync(studentFriends.StudentId);
        if (updatedDocument != null)
        {
            // Serialize the updated document to JSON and log it
            var json = JsonSerializer.Serialize(updatedDocument, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Updated StudentFriends document:");
            Console.WriteLine(json);
        }
        else
        {
            Console.WriteLine("Failed to retrieve the updated document.");
        }
    }
    else
    {
        Console.WriteLine("No changes were made to the document.");
    }
}

public async Task RemoveFriendAsync(Guid studentId, Guid friendId)
{
    var filter = Builders<StudentFriendsDocument>.Filter.Eq(doc => doc.StudentId, studentId);
    var update = Builders<StudentFriendsDocument>.Update.PullFilter(doc => doc.Friends, Builders<FriendDocument>.Filter.Eq("FriendId", friendId));

    var result = await _repository.Collection.UpdateOneAsync(filter, update);

    if (result.ModifiedCount == 0)
    {
        Console.WriteLine($"No friend removed for Student ID: {studentId} with Friend ID: {friendId}");
    }
    else
    {
        Console.WriteLine($"Friend ID: {friendId} removed from Student ID: {studentId}'s friends list.");
    }
}




}
