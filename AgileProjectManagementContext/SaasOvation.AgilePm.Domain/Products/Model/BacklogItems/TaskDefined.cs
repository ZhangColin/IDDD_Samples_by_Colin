using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class TaskDefined: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public TaskId TaskId { get; private set; }
        public string VolunteerMemberId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public TaskDefined(TenantId tenantId, BacklogItemId backlogItemId, TaskId taskId, string volunteerMemberId,
            string name, string description) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.TaskId = taskId;
            this.VolunteerMemberId = volunteerMemberId;
            this.Name = name;
            this.Description = description;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}