namespace SaasOvation.Common.Test.Port.Adapter.Messaging {
    public class PhoneNumbersMatched : PhoneNumberProcessEvent{
        public string MatchedPhoneNumbers { get; set; }

        public PhoneNumbersMatched(string processId, string matchedPhoneNumbers): base(processId) {
            this.MatchedPhoneNumbers = matchedPhoneNumbers;
        }
    }
}