using System;
using System.Collections.Generic;
using System.Linq;
using SaasOvation.AgilePm.Domain.Discussions;
using SaasOvation.AgilePm.Domain.Products.Model.Products;
using SaasOvation.AgilePm.Domain.Products.Model.Releases;
using SaasOvation.AgilePm.Domain.Products.Model.Sprints;
using SaasOvation.AgilePm.Domain.Teams.Model;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItem: Entity {
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public string Summary { get; private set; }
        public string Category { get; private set; }
        public BacklogItemType Type { get; private set; }
        public BacklogItemStatus Status { get; set; }
        public StoryPoints StoryPoints { get; private set; }

        public string AssociatedIssueId { get; private set; }
        public BusinessPrionrity BusinessPrionrity { get; private set; }
        public ReleaseId ReleaseId { get; private set; }
        public SprintId SprintId { get; private set; }
        public BacklogItemDiscussion Discussion { get; private set; }
        public string Story { get; private set; }

        private string _discussionInitiationId;

        private readonly List<Task> _tasks; 

        public BacklogItem(TenantId tenantId, ProductId productId, BacklogItemId backlogItemId, string summary,
            string category, BacklogItemType type, BacklogItemStatus backlogItemStatus, StoryPoints storyPoints) {
            AssertionConcern.NotEmpty(summary, "The summary must be provided.");
            AssertionConcern.Length(summary, 100, "The summary must be 100 characters or less.");

            this.TenantId = tenantId;
            this.ProductId = productId;
            this.BacklogItemId = backlogItemId;
            this.Summary = summary;
            this.Category = category;
            this.Type = type;
            this.Status = backlogItemStatus;
            this.StoryPoints = storyPoints;

            this._tasks = new List<Task>();
        }

        public bool IsDone {
            get { return this.Status == BacklogItemStatus.Done; }
        }

        public bool IsPlanned {
            get { return this.Status == BacklogItemStatus.Planned; }
        }

        public bool IsRemoved {
            get { return this.Status == BacklogItemStatus.Removed; }
        }

        public void AssociateWithIssue(string issueId) {
            if(this.AssociatedIssueId==null) {
                this.AssociatedIssueId = issueId;
            }
        }

        public bool HasBusinessPriority {
            get { return this.BusinessPrionrity != null; }
        }

        public void AssignBusinessPriority(BusinessPrionrity businessPrionrity) {
            this.BusinessPrionrity = businessPrionrity;
            DomainEventPublisher.Instance.Publish(new BusinessPriorityAssigned(this.TenantId, this.BacklogItemId,
                businessPrionrity));
        }

        public void AssignStoryPoints(StoryPoints storyPoints) {
            this.StoryPoints = storyPoints;
            DomainEventPublisher.Instance.Publish(new BacklogItemStoryPointsAssigned(this.TenantId, this.BacklogItemId,
                storyPoints));
        }

        public Task GetTask(TaskId taskId) {
            return this._tasks.FirstOrDefault(x => x.TaskId.Equals(taskId));
        }

        private Task LoadTask(TaskId taskId) {
            Task task = this.GetTask(taskId);
            if(task==null) {
                throw new InvalidOperationException("Task does not exist.");
            }
            return task;
        }

        public void AssignTaskVolunteer(TaskId taskId, TeamMember volunteer) {
            Task task = this.LoadTask(taskId);
            task.AssignVolunteer(volunteer);
        }

        public void ChangeCategory(string category) {
            this.Category = category;
            DomainEventPublisher.Instance.Publish(new BacklogItemCategoryChanged(this.TenantId, this.BacklogItemId,
                category));
        }

        public void ChangeTaskStatus(TaskId taskId, TaskStatus status) {
            Task task = this.LoadTask(taskId);
            task.ChangeStatus(status);
        }

        public void ChangeType(BacklogItemType type) {
            this.Type = type;
            DomainEventPublisher.Instance.Publish(new BacklogItemTypeChanged(this.TenantId, this.BacklogItemId, type));
        }

        public bool IsScheduledForRelease {
            get { return this.ReleaseId != null; }
        }

        public bool IsCommittedToSprint {
            get { return this.SprintId != null; }
        }

        public void CommitTo(Sprint sprint) {
            AssertionConcern.NotNull(sprint, "Sprint must not be null.");
            AssertionConcern.Equals(sprint.TenantId, this.TenantId, "Sprint must be of same tenant.");
            AssertionConcern.Equals(sprint.ProductId, this.ProductId, "Sprint must be of same product.");

            if(!this.IsScheduledForRelease) {
                throw new InvalidOperationException("Must be scheduled for release to commit to sprint.");
            }

            if(this.IsCommittedToSprint) {
                if(!sprint.SprintId.Equals(this.SprintId)) {
                    UncommittFromSprint();
                }
            }

            if(this.Status==BacklogItemStatus.Scheduled) {
                this.Status = BacklogItemStatus.Committed;
            }

            this.SprintId = sprint.SprintId;

            DomainEventPublisher.Instance.Publish(new BacklogItemCommitted(this.TenantId, this.BacklogItemId,
                sprint.SprintId));
        }

        public void UncommittFromSprint() {
            if(!this.IsCommittedToSprint) {
                throw new InvalidOperationException("Not currently committed.");
            }

            this.Status = BacklogItemStatus.Scheduled;
            SprintId uncommittedSprintId = this.SprintId;
            this.SprintId = null;

            DomainEventPublisher.Instance.Publish(new BacklogItemUncommitted(this.TenantId, this.BacklogItemId,
                uncommittedSprintId));
        }

        public void DefineTask(TeamMember volunteer, string name, string description, int hoursRemaining) {
            Task task = new Task(this.TenantId, this.BacklogItemId, new TaskId(), volunteer, name, description,
                hoursRemaining, TaskStatus.NotStarted);
            this._tasks.Add(task);
            DomainEventPublisher.Instance.Publish(new TaskDefined(this.TenantId, this.BacklogItemId, task.TaskId,
                volunteer.TeamMemberId.Id, name, description));
        }

        public void DescribeTask(TaskId taskId, string description) {
            Task task = this.LoadTask(taskId);
            task.DescribeAs(description);
        }

        public string DiscussionInitiationId {
            get { return this._discussionInitiationId; }
            private set {
                if(value!=null) {
                    AssertionConcern.Length(value, 100, "Discussion initiation identity must be 100 characters or less.");
                }
                this._discussionInitiationId = value;
            }
        }

        public void FailDiscussionInitiation() {
            if(this.Discussion.Availability == DiscussionAvailability.Ready) {
                this._discussionInitiationId = null;
                this.Discussion = BacklogItemDiscussion.FromAvailability(DiscussionAvailability.Failed);
            }
        }

        public void InitiateDiscussion(DiscussionDescriptor descriptor) {
            AssertionConcern.NotNull(descriptor, "The descriptor must not be null.");
            if(this.Discussion.Availability==DiscussionAvailability.Requested) {
                this.Discussion = this.Discussion.NowReady(descriptor);
                DomainEventPublisher.Instance.Publish(new BacklogItemDiscussionInitiated(this.TenantId,
                    this.BacklogItemId, this.Discussion));
            }
        }

        public void InitiateDiscussion(BacklogItemDiscussion discussion) {
            this.Discussion = discussion;
            DomainEventPublisher.Instance.Publish(new BacklogItemDiscussionInitiated(this.TenantId,
                this.BacklogItemId, this.Discussion));
        }

        public int TotalTaskHoursRemaining {
            get { return this._tasks.Select(x => x.HoursRemaining).Sum(); }
        }

        public bool AnyTaskHoursRemaining {
            get { return this.TotalTaskHoursRemaining > 0; }
        }

        public void EstimateTaskHoursRemaining(TaskId taskId, int hoursRemaining) {
            Task task = this.LoadTask(taskId);
            task.EstimateHoursRemaining(hoursRemaining);

            BacklogItemStatus? changedStatus = default (BacklogItemStatus?);

            if(hoursRemaining==0) {
                if(!this.AnyTaskHoursRemaining) {
                    changedStatus = BacklogItemStatus.Done;
                }
            }
            else if(this.IsDone) {
                if(this.IsCommittedToSprint) {
                    changedStatus = BacklogItemStatus.Committed;
                }
                else if(this.IsScheduledForRelease) {
                    changedStatus = BacklogItemStatus.Scheduled;
                }
                else {
                    changedStatus = BacklogItemStatus.Planned;
                }
            }

            if(changedStatus != null) {
                this.Status = changedStatus.Value;
                DomainEventPublisher.Instance.Publish(new BacklogItemStatusChanged(this.TenantId, this.BacklogItemId,
                    changedStatus.Value));
            }
        }

        public void MarkAsRemoved() {
            if(this.IsRemoved) {
                throw new InvalidOperationException("Already removed, not outstanding.");
            }
            if(this.IsDone) {
                throw new InvalidOperationException("Already done, not outstanding.");
            }
            if(this.IsCommittedToSprint) {
                this.UncommittFromSprint();
            }
            if(this.IsScheduledForRelease) {
                this.UnscheduleFromRelease();
            }

            this.Status = BacklogItemStatus.Removed;

            DomainEventPublisher.Instance.Publish(new BacklogItemMarkedAsRemoved(this.TenantId, this.BacklogItemId));
        }

        public void UnscheduleFromRelease() {
            if(this.IsCommittedToSprint) {
                throw new InvalidOperationException("Must first uncommit.");
            }
            if(!this.IsScheduledForRelease) {
                throw new InvalidOperationException("Not scheduled for release.");
            }

            this.Status = BacklogItemStatus.Planned;
            ReleaseId unscheduledReleaseId = this.ReleaseId;
            this.ReleaseId = null;

            DomainEventPublisher.Instance.Publish(new BacklogItemUnscheduled(this.TenantId, this.BacklogItemId,
                unscheduledReleaseId));
        }

        public void RemoveTask(TaskId taskId) {
            Task task = this.LoadTask(taskId);

            if(!this._tasks.Remove(task)) {
                throw new InvalidOperationException("Task was not removed.");
            }

            DomainEventPublisher.Instance.Publish(new TaskRemoved(this.TenantId, this.BacklogItemId));
        }

        public void RenameTask(TaskId taskId, string name) {
            Task task = this.LoadTask(taskId);
            task.Rename(name);
        }

        public void RequestDiscussion(DiscussionAvailability availability) {
            if(this.Discussion.Availability!=DiscussionAvailability.Ready) {
                this.Discussion = BacklogItemDiscussion.FromAvailability(availability);

                DomainEventPublisher.Instance.Publish(new BacklogItemDiscussionRequested(this.TenantId, this.ProductId,
                    this.BacklogItemId, availability == DiscussionAvailability.Requested));
            }
        }

        public void ScheduleFor(Release release) {
            AssertionConcern.NotNull(release, "Release must not be null.");
            AssertionConcern.Equals(this.TenantId, release.TenantId, "Release must be of same tenant.");
            AssertionConcern.Equals(this.ProductId, release.ProductId, "Release must be of same product.");

            if(this.IsScheduledForRelease && !this.ReleaseId.Equals(release.ReleaseId)) {
                this.UnscheduleFromRelease();
            }

            if(this.Status==BacklogItemStatus.Planned) {
                this.Status = BacklogItemStatus.Scheduled;
            }

            this.ReleaseId = release.ReleaseId;

            DomainEventPublisher.Instance.Publish(new BacklogItemScheduled(this.TenantId, this.BacklogItemId,
                release.ReleaseId));
        }

        public void StartDiscussionInitiation(string discussionInitiationId) {
            if(this.Discussion.Availability!=DiscussionAvailability.Ready) {
                this.DiscussionInitiationId = discussionInitiationId;
            }
        }

        public void Summarize(string summary) {
            this.Summary = summary;
            DomainEventPublisher.Instance.Publish(new BacklogItemSummarized(this.TenantId, this.BacklogItemId, summary));
        }

        public void TellStory(string story) {
            if(story!=null) {
                AssertionConcern.Length(story, 65000, "The story must be 65000 characters or less.");
            }

            this.Story = story;

            DomainEventPublisher.Instance.Publish(new BacklogItemStoryTold(this.TenantId, this.BacklogItemId, story));
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.ProductId;
            yield return this.BacklogItemId;
        }
    }
}