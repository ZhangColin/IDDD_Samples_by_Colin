using System;
using SaasOvation.Collaboration.Domain.Calendars.Model.Calendars;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries {
    public class CalendarEntryDescriptionChanged : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public CalendarId CalendarId { get; private set; }
        public CalendarEntryId CalendarEntryId { get; private set; }
        public string Description { get; private set; }

        public CalendarEntryDescriptionChanged(TenantId tenantId, CalendarId calendarId, CalendarEntryId calendarEntryId, string description) {
            this.TenantId = tenantId;
            this.CalendarId = calendarId;
            this.CalendarEntryId = calendarEntryId;
            this.Description = description;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}