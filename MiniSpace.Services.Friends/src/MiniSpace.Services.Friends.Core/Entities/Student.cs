namespace MiniSpace.Services.Friends.Core.Entities
{
    public class Student
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; }

        public Student(Guid id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }
    }
}