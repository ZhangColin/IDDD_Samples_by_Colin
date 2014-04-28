using System;

namespace SaasOvation.Common.Domain.Model.LongRunningProcess {
    public interface IProcess {
        long AllowableDuration { get; }
        bool CanTimeout { get; }
        long CurrentDuration { get; }
        string Description { get; }
        bool DidProcessingComplete { get; }
        void InformTimeout(DateTime timedOutDate);
        bool IsCompleted { get; }
        bool IsTimedOut { get; }
        bool NotCompleted { get; }
        ProcessCompletionType ProcessCompletionType { get; }
        ProcessId ProcessId { get; }
        DateTime StartTime { get; }
        TimeConstrainedProcessTracker TimeConstrainedProcessTracker { get; }
        DateTime? TimedOutDate { get; }
        long TotalAllowableDuration { get; }
        int TotalRetriesPermitted { get; }
    }
}