namespace MiniSpace.Services.Posts.Core.Entities
{
    public class Student
    {
        public Guid Id { get; private set; }

        public Student(Guid id)
        {
            Id = id;
        }
    }
}
