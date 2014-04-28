namespace SaasOvation.AgilePm.Application.Sprints {
    public class CommitBacklogItemToSprintCommand {
        public string TenantId { get; set; }
        public string BacklogItemId { get; set; }
        public string SprintId { get; set; }

        public CommitBacklogItemToSprintCommand(string tenantId, string backlogItemId, string sprintId) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.SprintId = sprintId;
        }
    }
}