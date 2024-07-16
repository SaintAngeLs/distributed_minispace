using MiniSpacePwa.HttpClients;

namespace MiniSpacePwa.Areas.Http
{
    public class ErrorMapperService : IErrorMapperService
    {
        public string MapError(ErrorMessage error)
        {
            switch (error.Code)
            {
                case "invalid_credentials":
                    return "Failed to sign in. Please check your credentials and try again.";
                case "email_in_use":
                    return $"{error.Reason} Please try again with a different email.";
                case "invalid_event_date_time":
                    return "Please review the selected dates. They should be in chronological order.";
                case "invalid_student_date_of_birth":
                    return "Please review the student's date of birth. It should be in the past.";
                default:
                    return "Something went wrong. Please try again later.";
            }
        }
    }
}
