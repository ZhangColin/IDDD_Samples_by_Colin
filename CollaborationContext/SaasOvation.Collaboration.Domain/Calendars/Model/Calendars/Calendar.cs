using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries;
using SaasOvation.Collaboration.Domain.Calendars.Service;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.Calendars {
    public class Calendar: EventSourcedRootEntity {
        private TenantId _tenantId;
        private CalendarId _calendarId;
        private string _name;
        private string _description;
        private HashSet<CalendarSharer> _sharedWith;

        public Calendar(IEnumerable<IDomainEvent> eventStream, int streamVersion): base(eventStream, streamVersion) {}

        public Calendar(TenantId tenantId, CalendarId calendarId, string name, string description, Owner owner,
            IEnumerable<CalendarSharer> sharedWith = null) {
            AssertionConcern.NotNull(tenantId, "The tenant must be provided.");
            AssertionConcern.NotNull(calendarId, "The calendar id must be provided.");
            AssertionConcern.NotEmpty(name, "The name must be provided.");
            AssertionConcern.NotEmpty(description, "The description must be provided.");
            AssertionConcern.NotNull(owner, "The owner must be provided.");

            this.Apply(new CalendarCreated(tenantId, calendarId, name, description, owner, sharedWith));
        }

        private void When(CalendarCreated e) {
            this._tenantId = e.TenantId;
            this._calendarId = e.CalendarId;
            this._name = e.Name;
            this._description = e.Description;
            this._sharedWith = new HashSet<CalendarSharer>(e.SharedWith ?? Enumerable.Empty<CalendarSharer>());
        }

        public CalendarId CalendarId {
            get { return this._calendarId; }
        }

        public IReadOnlyCollection<CalendarSharer> AllSharedWith {
            get {
                return new ReadOnlyCollection<CalendarSharer>(this._sharedWith.ToArray());
            }
        }

        public void ChangeDescription(string description) {
            AssertionConcern.NotEmpty(description, "The description must be provided.");
            this.Apply(new CalendarDescriptionChanged(this._tenantId, this._calendarId, this._name, description));
        }

        private void When(CalendarDescriptionChanged e) {
            this._description = e.Description;
        }

        public void Rename(string name) {
            AssertionConcern.NotEmpty(name, "The name must be provided.");
            this.Apply(new CalendarRenamed(this._tenantId, this._calendarId, name, this._description));
        }

        private void When(CalendarRenamed e) {
            this._name = e.Name;
        }

        public CalendarEntry ScheduleCalendarEntry(CalendarIdentityService calendarIdentityService, string description,
            string location, Owner owner, DateRange timeSpan, Repetition repetition, Alarm alarm,
            IEnumerable<Participant> invitees = null) {
            return new CalendarEntry(this._tenantId, this._calendarId, calendarIdentityService.GetNextCalendarEntryId(),
                description, location, owner, timeSpan, repetition, alarm, invitees);
        }

        public void ShareCalendarWith(CalendarSharer calendarSharer) {
            AssertionConcern.NotNull(calendarSharer, "The calendar sharer must be provided.");
            if(!this._sharedWith.Contains(calendarSharer)) {
                this.Apply(new CalendarShared(this._tenantId, this._calendarId, this._name, calendarSharer));
            }
        }

        private void When(CalendarShared e) {
            this._sharedWith.Add(e.SharedWith);
        }

        public void UnshareCalendarWith(CalendarSharer calendarSharer) {
            AssertionConcern.NotNull(calendarSharer, "The calendar sharer must be provided.");
            if(this._sharedWith.Contains(calendarSharer)) {
                this.Apply(new CalendarUnhared(this._tenantId, this._calendarId, this._name, calendarSharer));
            }
        }

        private void When(CalendarUnhared e) {
            this._sharedWith.Remove(e.UnsharedWith);
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this._tenantId;
            yield return this._calendarId;
        }
    }
}