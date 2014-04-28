namespace SaasOvation.Collaboration.Domain.Collaborators {
    public class Author:Collaborator {
        public Author(string identity, string name, string emailAddress)
            : base(identity, name, emailAddress) {}
    }
}