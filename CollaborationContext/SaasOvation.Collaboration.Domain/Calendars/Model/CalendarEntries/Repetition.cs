using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries {
    public class Repetition : ValueObject{
        public RepeatType Repeats { get; private set; }
        public DateTime Ends { get; private set; }

        public Repetition(RepeatType repeats, DateTime ends) {
            this.Repeats = repeats;
            this.Ends = ends;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Repeats;
            yield return this.Ends;
        }

        public static Repetition DoesNotRepeat(DateTime ends) {
            return new Repetition(RepeatType.DoesNotRepeat, ends);
        }

        public static Repetition RepeatsIndefinitely(RepeatType repeatType) {
            return new Repetition(repeatType, DateTime.MaxValue);
        }
    }
}