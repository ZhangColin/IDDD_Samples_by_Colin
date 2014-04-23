namespace SaasOvation.Common.Events.Sourcing {
    public interface IEventDispatcher {
        void Dispatch(DispatchableDomainEvent dispatchableDomainEvent);
        void RegisterEventDispatcher(IEventDispatcher eventDispatcher);
        bool Understands(DispatchableDomainEvent dispatchableDomainEvent);
    }
}