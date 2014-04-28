using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class TaskRenamed: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public TaskId TaskId { get; private set; }
        public string Name { get; private set; }

        public TaskRenamed(TenantId tenantId, BacklogItemId backlogItemId, TaskId taskId, string name) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.TaskId = taskId;
            this.Name = name;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}