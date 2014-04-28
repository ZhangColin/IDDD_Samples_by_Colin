using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class TaskStatusChanged: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public TaskId TaskId { get; private set; }
        public TaskStatus Status { get; private set; }

        public TaskStatusChanged(TenantId tenantId, BacklogItemId backlogItemId, TaskId taskId, TaskStatus status) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.TaskId = taskId;
            this.Status = status;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}