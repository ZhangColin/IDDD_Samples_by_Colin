using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Products.Model.BacklogItems;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Sprints {
    public class CommittedBacklogItem : Entity{
        public TenantId TenantId { get; private set; }
        public SprintId SprintId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public int Ordering { get; private set; }

        public CommittedBacklogItem(TenantId tenantId, SprintId sprintId, BacklogItemId backlogItemId, int ordering = 0) {
            this.TenantId = tenantId;
            this.SprintId = sprintId;
            this.BacklogItemId = backlogItemId;
            this.Ordering = ordering;
        }


        public void ReOrderFrom(BacklogItemId id, int orderOfPriority) {
            if(this.BacklogItemId.Equals(id)) {
                this.Ordering = orderOfPriority;
            }
            else if(this.Ordering >= orderOfPriority) {
                this.Ordering = this.Ordering + 1;
            }
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.SprintId;
            yield return this.BacklogItemId;
        }
    }
}