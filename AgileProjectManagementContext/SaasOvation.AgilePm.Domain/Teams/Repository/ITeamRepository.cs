using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Teams.Model;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Domain.Teams.Repository {
    public interface ITeamRepository {
        ICollection<Team> GetAllTeams(TenantId tenantId);
        void Remove(Team team);
        void RemoveAll(IEnumerable<Team> teams);
        void Save(Team team);
        void SaveAll(IEnumerable<Team> teams);
        Team GetByName(TenantId tenantId, string name);
    }
}