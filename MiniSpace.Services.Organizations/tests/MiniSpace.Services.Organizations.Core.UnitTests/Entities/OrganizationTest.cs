using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MiniSpace.Services.Organizations.Core.UnitTests.Entities
{
    public class OrganizationTest
    {
        [Fact]
        public void AddOrganizer_OrganizorAdded_ShouldAddOrgizer()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            var organiazation = new Organization(Guid.NewGuid(), "name");

            // Act
            organiazation.AddOrganizer(organizerId);

            // Assert
            Assert.True(organiazation.Organizers.Any(x => x.Id == organizerId));
        }

        [Fact]
        public void AddOrganizer_OrganizorOlreadyAdded_ShouldThrowOrganizerAlreadyAddedToOrganizationException()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            var organiazation = new Organization(Guid.NewGuid(), "name");
            organiazation.AddOrganizer(organizerId);

            // Act & Assert
            var act = new Action(() => organiazation.AddOrganizer(organizerId));
            Assert.Throws<OrganizerAlreadyAddedToOrganizationException>(act); 
        }

        [Fact]
        public void RemoveOrganizer_OrganizorRemoved_ShouldRemoveOrganizer()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            var organiazation = new Organization(Guid.NewGuid(), "name");
            organiazation.AddOrganizer(organizerId);

            // Act
            organiazation.RemoveOrganizer(organizerId);

            // Assert
            Assert.DoesNotContain<Organizer>(new Organizer(organizerId), organiazation.Organizers);
        }

        [Fact]
        public void RemoveOrganizer_OrganizorOlreadyRemoved_ShouldThrowOrganizerIsNotInOrganization()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            var organiazation = new Organization(Guid.NewGuid(), "name");

            // Act & Assert
            var act = new Action(() => organiazation.RemoveOrganizer(organizerId));
            Assert.Throws<OrganizerIsNotInOrganization>(act);
        }

        [Fact]
        public void AddSubOrganization_AddOrganization_ShouldAddOrganization()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var organiazation = new Organization(organizationId, "name");
            var neworganization = new Organization(Guid.NewGuid(), "name");

            // Act
            organiazation.AddSubOrganization(neworganization);

            // Assert
            Assert.Contains(neworganization, organiazation.SubOrganizations);

        }


        [Fact]
        public void GetSubOrganization_AskedAboutThemself_ShouldReturnThemself()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var organiazation = new Organization(organizationId, "name");

            // Act
            var result = organiazation.GetSubOrganization(organizationId);

            // Assert
            Assert.Equal(result.Id.ToString(), organizationId.ToString());

        }

        [Fact]
        public void GetSubOrganization_DorsNotContainsOrganization_ShouldReturnNull()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var organiazation = new Organization(Guid.NewGuid(), "name");

            // Act
            var result = organiazation.GetSubOrganization(organizationId);

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public void GetSubOrganization_ContainsOrganization_ShouldReturnOrganization()
        {
            // Arrange
            var organizationId = Guid.NewGuid();
            var organiazation = new Organization(Guid.NewGuid(), "name");
            organiazation.AddSubOrganization(new Organization(organizationId, "name"));

            // Act
            var result = organiazation.GetSubOrganization(organizationId);

            // Assert
            Assert.Equal(result.Id.ToString(), organizationId.ToString());

        }

        [Fact]
        public void FindOrganizations_FindOrganizationOfAOrganizer_ShouldReturnAllOrganizerOrganizations()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            var organizer = new Organizer(organizerId);
            var organizers = new List<Organizer>() { organizer };
            var organizers2 = new List<Organizer>() { };
            var ch2_1 = new Organization(Guid.NewGuid(), "ch2_1", organizers, new List<Organization> { });
            var ch1_1 = new Organization(Guid.NewGuid(), "ch1_1", organizers, new List<Organization> { });
            var ch1_2 = new Organization(Guid.NewGuid(), "ch1_2", organizers2, new List<Organization> { ch2_1});
            var root = new Organization(Guid.NewGuid(), "root", organizers, new List<Organization> { ch1_1, ch1_2});
            
            //Act
            var result = Organization.FindOrganizations(organizerId, root);

            // Assert
            Assert.Contains(root, result);
            Assert.Contains(ch1_1, result);
            Assert.DoesNotContain(ch1_2, result);
            Assert.Contains(ch2_1, result);
        }

        [Fact]
        public void FindAllChildrenOrganizations_FromRoot_ShouldReturnFullOrganizationTree()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            var organizer = new Organizer(organizerId);
            var organizers = new List<Organizer>() { organizer };
            var ch2_1 = new Organization(Guid.NewGuid(), "ch2_1", organizers, new List<Organization> { });
            var ch1_1 = new Organization(Guid.NewGuid(), "ch1_1", organizers, new List<Organization> { });
            var ch1_2 = new Organization(Guid.NewGuid(), "ch1_1", organizers, new List<Organization> { ch2_1 });
            var root = new Organization(Guid.NewGuid(), "root", organizers, new List<Organization> { ch1_1, ch1_2 });

            //Act
            var result = Organization.FindOrganizations(organizerId, root);

            // Assert
            Assert.Contains(root, result);
            Assert.Contains(ch1_1, result);
            Assert.Contains(ch1_2, result);
            Assert.Contains(ch1_2, result);
        }
    }
}
