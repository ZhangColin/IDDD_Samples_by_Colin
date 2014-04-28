using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Products.Model.BacklogItems;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Releases {
    public class ScheduledBacklogItem : Entity{
        public TenantId TenantId { get; private set; }
        public ReleaseId ReleaseId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public int Ordering { get; private set; }

        public ScheduledBacklogItem(TenantId tenantId, ReleaseId releaseId, BacklogItemId backlogItemId, int ordering = 0) {
            this.TenantId = tenantId;
            this.ReleaseId = releaseId;
            this.BacklogItemId = backlogItemId;
            this.Ordering = ordering;
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.ReleaseId;
            yield return this.BacklogItemId;
        }
    }
}