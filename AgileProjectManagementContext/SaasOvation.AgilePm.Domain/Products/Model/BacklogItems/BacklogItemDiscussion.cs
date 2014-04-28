using System;
using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Discussions;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemDiscussion: ValueObject {
        public DiscussionDescriptor Descriptor { get; private set; }
        public DiscussionAvailability Availability { get; private set; }

        public static BacklogItemDiscussion FromAvailability(DiscussionAvailability availability) {
            if(availability==DiscussionAvailability.Ready) {
                throw new ArgumentException("Cannot be created ready.");
            }
            return new BacklogItemDiscussion(new DiscussionDescriptor(DiscussionDescriptor.UndefinedId), availability);
        }

        public BacklogItemDiscussion(DiscussionDescriptor descriptor, DiscussionAvailability availability) {
            this.Descriptor = descriptor;
            this.Availability = availability;
        }

        public BacklogItemDiscussion NowReady(DiscussionDescriptor descriptor) {
            if(descriptor==null || descriptor.IsUndefined) {
                throw new InvalidOperationException("The discussion descriptor must be defined.");
            }
            if(this.Availability!=DiscussionAvailability.Requested) {
                throw new InvalidOperationException("The discussion must be requested first.");
            }
            return new BacklogItemDiscussion(descriptor, DiscussionAvailability.Ready);
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Availability;
            yield return this.Descriptor;
        }
    }
}