using System;
using System.Collections.Generic;
using System.Linq;
using SaasOvation.AgilePm.Domain.Teams.Model;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class Task: Entity {
        public TenantId TenantId { get; private set; }
        internal BacklogItemId BacklogItemId { get; private set; }
        internal TaskId TaskId { get; private set; }
        public TeamMemberId Volunteer { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        internal int HoursRemaining { get; private set; }
        public TaskStatus Status { get; private set; }

        private readonly List<EstimationLogEntry> _estimationLog; 

        public Task(TenantId tenantId, BacklogItemId backlogItemId, TaskId taskId, TeamMember teamMember, string name,
            string description, int hoursRemaining, TaskStatus status) {
            AssertionConcern.NotEmpty(name, "Name is required.");
            AssertionConcern.Length(name, 100, "Name must be 100 characters or less");
            AssertionConcern.NotEmpty(description, "Description is required.");
            AssertionConcern.Length(description, 65000, "Description must be 65000 characters or less.");
            AssertionConcern.NotNull(teamMember, "The team member must be provided");
            AssertionConcern.Equals(tenantId, teamMember.TeamMemberId.TenantId,
                "The volunteer must be of the same tenant.");

            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.TaskId = taskId;
            this.Volunteer = teamMember.TeamMemberId;
            this.Name = name;
            this.Description = description;
            this.HoursRemaining = hoursRemaining;
            this.Status = status;
        }

        internal void AssignVolunteer(TeamMember teamMember) {
            AssertionConcern.NotNull(teamMember, "A valunteer must be provided.");
            this.Volunteer = teamMember.TeamMemberId;
            DomainEventPublisher.Instance.Publish(new TaskVolunteerAssigned(this.TenantId, this.BacklogItemId,
                this.TaskId, teamMember.TeamMemberId.Id));
        }

        internal void ChangeStatus(TaskStatus status) {
            this.Status = status;
            DomainEventPublisher.Instance.Publish(new TaskStatusChanged(this.TenantId, this.BacklogItemId, this.TaskId,
                status));
        }

        internal void DescribeAs(string description) {
            this.Description = description;
            DomainEventPublisher.Instance.Publish(new TaskDescribed(this.TenantId, this.BacklogItemId, this.TaskId,
                description));
        }

        internal void EstimateHoursRemaining(int hoursRemaining) {
            if(hoursRemaining<0) {
                throw new ArgumentOutOfRangeException("hoursRemaining");
            }
            if(this.HoursRemaining!=hoursRemaining) {
                this.HoursRemaining = hoursRemaining;
                DomainEventPublisher.Instance.Publish(new TaskHoursRemainingEstimated(this.TenantId, this.BacklogItemId,
                    this.TaskId, hoursRemaining));

                if(hoursRemaining==0 && this.Status!=TaskStatus.Done) {
                    this.ChangeStatus(TaskStatus.Done);
                }
                else if(hoursRemaining>0 && this.Status!=TaskStatus.InProgress) {
                    this.ChangeStatus(TaskStatus.InProgress);
                }

                LogEstimation(hoursRemaining);
            }
        }

        private void LogEstimation(int hoursRemaining) {
            DateTime today = EstimationLogEntry.CurrentLogDate;
            bool updatedLogForToday =
                this._estimationLog.Any(entry => entry.UpdateHoursRemainingWhenDateMatches(hoursRemaining, today));
            if(updatedLogForToday) {
                this._estimationLog.Add(new EstimationLogEntry(this.TenantId, this.TaskId, today, hoursRemaining));
            }
        }

        internal void Rename(string name) {
            this.Name = name;
            DomainEventPublisher.Instance.Publish(new TaskRenamed(this.TenantId, this.BacklogItemId, this.TaskId, name));
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.BacklogItemId;
            yield return this.TaskId;
        }
    }
}