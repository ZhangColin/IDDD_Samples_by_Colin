using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model.LongRunningProcess;

namespace SaasOvation.AgilePm.Application.Processes {
    public class ProcessApplicationService {
        private readonly ITimeConstrainedProcessTrackerRepository _processTrackerRepository;

        public ProcessApplicationService(ITimeConstrainedProcessTrackerRepository processTrackerRepository) {
            this._processTrackerRepository = processTrackerRepository;
        }

        public void CheckForTimedOutProcesses() {
            ApplicationServiceLifeCycle.Begin();
            try {
                ICollection<TimeConstrainedProcessTracker> trackers = this._processTrackerRepository.GetAllTimedOut();

                foreach(TimeConstrainedProcessTracker tracker in trackers) {
                    tracker.InformProcessTimedOut();
                    this._processTrackerRepository.Save(tracker);
                }
                ApplicationServiceLifeCycle.Success();
            }
            catch(Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }
        }
    }
}