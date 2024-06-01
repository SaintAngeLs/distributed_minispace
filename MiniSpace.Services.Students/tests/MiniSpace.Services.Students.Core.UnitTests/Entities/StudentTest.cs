using FluentAssertions;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MiniSpace.Services.Students.Core.UnitTests.Entities
{
    public class StudentTest
    {
        [Fact]
        public void Student_WrongName_ShouldThrowInvalidStudentFullNameException()
        {
            //Act & Assert
            Func<Student> fun = () => { return new Student(Guid.NewGuid(), "", "", "an@email.com", DateTime.Now);  };
            Assert.Throws<InvalidStudentFullNameException>(() => fun());
        }

        [Fact]
        public void CompleteRegistration_StudentIncomleated_ShouldMakeStudentValid()
        {
            // Arrange
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var description = "desc";
            var dOB = new DateTime(2000, 1, 1);

            // Act
            student.CompleteRegistration(Guid.NewGuid(), description, dOB, DateTime.Now, true);

            // Assert
            Assert.Equal(State.Valid, student.State);
        }

        [Fact]
        public void CompleteRegistration_StudentAlreadyCompleated_ShouldThrowCannotChangeStudentStateException()
        {
            // Arrange
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var description = "desc";
            var dOB = new DateTime(2000, 1, 1);
            student.SetValid();

            // Act
            Func<bool> fun = () => { student.CompleteRegistration(Guid.NewGuid(), description, dOB, DateTime.Now, true); return true; };

            // Assert
            Assert.Throws<CannotChangeStudentStateException>(() => fun());
        }

        [Fact]
        public void CompleteRegistration_DescriptionNullorWhite_ShouldThrowInvalidStudentDescriptionException()
        {
            // Arrange
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var description = "";
            var dOB = new DateTime(2000, 1, 1);

            // Act
            Func<bool> fun = () => { student.CompleteRegistration(Guid.NewGuid(), description, dOB, DateTime.Now, true); return true; };

            // Assert
            Assert.Throws<InvalidStudentDescriptionException>(() => fun());
        }

        [Fact]
        public void CompleteRegistration_DateOfBirthWrong_ShouldThrowInvalidStudentDateOfBirthException()
        {
            // Arrange
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var description = "text";
            var dOB = new DateTime(3000, 1, 1);

            // Act
            Func<bool> fun = () => { student.CompleteRegistration(Guid.NewGuid(), description, dOB, DateTime.Now, true); return true; };

            // Assert
            Assert.Throws<InvalidStudentDateOfBirthException>(() => fun());
        }

        [Fact]
        public void Update_StudentIsNotVaild_ShouldThrowCannotUpdateStudentException()
        {
            // Arrange
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var description = "text";

            // Act
            Func<bool> fun = () => { student.Update(Guid.NewGuid(), description, true); return true; };

            // Assert
            Assert.Throws<CannotUpdateStudentException>(() => fun());
        }

        [Fact]
        public void Update_DescriptionNullorWhite_ShouldThrowInvalidStudentDescriptionException()
        {
            // Arrange
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            var description = "";
            student.SetBanned();
            student.SetIncomplete();
            student.SetValid();

            // Act
            Func<bool> fun = () => { student.Update(Guid.NewGuid(), description, true); return true; };

            // Assert
            Assert.Throws<InvalidStudentDescriptionException>(() => fun());
        }

        [Fact]
        public void AddInterestedInEvent_GivenEmptyGuid_ShouldNotAddItAsEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            student.InterestedInEvents = new List<Guid> { eventId };

            // Act
            student.AddInterestedInEvent(Guid.Empty);

            // Assert
            Assert.Equal(student.InterestedInEvents, new List<Guid> { eventId });
        }

        [Fact]
        public void AddInterestedInEvent_AlreadyAdded_ShouldThrowEventAlreadyAddedExeption()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            student.InterestedInEvents = new List<Guid> { eventId };

            // Act
            Func<bool> fun = () => { student.AddInterestedInEvent(eventId); return true; };

            // Assert
            Assert.ThrowsAny<DomainException>(() => fun());
        }

        [Fact]
        public void RemoveInterestedInEvent_AlreadyRemoved_ShouldThrowEventAlreadyRemovedExeption()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);

            // Act
            Func<bool> fun = () => { student.RemoveInterestedInEvent(eventId); return true; };

            // Assert
            Assert.ThrowsAny<DomainException>(() => fun());
        }

        [Fact]
        public void AddSignedUpEvent_GivenEmptyGuid_ShouldNotAddItAsEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            student.SignedUpEvents = new List<Guid> { eventId };

            // Act
            student.AddSignedUpEvent(Guid.Empty);

            // Assert
            Assert.Equal(student.SignedUpEvents, new List<Guid> { eventId });
        }

        [Fact]
        public void AddSignedUpEvent_AlreadyAdded_ShouldThrowStudentAlreadySignedUpException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            student.SignedUpEvents = new List<Guid> { eventId };

            // Act
            Func<bool> fun = () => { student.AddSignedUpEvent(eventId); return true; };

            // Assert
            Assert.Throws<StudentAlreadySignedUpException>(() => fun());
        }

        [Fact]
        public void RemoveSignedUpEvent_AlreadyRemoved_ShouldThrowStudentIsNotSignedUpException()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);

            // Act
            Func<bool> fun = () => { student.RemoveSignedUpEvent(eventId); return true; };

            // Assert
            Assert.Throws<StudentIsNotSignedUpException>(() => fun());
        }
        [Fact]
        public void RemoveProfileImage_CorrectMediaFileId_ShouldSetMediaImageEmpty()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var imageId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            student.SetValid();
            student.Update(imageId, "text", true);
            // Act
            student.RemoveProfileImage(imageId);

            // Assert
            Assert.Equal(student.ProfileImage, Guid.Empty);
        }

        [Fact]
        public void RemoveProfileImage_IncorrectMediaFileId_ShouldNotDeleteImage()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var imageId = Guid.NewGuid(); 
            var newImageId = Guid.NewGuid();
            var student = new Student(Guid.NewGuid(), "Adam", "Nowak", "an@email.com", DateTime.Now);
            student.SetValid();
            student.Update(imageId, "text", true);
            // Act
            student.RemoveProfileImage(newImageId);

            // Assert
            Assert.Equal(student.ProfileImage, imageId);
        }
    }
}
