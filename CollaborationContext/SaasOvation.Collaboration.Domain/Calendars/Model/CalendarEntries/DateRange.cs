using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries {
    public class DateRange : ValueObject{
        public DateTime Begins { get; private set; }
        public DateTime Ends { get; private set; }

        public DateRange(DateTime begins, DateTime ends) {
            AssertionConcern.True(begins>ends, "Time span must not end before it begins.");
            this.Begins = begins;
            this.Ends = ends;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Begins;
            yield return this.Ends;
        }
    }
}