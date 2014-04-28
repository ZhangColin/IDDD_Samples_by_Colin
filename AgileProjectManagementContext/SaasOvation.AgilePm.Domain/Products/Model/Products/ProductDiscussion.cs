using System;
using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Discussions;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Products {
    public class ProductDiscussion: ValueObject {
        public DiscussionDescriptor Descriptor { get; private set; }
        public DiscussionAvailability Availability { get; private set; }

        public ProductDiscussion(DiscussionDescriptor descriptor, DiscussionAvailability availability) {
            AssertionConcern.NotNull(descriptor, "The descriptor must be provided.");
            this.Descriptor = descriptor;
            this.Availability = availability;
        }

        public ProductDiscussion(ProductDiscussion productDiscussion)
            : this(productDiscussion.Descriptor, productDiscussion.Availability) {}

        public static ProductDiscussion FromAvailability(DiscussionAvailability availability) {
            if(availability==DiscussionAvailability.Ready) {
                throw new InvalidOperationException("Cannot be created ready.");
            }

            DiscussionDescriptor descriptor = new DiscussionDescriptor(DiscussionDescriptor.UndefinedId);

            return new ProductDiscussion(descriptor, availability);
        }

        public ProductDiscussion NowReady(DiscussionDescriptor descriptor) {
            if(descriptor==null || descriptor.IsUndefined) {
                throw new ArgumentException("The discussion descriptor must be defined.");
            }
            if(this.Availability!=DiscussionAvailability.Requested) {
                throw new InvalidOperationException("The discussion must be requested first.");
            }
            return new ProductDiscussion(descriptor, DiscussionAvailability.Ready);
        }


        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Availability;
            yield return this.Descriptor;
        }
    }
}