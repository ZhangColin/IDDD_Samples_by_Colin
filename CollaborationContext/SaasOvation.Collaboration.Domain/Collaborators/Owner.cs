namespace SaasOvation.Collaboration.Domain.Collaborators {
    public class Owner: Collaborator {
        public Owner(string identity, string name, string emailAddress)
            : base(identity, name, emailAddress) {}
    }
}