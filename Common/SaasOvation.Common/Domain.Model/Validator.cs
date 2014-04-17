namespace SaasOvation.Common.Domain.Model {
    public abstract class Validator {
        private readonly IValidationNotificationHandler _validationNotificationHandler;

        protected Validator(IValidationNotificationHandler validationNotificationHandler) {
            this._validationNotificationHandler = validationNotificationHandler;
        }

        protected IValidationNotificationHandler ValidationNotificationHandler {
            get { return this._validationNotificationHandler; }
        }

        public abstract void Validate();
    }
}