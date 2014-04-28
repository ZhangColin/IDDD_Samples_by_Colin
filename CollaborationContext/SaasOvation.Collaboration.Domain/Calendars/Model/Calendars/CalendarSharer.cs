using System;
using System.Collections.Generic;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.Calendars {
    public class CalendarSharer: ValueObject, IComparable<CalendarSharer> {
        private readonly Participant _participant;

        public CalendarSharer(Participant participant) {
            AssertionConcern.NotNull(participant, "Participant must be provided.");
            this._participant = participant;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this._participant;
        }

        public int CompareTo(CalendarSharer other) {
            return this._participant.CompareTo(other._participant);
        }
    }
}