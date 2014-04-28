using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Domain.Collaborators {
    public interface ICollaboratorService {
        Author GetAuthorFrom(TenantId tenantId, string identity);
        Creator GetCreatorFrom(TenantId tenantId, string identity);
        Moderator GetModeratorFrom(TenantId tenantId, string identity);
        Owner GetOwnerFrom(TenantId tenantId, string identity);
        Participant GetParticipantFrom(TenantId tenantId, string identity);
    }
}