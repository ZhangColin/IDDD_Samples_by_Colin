using System;
using SaasOvation.Collaboration.Domain.Calendars.Model.Calendars;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries {
    public class CalendarEntryParticipantUninvited: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public CalendarId CalendarId { get; private set; }
        public CalendarEntryId CalendarEntryId { get; private set; }
        public Participant Participant { get; private set; }

        public CalendarEntryParticipantUninvited(TenantId tenantId, CalendarId calendarId, CalendarEntryId calendarEntryId, Participant participant) {
            this.TenantId = tenantId;
            this.CalendarId = calendarId;
            this.CalendarEntryId = calendarEntryId;
            this.Participant = participant;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}