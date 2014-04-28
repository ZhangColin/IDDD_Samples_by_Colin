using System;
using SaasOvation.AgilePm.Domain.Products.Model.Sprints;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Products {
    public class ProductSprintScheduled: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public SprintId SprintId { get; private set; }
        public string Name { get; private set; }
        public string Goals { get; private set; }
        public DateTime Starts { get; private set; }
        public DateTime Ends { get; private set; }

        public ProductSprintScheduled(TenantId tenantId, ProductId productId, SprintId sprintId, string name,
            string goals, DateTime starts, DateTime ends) {
            this.TenantId = tenantId;
            this.ProductId = productId;
            this.SprintId = sprintId;
            this.Name = name;
            this.Goals = goals;
            this.Starts = starts;
            this.Ends = ends;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}