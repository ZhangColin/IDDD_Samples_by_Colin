using System;
using SaasOvation.Collaboration.Domain.Calendars.Model.Calendars;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries {
    public class CalendarEntryRescheduled : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public CalendarId CalendarId { get; private set; }
        public CalendarEntryId CalendarEntryId { get; private set; }
        public DateRange TimeSpan { get; private set; }
        public Repetition Repetition { get; private set; }
        public Alarm Alarm { get; private set; }

        public CalendarEntryRescheduled(TenantId tenantId, CalendarId calendarId, CalendarEntryId calendarEntryId,
            DateRange timeSpan, Repetition repetition, Alarm alarm) {
            this.TenantId = tenantId;
            this.CalendarId = calendarId;
            this.CalendarEntryId = calendarEntryId;
            this.TimeSpan = timeSpan;
            this.Repetition = repetition;
            this.Alarm = alarm;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}