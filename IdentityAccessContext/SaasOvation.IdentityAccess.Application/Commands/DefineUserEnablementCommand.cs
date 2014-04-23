using System;

namespace SaasOvation.IdentityAccess.Application.Commands {
    public class DefineUserEnablementCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public bool Enabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DefineUserEnablementCommand(string tenantId, string userName, bool enabled, DateTime startDate, DateTime endDate) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.Enabled = enabled;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }
}