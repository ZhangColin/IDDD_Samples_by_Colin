using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SaasOvation.AgilePm.Domain.Discussions;
using SaasOvation.AgilePm.Domain.Products.Model.BacklogItems;
using SaasOvation.AgilePm.Domain.Products.Model.Releases;
using SaasOvation.AgilePm.Domain.Products.Model.Sprints;
using SaasOvation.AgilePm.Domain.Teams.Model;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Products {
    public class Product: Entity {
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public ProductOwnerId ProductOwnerId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ProductDiscussion Discussion{ get; private set; }
        public string DiscussionInitiationId { get; private set; }

        private ISet<ProductBacklogItem> _backlogItems; 

        public Product(TenantId tenantId, ProductId productId, ProductOwnerId productOwnerId, string name,
            string description, DiscussionAvailability discussionAvailability) {
            this.TenantId = tenantId;
            this.ProductId = productId;
            this.ProductOwnerId = productOwnerId;
            this.Name = name;
            this.Description = description;
            this.Discussion = ProductDiscussion.FromAvailability(discussionAvailability);
            this.DiscussionInitiationId = null;

            this._backlogItems = new HashSet<ProductBacklogItem>();

            DomainEventPublisher.Instance.Publish(new ProductCreated(this.TenantId, this.ProductId, this.ProductOwnerId,
                this.Name, this.Description, this.Discussion.Availability));
        }

        public ICollection<ProductBacklogItem> AllBacklogItems() {
            return new ReadOnlyCollection<ProductBacklogItem>(this._backlogItems.ToArray());
        }

        public void ChangeProductOwner(ProductOwner productOwner) {
            if(!this.ProductOwnerId.Equals(productOwner.ProductOwnerId)) {
                this.ProductOwnerId = productOwner.ProductOwnerId;

                // TODO: publish event
            }
        }

        public void FailDiscussionInitiation() {
            if(this.Discussion.Availability!=DiscussionAvailability.Ready) {
                this.DiscussionInitiationId = null;
                this.Discussion = ProductDiscussion.FromAvailability(DiscussionAvailability.Failed);
            }
        }

        public void InitiateDiscussion(DiscussionDescriptor descriptor) {
            if(descriptor == null) {
                throw new InvalidOperationException("The descriptor must not be null.");
            }
            if(this.Discussion.Availability==DiscussionAvailability.Requested) {
                this.Discussion = this.Discussion.NowReady(descriptor);

                DomainEventPublisher.Instance.Publish(new ProductDiscussionInitiated(this.TenantId, this.ProductId,
                    this.Discussion));
            }
        }

        public BacklogItem PlanBacklogItem(BacklogItemId newBacklogItemId, string summary, string category,
            BacklogItemType type, StoryPoints storyPoints) {
            BacklogItem backlogItem = new BacklogItem(this.TenantId, this.ProductId, newBacklogItemId, summary, category,
                type, BacklogItemStatus.Planned, storyPoints);

            DomainEventPublisher.Instance.Publish(new ProductBacklogItemPlanned(backlogItem.TenantId,
                backlogItem.ProductId, backlogItem.BacklogItemId, backlogItem.Summary, backlogItem.Category,
                backlogItem.Type, backlogItem.StoryPoints));

            return backlogItem;
        }

        public void PlannedProductBacklogItem(BacklogItem backlogItem) {
            AssertionConcern.Equals(this.TenantId, backlogItem.TenantId,
                "The product and backlog item must have same tenant.");
            AssertionConcern.Equals(this.ProductId, backlogItem.ProductId, "The backlog item must belong to product");

            int ordering = this._backlogItems.Count + 1;

            ProductBacklogItem productBacklogItem = new ProductBacklogItem(this.TenantId, this.ProductId,
                backlogItem.BacklogItemId, ordering);

            this._backlogItems.Add(productBacklogItem);
        }

        public void ReorderFrom(BacklogItemId id, int ordering) {
            foreach(ProductBacklogItem productBacklogItem in this._backlogItems) {
                productBacklogItem.ReorderFrom(id, ordering);
            }
        }

        public void RequestDiscussion(DiscussionAvailability discussionAvailability) {
            if(this.Discussion.Availability!=DiscussionAvailability.Ready) {
                this.Discussion = ProductDiscussion.FromAvailability(discussionAvailability);

                DomainEventPublisher.Instance.Publish(new ProductDiscussionRequested(this.TenantId, this.ProductId,
                    this.ProductOwnerId, this.Name, this.Description,
                    this.Discussion.Availability == DiscussionAvailability.Requested));
            }
        }

        public Release ScheduleRelease(ReleaseId newReleaseId, string name, string description, DateTime begins,
            DateTime ends) {
            Release release = new Release(this.TenantId, this.ProductId, newReleaseId, name, description, begins, ends);

            DomainEventPublisher.Instance.Publish(new ProductReleaseScheduled(release.TenantId, release.ProductId,
                release.ReleaseId, release.Name, release.Description, release.Begins, release.Ends));

            return release;
        }

        public Sprint ScheduleSprint(SprintId newSprintId, string name, string goals, DateTime begins, DateTime ends) {
            Sprint sprint = new Sprint(this.TenantId, this.ProductId, newSprintId, name, goals, begins, ends);
            DomainEventPublisher.Instance.Publish(new ProductSprintScheduled(sprint.TenantId, sprint.ProductId,
                sprint.SprintId, sprint.Name, sprint.Goals, sprint.Begins, sprint.Ends));

            return sprint;
        }

        public void StartDiscussionInitiation(string discussionInitiationId) {
            if(this.Discussion.Availability!=DiscussionAvailability.Ready) {
                this.DiscussionInitiationId = discussionInitiationId;
            }
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.ProductId;
        }
        public override string ToString() {
            return "Product [TenantId=" + TenantId + ", ProductId=" + ProductId
                + ", BacklogItems=" + _backlogItems + ", Description="
                + Description + ", Discussion=" + Discussion
                + ", DiscussionInitiationId=" + DiscussionInitiationId
                + ", Name=" + Name + ", ProductOwnerId=" + ProductOwnerId + "]";
        }
    }
}