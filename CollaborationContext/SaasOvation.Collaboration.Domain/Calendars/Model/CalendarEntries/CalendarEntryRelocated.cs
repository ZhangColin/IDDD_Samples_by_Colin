using System;
using SaasOvation.Collaboration.Domain.Calendars.Model.Calendars;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries {
    public class CalendarEntryRelocated : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public CalendarId CalendarId { get; private set; }
        public CalendarEntryId CalendarEntryId { get; private set; }
        public string Location { get; private set; }

        public CalendarEntryRelocated(TenantId tenantId, CalendarId calendarId, CalendarEntryId calendarEntryId, string location) {
            this.TenantId = tenantId;
            this.CalendarId = calendarId;
            this.CalendarEntryId = calendarEntryId;
            this.Location = location;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}