using System;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.Calendars {
    public class CalendarUnhared : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public CalendarId CalendarId { get; private set; }
        public string Name { get; private set; }
        public CalendarSharer UnsharedWith { get; private set; }

        public CalendarUnhared(TenantId tenantId, CalendarId calendarId, string name, CalendarSharer unsharedWith) {
            this.TenantId = tenantId;
            this.CalendarId = calendarId;
            this.Name = name;
            this.UnsharedWith = unsharedWith;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}