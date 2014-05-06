namespace SaasOvation.Common.Test.Port.Adapter.Messaging {
    public class AllPhoneNumbersListed: PhoneNumberProcessEvent {
        public string PhoneNumbersArray { get; set; }

        public AllPhoneNumbersListed(string processId, string phoneNumbersArray): base(processId) {
            this.PhoneNumbersArray = phoneNumbersArray;
        }
    }
}