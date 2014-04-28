using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries {
    public class Alarm : ValueObject{
        public AlarmunitsType AlarmunitsType { get; private set; }
        public int AlarmUnits { get; private set; }

        public Alarm(AlarmunitsType alarmunitsType, int alarmUnits) {
            this.AlarmunitsType = alarmunitsType;
            this.AlarmUnits = alarmUnits;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.AlarmUnits;
            yield return this.AlarmunitsType;
        }
    }
}