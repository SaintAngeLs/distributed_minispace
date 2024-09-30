using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MiniSpace.Services.Identity.Application.Events;
using MiniSpace.Services.Identity.Application.Exceptions;
using MiniSpace.Services.Identity.Application.Services;
using MiniSpace.Services.Identity.Core.Entities;
using MiniSpace.Services.Identity.Core.Repositories;
using MiniSpace.Services.Identity.Application.Commands.Handlers;
using MiniSpace.Services.Identity.Application.Commands;
using MiniSpace.Services.Identity.Infrastructure.Contexts;
using Paralax.CQRS.Commands;
using System.Threading;
using System.Security.Claims;
using FluentAssertions;
using MiniSpace.Services.Identity.Core.Exceptions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MiniSpace.Services.Identity.Core.UnitTests.Entities
{
    public class UserTest
    {
        [Fact]
        public void User_Create_ShouldBeCorrect() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "USER", date, null);
            };
        
            createUser.Should().NotThrow();
            
            var user = createUser();
            Assert.True(user.CreatedAt == date &&
                user.Email == "alan.turing@gmail.com" &&
                user.Id == id &&
                user.Name == "Alan Turing" &&
                user.Password == "Secret" &&
                !user.Permissions.Any() &&
                user.Role == "user");
        }

        [Fact]
        public void User_CreateWithNameSpacesOnly_ShouldThrowInvalidNameException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "  \t\t\t   ", "Alan.Turing@GMAIL.COM",
                                                "Secret", "USER", date, null);
            };
        
            Assert.Throws<InvalidNameException>(createUser);
        }

        [Fact]
        public void User_CreateWithNullName_ShouldThrowInvalidNameException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, null, "Alan.Turing@GMAIL.COM",
                                                "Secret", "USER", date, null);
            };
        
            Assert.Throws<InvalidNameException>(createUser);
        }

        [Fact]
        public void User_CreateWithEmailSpacesOnly_ShouldThrowInvalidEmailException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "  \t\t\t   ",
                                                "Secret", "USER", date, null);
            };
        
            Assert.Throws<InvalidEmailException>(createUser);
        }

        [Fact]
        public void User_CreateWithNullEmail_ShouldThrowInvalidEmailException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", null,
                                                "Secret", "USER", date, null);
            };
        
            Assert.Throws<InvalidEmailException>(createUser);
        }

        [Fact]
        public void User_CreateWithPasswordSpacesOnly_ShouldThrowInvalidPasswordException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "  \t\t\t   ", "USER", date, null);
            };
        
            Assert.Throws<InvalidPasswordException>(createUser);
        }

        [Fact]
        public void User_CreateWithNullPassword_ShouldThrowInvalidPasswordException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                null, "USER", date, null);
            };
        
            Assert.Throws<InvalidPasswordException>(createUser);
        }

        [Fact]
        public void User_CreateWithInvalidRole_ShouldThrowInvalidRoleException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "tghguyjhyjh", date, null);
            };
        
            Assert.Throws<InvalidRoleException>(createUser);
        }

        [Fact]
        public void User_CreateWithPermissions_ShouldBeCorrect() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "USER", date, ["permission", "yet another permission"]);
            };
        
            createUser.Should().NotThrow();
            
            var user = createUser();
            Assert.True(user.CreatedAt == date &&
                user.Email == "alan.turing@gmail.com" &&
                user.Id == id &&
                user.Name == "Alan Turing" &&
                user.Password == "Secret" &&
                user.Permissions.Count() == 2 &&
                user.Role == "user");
        }

        [Fact]
        public void GrantOrganizerRights_WithCorrectParameters_ShouldNotThrowException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "USER", date, ["permission", "yet another permission"]);
            };
        
            createUser.Should().NotThrow();

            var user = createUser();
            var act = user.GrantOrganizerRights;
            act.Should().NotThrow();

            Assert.True(user.Role == Role.Organizer);
        }

        [Fact]
        public void GrantOrganizerRights_WithInvalidRole_ShouldThrowUserCannotBecomeAnOrganizerException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "BANNED", date, ["permission", "yet another permission"]);
            };
        
            createUser.Should().NotThrow();

            var user = createUser();
            var act = user.GrantOrganizerRights;
            Assert.Throws<UserCannotBecomeAnOrganizerException>(act);
        }

        [Fact]
        public void RevokeOrganizerRights_WithCorrectParameters_ShouldNotThrowException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "ORGANIZER", date, ["permission", "yet another permission"]);
            };
        
            createUser.Should().NotThrow();

            var user = createUser();
            var act = user.RevokeOrganizerRights;
            act.Should().NotThrow();

            Assert.True(user.Role == Role.User);
        }

        [Fact]
        public void RevokeOrganizerRights_WithInvalidRole_ShouldThrowUserIsNotAnOrganizerException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "USER", date, ["permission", "yet another permission"]);
            };
        
            createUser.Should().NotThrow();

            var user = createUser();
            var act = user.RevokeOrganizerRights;
            Assert.Throws<UserIsNotAnOrganizerException>(act);
        }

        [Fact]
        public void Ban_WithCorrectParameters_ShouldNotThrowException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "USER", date, ["permission", "yet another permission"]);
            };
        
            createUser.Should().NotThrow();

            var user = createUser();
            var act = user.Ban;
            act.Should().NotThrow();

            Assert.True(user.Role == Role.Banned);
        }

        [Fact]
        public void Ban_WithAdminRole_ShouldThrowUserCannotBeBannedException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "ADMIN", date, ["permission", "yet another permission"]);
            };
        
            createUser.Should().NotThrow();

            var user = createUser();
            var act = user.Ban;
            Assert.Throws<UserCannotBeBannedException>(act);
        }

        [Fact]
        public void Ban_WithBannedRole_ShouldThrowUserCannotBeBannedException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "BANNED", date, ["permission", "yet another permission"]);
            };
        
            createUser.Should().NotThrow();

            var user = createUser();
            var act = user.Ban;
            Assert.Throws<UserCannotBeBannedException>(act);
        }

        [Fact]
        public void Unban_WithCorrectParameters_ShouldNotThrowException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "BANNED", date, ["permission", "yet another permission"]);
            };
        
            createUser.Should().NotThrow();

            var user = createUser();
            var act = user.Unban;
            act.Should().NotThrow();

            Assert.True(user.Role == Role.User);
        }

        [Fact]
        public void Unban_WithInvalidParameters_ShouldThrowUserIsNotBannedException() {
            var date = DateTime.Today;
            var id = Guid.NewGuid();
            var createUser = () => { return new User(id, "Alan Turing", "Alan.Turing@GMAIL.COM",
                                                "Secret", "USER", date, ["permission", "yet another permission"]);
            };
        
            createUser.Should().NotThrow();

            var user = createUser();
            var act = user.Unban;
            Assert.Throws<UserIsNotBannedException>(act);
        }

    }
}