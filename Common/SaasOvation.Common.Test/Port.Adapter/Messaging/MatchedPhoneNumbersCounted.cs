namespace SaasOvation.Common.Test.Port.Adapter.Messaging {
    public class MatchedPhoneNumbersCounted: PhoneNumberProcessEvent {
        public int MatchedPhoneNumbersCount { get; set; }

        public MatchedPhoneNumbersCounted(string processId, int matchedPhoneNumbersCount): base(processId) {
            this.MatchedPhoneNumbersCount = matchedPhoneNumbersCount;
        }
    }
}