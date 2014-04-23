using System;

namespace SaasOvation.Common.Domain.Model.Process {
    public class ProcessId: Identity {
        public static ProcessId ExistingProcessId(string id) {
            return new ProcessId(id);
        }

        public static ProcessId NewProcessId() {
            return new ProcessId(Guid.NewGuid().ToString());
        }

        protected ProcessId() {}
        protected ProcessId(string id): base(id) {}
    }
}