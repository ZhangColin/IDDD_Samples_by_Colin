using System;
using System.Collections.Generic;

namespace SaasOvation.Common.Domain.Model.LongRunningProcess {
    public class ProcessId: Identity {
        public static ProcessId ExistingProcessId(string id) {
            return new ProcessId(id);
        }

        public static ProcessId NewProcessId() {
            return new ProcessId(Guid.NewGuid().ToString());
        }

        public ProcessId(string id): base(id) {}

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Id;
        }
    }
}