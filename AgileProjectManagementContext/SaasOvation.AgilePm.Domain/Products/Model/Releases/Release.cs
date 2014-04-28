using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SaasOvation.AgilePm.Domain.Products.Model.Products;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Releases {
    public class Release: Entity {
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public ReleaseId ReleaseId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime Begins { get; private set; }
        public DateTime Ends { get; private set; }

        private readonly ISet<ScheduledBacklogItem> _backlogItems; 

        public Release(TenantId tenantId, ProductId productId, ReleaseId releaseId, string name, string description, DateTime begins, DateTime ends) {
            this.TenantId = tenantId;
            this.ProductId = productId;
            this.ReleaseId = releaseId;
            this.Name = name;
            this.Description = description;
            this.Begins = begins;
            this.Ends = ends;

            _backlogItems = new HashSet<ScheduledBacklogItem>();
        }

        public ICollection<ScheduledBacklogItem> AllScheduledBacklogItems() {
            return new ReadOnlyCollection<ScheduledBacklogItem>(this._backlogItems.ToArray());
        } 

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.ProductId;
            yield return this.ReleaseId;
        }
    }
}