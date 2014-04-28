using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Discussions {
    public class DiscussionDescriptor : ValueObject {
        public const string UndefinedId = "UNDEFINED";
        public string Id { get; private set; }

        public DiscussionDescriptor(string id) {
            this.Id = id;
        }

        public DiscussionDescriptor(DiscussionDescriptor discussionDescriptor)
            : this(discussionDescriptor.Id) {}

        public bool IsUndefined {
            get { return this.Id.Equals(UndefinedId); }
        }

        public override string ToString() {
            return "DiscussionDescriptor [id=" + Id + "]";
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Id;
        }
    }
}