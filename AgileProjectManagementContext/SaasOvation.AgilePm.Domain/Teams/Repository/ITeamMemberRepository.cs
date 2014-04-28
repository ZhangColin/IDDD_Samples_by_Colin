using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Teams.Model;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Domain.Teams.Repository {
    public interface ITeamMemberRepository {
        ICollection<TeamMember> GetAllTeamMembers(TenantId tenantId);
        void Remove(TeamMember teamMember);
        void RemoveAll(IEnumerable<TeamMember> teamMembers);
        void Save(TeamMember teamMember);
        void SaveAll(IEnumerable<TeamMember> teamMembers);
        TeamMember Get(TenantId tenantId, string userName);
    }
}