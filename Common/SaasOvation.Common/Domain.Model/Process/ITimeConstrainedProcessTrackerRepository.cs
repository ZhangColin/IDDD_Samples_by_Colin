using System.Collections.Generic;

namespace SaasOvation.Common.Domain.Model.Process {
    public interface ITimeConstrainedProcessTrackerRepository {
        void Add(TimeConstrainedProcessTracker timeConstrainedProcessTracker);
        ICollection<TimeConstrainedProcessTracker> AllTimedOut();
        ICollection<TimeConstrainedProcessTracker> AllTimedOutOf(string tenantId);
        ICollection<TimeConstrainedProcessTracker> AllTrackers(string tenantId);
        void Save(TimeConstrainedProcessTracker timeConstrainedProcessTracker);
        TimeConstrainedProcessTracker TrackerOfProcessId(string tenantId, ProcessId processId);
    }
}