using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Teams.Model {
    public class Team: Entity {
        public TenantId TenantId { get; private set; }
        public string Name { get; private set; }
        public ProductOwner ProductOwner { get; private set; }

        private readonly HashSet<TeamMember> _teamMembers; 

        public Team(TenantId tenantId, string name, ProductOwner productOwner = null) {
            AssertionConcern.NotNull(tenantId, "The tenant id must be provided.");
            AssertionConcern.NotEmpty(name, "The name must be provided.");
            AssertionConcern.Length(name, 100, "The name must be 100 characters or less.");
            AssertionConcern.NotNull(productOwner, "The productOwner must be provided.");
            AssertionConcern.Equals(tenantId, productOwner.TenantId, "The productOwner must be of the same tenant.");
                      
            this.TenantId = tenantId;
            this.Name = name;
            if(productOwner!=null) {
                this.ProductOwner = productOwner;
            }
            _teamMembers = new HashSet<TeamMember>();
        }

        public ReadOnlyCollection<TeamMember> AllTeamMembers {
            get {
                return new ReadOnlyCollection<TeamMember>(this._teamMembers.ToArray());
            }
        }

        public void AssignProductOwner(ProductOwner productOwner) {
            this.ProductOwner = productOwner;
        }

        public void AssignTeamMember(TeamMember teamMember) {
            AssertValidTeamMember(teamMember);
            this._teamMembers.Add(teamMember);
        }

        public bool IsTeamMember(TeamMember teamMember) {
            AssertValidTeamMember(teamMember);
            return GetTeamMemberByUserName(teamMember.UserName) != null;
        }

        public void RemoveTeamMember(TeamMember teamMember) {
            AssertValidTeamMember(teamMember);
            TeamMember existingTeamMember = GetTeamMemberByUserName(teamMember.UserName);
            if(existingTeamMember!=null) {
                this._teamMembers.Remove(existingTeamMember);
            }
        }

        private void AssertValidTeamMember(TeamMember teamMember) {
            AssertionConcern.NotNull(teamMember, "A team member must be provided.");
            AssertionConcern.Equals(this.TenantId, teamMember.TenantId, "Team member must be of the same tenant.");
        }

        private TeamMember GetTeamMemberByUserName(string userName) {
            return this._teamMembers.FirstOrDefault(x => x.UserName == userName);
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.Name;
        }
    }
}