namespace SaasOvation.Collaboration.Domain.Collaborators {
    public class Moderator: Collaborator {
        public Moderator(string identity, string name, string emailAddress)
            : base(identity, name, emailAddress) {}
    }
}