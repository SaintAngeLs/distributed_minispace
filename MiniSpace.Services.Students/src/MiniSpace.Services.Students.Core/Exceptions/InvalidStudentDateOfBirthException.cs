namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidStudentDateOfBirthException : DomainException
    {
        public override string Code { get; } = "invalid_student_date_of_birth";
        public Guid Id;
        public DateTime DateOfBirth { get; }
        public DateTime Now { get; }

        public InvalidStudentDateOfBirthException(Guid id, DateTime dateOfBirth, DateTime now) : base(
            $"Student with id: {id} has invalid date of birth. Today date: " +
            $"'{now}' must be greater than date of birth: '{dateOfBirth}'.")
        {
            Id = id;
            DateOfBirth = dateOfBirth;
            Now = now;
        }
    }
}
