using SaasOvation.AgilePm.Domain.Products.Model.BacklogItems;
using SaasOvation.AgilePm.Domain.Products.Model.Sprints;
using SaasOvation.AgilePm.Domain.Products.Repository;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Application.Sprints {
    public class SprintApplicationService {
        private readonly ISprintRepository _sprintRepository;
        private readonly IBacklogItemRepository _backlogItemRepository;

        public SprintApplicationService(ISprintRepository sprintRepository, IBacklogItemRepository backlogItemRepository) {
            this._sprintRepository = sprintRepository;
            this._backlogItemRepository = backlogItemRepository;
        }

        public void CommitBacklogItemToSprint(CommitBacklogItemToSprintCommand command) {
            TenantId tenantId = new TenantId(command.TenantId);
            Sprint sprint = this._sprintRepository.Get(tenantId, new SprintId(command.SprintId));
            BacklogItem backlogItem = this._backlogItemRepository.Get(tenantId, new BacklogItemId(command.BacklogItemId));

            sprint.Commit(backlogItem);

            this._sprintRepository.Save(sprint);
        }
    }
}