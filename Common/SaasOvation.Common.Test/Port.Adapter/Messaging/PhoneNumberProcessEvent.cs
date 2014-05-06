using System;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Test.Port.Adapter.Messaging {
    public class PhoneNumberProcessEvent: IDomainEvent {
        public string ProcessId { get; set; }

        public PhoneNumberProcessEvent(string processId) {
            this.ProcessId = processId;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}