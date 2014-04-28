using SaasOvation.AgilePm.Domain.Products.Repository;

namespace SaasOvation.AgilePm.Application.Products.BacklogItems {
    public class BacklogItemApplicationService {
         private readonly IBacklogItemRepository _backlogItemRepository;

         public BacklogItemApplicationService(IBacklogItemRepository backlogItemRepository) {
            this._backlogItemRepository = backlogItemRepository;
        }

        //TODO: APIs for student assignment
    }
}