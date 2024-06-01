using FluentAssertions;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Core.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MiniSpace.Services.Events.Core.UnitTests.Entities
{
    public class EventTest
    {
        [Fact]
        public void Update_ShoudUpdate()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var oldevent = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var newevent = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);

            // Act
            var newadress = new Address("new bname", "new street", "2A", "2", "new city", "10-000");
            newevent.Update("new name", "new text", new DateTime(2025, 1, 2),
                new DateTime(2025, 2, 2), newadress, 20, 20,
                Category.Business, State.Published, new DateTime(2024, 10, 2), DateTime.Now);

            // Assert
            Assert.NotEqual(oldevent.Name, newevent.Name);
            Assert.NotEqual(oldevent.Description, newevent.Description);
        }

        [Fact]
        public void SignUpStudent_StateNotPublished_ShoudThrowInvalidEventState()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var participant = new Participant(studentId, "Adam");

            // Act
            Func<bool> act = () => { @event.SignUpStudent(participant); return true; };
            // Assert
            act.Should().Throw<InvalidEventState>();
        }
        [Fact]
        public void AddParticipant_StudentAlreadySigned_ShoudThrowStudentAlreadySignedUpException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var participant = new Participant(studentId, "Adam");
            @event.AddParticipant(participant);

            // Act
            Func<bool> act = () => { @event.AddParticipant(participant); return true; };
            // Assert
            act.Should().Throw<StudentAlreadySignedUpException>();
        }
        [Fact]
        public void AddParticipant_StudentLimitMet_ShoudThrowEventCapacityExceededException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 0, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var participant = new Participant(studentId, "Adam");


            // Act
            Func<bool> act = () => { @event.AddParticipant(participant); return true; };
            // Assert
            act.Should().Throw<EventCapacityExceededException>();
        }
        [Fact]
        public void AddParticipant_OrganizerSignsUp_ShoudThrowOrganizerCannotSignUpForOwnEventException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = studentId;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var participant = new Participant(studentId, "Adam");

            // Act
            Func<bool> act = () => { @event.AddParticipant(participant); return true; };
            // Assert
            act.Should().Throw<OrganizerCannotSignUpForOwnEventException>();
        }
        [Fact]
        public void CancelSighnUp_EventNotPublished_ShoudThrowInvalidEventState()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = studentId;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);

            // Act
            Func<bool> act = () => { @event.CancelSignUp(studentId); return true; };
            // Assert
            act.Should().Throw<InvalidEventState>();
        }
        [Fact]
        public void RemoveParticipant_StudentNotSighnedUp_ShoudThrowStudentNotSignedUpException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = studentId;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);

            // Act
            Func<bool> act = () => { @event.RemoveParticipant(studentId); return true; };
            // Assert
            act.Should().Throw<StudentNotSignedUpException>();
        }
        [Fact]
        public void ShowStudentInterest_StudentAlreadyInterested_ShoudThrowStudentAlreadyInterestedInEventException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = studentId;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var participant = new Participant(studentId, "Adam");
            @event.ShowStudentInterest(participant);

            // Act
            Func<bool> act = () => { @event.ShowStudentInterest(participant); return true; };
            // Assert
            act.Should().Throw<StudentAlreadyInterestedInEventException>();
        }
        [Fact]
        public void CancelInterest_StudentNotInterested_ShoudThrowStudentNotInterestedInEventException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = studentId;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);

            // Act
            Func<bool> act = () => { @event.CancelInterest(studentId); return true; };
            // Assert
            act.Should().Throw<StudentNotInterestedInEventException>();
        }
        [Fact]
        public void Rate_EventNotArchived_ShoudThrowInvalidEventState()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = studentId;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);

            // Act
            Func<bool> act = () => { @event.Rate(studentId, 1); return true; };
            // Assert
            act.Should().Throw<InvalidEventState>();
        }
        [Fact]
        public void Rate_StudentNotSighnedUp_ShoudThrowStudentNotSignedUpForEventException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = studentId;

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.Archived, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);

            // Act
            Func<bool> act = () => { @event.Rate(studentId, 1); return true; };
            // Assert
            act.Should().Throw<StudentNotSignedUpForEventException>();
        }
        [Fact]
        public void Rate_RateOutOfRange_ShoudThrowInvalidRatingValueException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.Archived, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var participant = new Participant(studentId, "Adam");
            @event.AddParticipant(participant);
            // Act
            Func<bool> act = () => { @event.Rate(studentId, 10); return true; };
            // Assert
            act.Should().Throw<InvalidRatingValueException>();
        }
        [Fact]
        public void CancelRate_StudentDidntRate_ShoudThrowStudentNotRatedEventException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var participant = new Participant(studentId, "Adam");

            // Act
            Func<bool> act = () => { @event.CancelRate(studentId); return true; };
            // Assert
            act.Should().Throw<StudentNotRatedEventException>();
        }
        [Fact]
        public void UpdateState_AfterPublishDate_ShoudChangeToPublished()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var participant = new Participant(studentId, "Adam");

            // Act
            @event.UpdateState(new DateTime(2024, 10, 2));

            // Assert
            Assert.Equal(State.Published, @event.State);
        }

        [Fact]
        public void RemoveMediaFile_AfterPublishDate_ShoudChangeToPublished()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var organizerId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();

            var adress = new Address("bname", "street", "1A", "1", "city", "00-000");
            var organizer = new Organizer(organizerId, "Bart", "bartek@mail.com", Guid.NewGuid(), "oname");
            var @event = Event.Create(eventId, "name", "text", new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1), adress, new List<Guid> { }, 10, 10,
                Category.Music, State.ToBePublished, new DateTime(2024, 10, 1),
                organizer, DateTime.Now);
            var participant = new Participant(studentId, "Adam");
            var organizerTru = @event.IsOrganizer(organizerId);

            // Act
            Func<bool> act = () => { @event.RemoveMediaFile(mediaFileId); return true; };

            // Assert
            act.Should().Throw<MediaFileNotFoundException>();
        }
    }
}
