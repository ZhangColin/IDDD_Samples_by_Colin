using System;
using System.Collections.Generic;
using System.Linq;
using SaasOvation.Collaboration.Domain.Calendars.Model.Calendars;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries {
    public class CalendarEntry: EventSourcedRootEntity {
        private TenantId _tenantId;
        private CalendarId _calendarId;
        private CalendarEntryId _calendarEntryId;
        private string _description;
        private string _location;
        private Owner _owner;
        private DateRange _timeSpan;
        private Repetition _repetition;
        private Alarm _alarm;
        private HashSet<Participant> _invitees;
 
        public CalendarEntry(IEnumerable<IDomainEvent> eventStream, int streamVersion): base(eventStream, streamVersion) {}

        public CalendarEntry(TenantId tenantId, CalendarId calendarId, CalendarEntryId calendarEntryId,
            string description, string location, Owner owner, DateRange timeSpan, Repetition repetition, 
            Alarm alarm, IEnumerable<Participant> invitees = null) {
            AssertionConcern.NotNull(tenantId, "The tenant must be provided.");
            AssertionConcern.NotNull(calendarId, "The calendar id must be provided.");
            AssertionConcern.NotNull(calendarEntryId, "The calendar entry id must be provided.");
            AssertionConcern.NotEmpty(description, "The description must be provided.");
            AssertionConcern.NotEmpty(location, "The location must be provided.");
            AssertionConcern.NotNull(owner, "The owner must be provided.");
            AssertionConcern.NotNull(timeSpan, "The time span must be provided.");
            AssertionConcern.NotNull(repetition, "The repetition must be provided.");
            AssertionConcern.NotNull(alarm, "The alarm must be provided.");

            if(repetition.Repeats==RepeatType.DoesNotRepeat) {
                repetition = Repetition.DoesNotRepeat(timeSpan.Ends);
            }

            AssertTimeSpans(repetition, timeSpan);

            this.Apply(new CalendarEntryScheduled(tenantId, calendarId, calendarEntryId, description, location, owner,
                timeSpan, repetition, alarm, invitees));
        }

        private void AssertTimeSpans(Repetition repetition, DateRange timeSpan) {
            if(repetition.Repeats==RepeatType.DoesNotRepeat) {
                AssertionConcern.Equals(repetition.Ends, timeSpan.Ends, "Non-repeating entry must end with time span end.");
            }
            else {
                AssertionConcern.False(timeSpan.Ends>repetition.Ends, "Time span must end when or before repetition ends.");
            }
        }

        public CalendarEntryId CalendarEntryId {
            get { return this._calendarEntryId; }
        }

        private void When(CalendarEntryScheduled e) {
            this._tenantId = e.TenantId;
            this._calendarId = e.CalendarId;
            this._calendarEntryId = e.CalendarEntryId;
            this._description = e.Description;
            this._location = e.Location;
            this._owner = e.Owner;
            this._timeSpan = e.TimeSpan;
            this._repetition = e.Repetition;
            this._alarm = e.Alarm;
            this._invitees = new HashSet<Participant>(e.Invitees ?? Enumerable.Empty<Participant>());
        }

        public void ChangeDescription(string description) {
            if(description==null) {
                // TODO: consider
            }

            description = description.Trim();

            if(!string.IsNullOrEmpty(description) && !this._description.Equals(description)) {
                this.Apply(new CalendarEntryDescriptionChanged(this._tenantId, this._calendarId, this._calendarEntryId,
                    description));
            }
        }

        private void When(CalendarEntryDescriptionChanged e) {
            this._description = e.Description;
        }

        public void Invite(Participant participant) {
            AssertionConcern.NotNull(participant, "The participant must be provided.");
            if(!this._invitees.Contains(participant)) {
                this.Apply(new CalendarEntryParticipantInvited(this._tenantId, this._calendarId, this._calendarEntryId,
                    participant));
            }
        }

        private void When(CalendarEntryParticipantInvited e) {
            this._invitees.Add(e.Participant);
        }

        public void Relocate(string location) {
            if(location == null) {
                // TODO: consider
            }

            location = location.Trim();

            if(!string.IsNullOrEmpty(location) && !this._location.Equals(location)) {
                this.Apply(new CalendarEntryRelocated(this._tenantId, this._calendarId, this._calendarEntryId, location));
            }
        }

        private void When(CalendarEntryRelocated e) {
            this._location = e.Location;
        }

        public void Reshedule(string description, string location, DateRange timeSpan, Repetition repetition,
            Alarm alarm) {
            AssertionConcern.NotNull(timeSpan, "The time span must be provided.");
            AssertionConcern.NotNull(repetition, "The repetition must be provided.");
            AssertionConcern.NotNull(alarm, "The alarm must be provided.");

            if(repetition.Repeats == RepeatType.DoesNotRepeat) {
                repetition = Repetition.DoesNotRepeat(timeSpan.Ends);
            }

            this.AssertTimeSpans(repetition, timeSpan);

            this.ChangeDescription(description);
            this.Relocate(location);

            this.Apply(new CalendarEntryRescheduled(this._tenantId, this._calendarId, this._calendarEntryId, timeSpan,
                repetition, alarm));
        }

        private void When(CalendarEntryRescheduled e) {
            this._timeSpan = e.TimeSpan;
            this._repetition = e.Repetition;
            this._alarm = e.Alarm;
        }

        public void Uninvite(Participant participant) {
            AssertionConcern.NotNull(participant, "The participant must be provided.");
            if(this._invitees.Contains(participant)) {
                this.Apply(new CalendarEntryParticipantUninvited(this._tenantId, this._calendarId, this._calendarEntryId,
                    participant));
            }
        }

        private void When(CalendarEntryParticipantUninvited e) {
            this._invitees.Remove(e.Participant);
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this._tenantId;
            yield return this._calendarId;
            yield return this._calendarEntryId;
        }
    }
}