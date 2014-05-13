using System;
using NUnit.Framework;
using SaasOvation.AgilePm.Domain.Teams.Model;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Domain.Test.Team {
    [TestFixture]
    public class TeamTest : DomainTest{
        [Test]
        public void TestCreate() {
            TenantId tenantId = new TenantId("T-12345");
            Teams.Model.Team team = new Teams.Model.Team(tenantId, "Identity and Access Management");

            Assert.AreEqual("Identity and Access Management", team.Name);
        }

        protected Teams.Model.Team CreateTeam() {
            TenantId tenantId = new TenantId("T-12345");
            Teams.Model.Team team = new Teams.Model.Team(tenantId, "Identity and Access Management");
            return team;
        }

        protected ProductOwner CreateProductOwner() {
            return new ProductOwner(new TenantId("T-12345"), "colin", "Colin", "Zhang", "colin@saasovation.com",
                new DateTime(DateTime.Now.Ticks - (86400000L * 30)));
        }

        [Test]
        public void TestAssignProductOwner() {
            Teams.Model.Team team = this.CreateTeam();
            ProductOwner productOwner = this.CreateProductOwner();

            team.AssignProductOwner(productOwner);

            Assert.NotNull(team.ProductOwner);
            Assert.AreEqual(productOwner.ProductOwnerId, team.ProductOwner.ProductOwnerId);
        }
    }
}