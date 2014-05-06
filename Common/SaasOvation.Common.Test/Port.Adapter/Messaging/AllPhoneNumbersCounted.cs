namespace SaasOvation.Common.Test.Port.Adapter.Messaging {
    public class AllPhoneNumbersCounted: PhoneNumberProcessEvent {
        public int TotalPhoneNumbersCount { get; set; }

        public AllPhoneNumbersCounted(string processId, int totalPhoneNumbersCount): base(processId) {
            this.TotalPhoneNumbersCount = totalPhoneNumbersCount;
        }
    }
}