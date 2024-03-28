namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidStudentDateOfBirthException : DomainException
    {
        public override string Code { get; } = "invalid_student_date_of_birth";
        public DateTime DateOfBirth { get; }
        public DateTime Now { get; }

        public InvalidStudentDateOfBirthException(DateTime dateOfBirth, DateTime now) : base(
            $"Invalid student date of birth. Now: '{now}' must be greater than date of birth: '{dateOfBirth}'.")
        {
            DateOfBirth = dateOfBirth;
            Now = now;
        }
    }
}
