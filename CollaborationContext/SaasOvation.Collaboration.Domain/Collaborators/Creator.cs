namespace SaasOvation.Collaboration.Domain.Collaborators {
    public class Creator: Collaborator {
        public Creator(string identity, string name, string emailAddress)
            : base(identity, name, emailAddress) {}
    }
}