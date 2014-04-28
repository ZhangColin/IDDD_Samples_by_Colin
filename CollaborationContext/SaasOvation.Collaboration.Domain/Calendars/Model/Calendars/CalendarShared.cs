using System;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.Calendars {
    public class CalendarShared : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public CalendarId CalendarId { get; private set; }
        public string Name { get; private set; }
        public CalendarSharer SharedWith { get; private set; }

        public CalendarShared(TenantId tenantId, CalendarId calendarId, string name, CalendarSharer sharedWith) {
            this.TenantId = tenantId;
            this.CalendarId = calendarId;
            this.Name = name;
            this.SharedWith = sharedWith;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}