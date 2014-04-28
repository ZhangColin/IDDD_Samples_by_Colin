using System;
using System.Collections.Generic;
using SaasOvation.Collaboration.Domain.Calendars.Model.Calendars;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries {
    public class CalendarEntryScheduled : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public CalendarId CalendarId { get; private set; }
        public CalendarEntryId CalendarEntryId { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public Owner Owner { get; private set; }
        public DateRange TimeSpan { get; private set; }
        public Repetition Repetition { get; private set; }
        public Alarm Alarm { get; private set; }
        public IEnumerable<Participant> Invitees { get; private set; }

        public CalendarEntryScheduled(TenantId tenantId, CalendarId calendarId, CalendarEntryId calendarEntryId,
            string description, string location, Owner owner, DateRange timeSpan, Repetition repetition, Alarm alarm,
            IEnumerable<Participant> invitees) {
            this.TenantId = tenantId;
            this.CalendarId = calendarId;
            this.CalendarEntryId = calendarEntryId;
            this.Description = description;
            this.Location = location;
            this.Owner = owner;
            this.TimeSpan = timeSpan;
            this.Repetition = repetition;
            this.Alarm = alarm;
            this.Invitees = invitees;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}