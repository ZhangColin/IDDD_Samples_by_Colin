namespace SaasOvation.Collaboration.Domain.Collaborators {
    public class Participant: Collaborator {
        public Participant(string identity, string name, string emailAddress)
            : base(identity, name, emailAddress) {}
    }
}