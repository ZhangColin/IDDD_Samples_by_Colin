using System.Collections.Generic;

namespace SaasOvation.Common.Domain.Model.LongRunningProcess {
    public interface ITimeConstrainedProcessTrackerRepository {
        void Add(TimeConstrainedProcessTracker timeConstrainedProcessTracker);
        ICollection<TimeConstrainedProcessTracker> GetAllTimedOut();
        ICollection<TimeConstrainedProcessTracker> GetAllTimedOutOf(string tenantId);
        ICollection<TimeConstrainedProcessTracker> GetAll(string tenantId);
        void Save(TimeConstrainedProcessTracker timeConstrainedProcessTracker);
        TimeConstrainedProcessTracker Get(string tenantId, ProcessId processId);
    }
}