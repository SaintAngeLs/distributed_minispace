namespace MiniSpace.Services.Email.Core.Entities
{
    public class EventNewSignUpSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "New sign-up for your event!";
    }

    public class EventNewSignUpFriendSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "Your friend has signed up for an event!";
    }

    public class StudentCancelledSignedUpToEventSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "A participant has cancelled their event sign-up.";
    }

    public class StudentShowedInterestInEventSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "Someone showed interest in your event!";
    }

    public class StudentCancelledInterestInEventSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "A participant has withdrawn interest in an event.";
    }

    public class EventParticipantAddedSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "A new participant has been added to your event.";
    }

    public class EventParticipantRemovedSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "A participant has been removed from your event.";
    }

    public class PostCreatedSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "Check out new posts in your feed!";
    }

    public class MentionedInPostSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "You've been mentioned in a post!";
    }

    public class EventReminderSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "Reminder for your upcoming event!";
    }

    public class OtherSubject : IEmailSubjectStrategy
    {
        public string GenerateSubject(string details) => "Notification: " + details;
    }

}