using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SaasOvation.AgilePm.Domain.Products.Model.BacklogItems;
using SaasOvation.AgilePm.Domain.Products.Model.Products;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Sprints {
    public class Sprint: Entity {
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public SprintId SprintId { get; private set; }
        public string Name { get; private set; }
        public string Goals { get; private set; }
        public DateTime Begins { get; private set; }
        public DateTime Ends { get; private set; }
        public string Retrospective { get; private set; }

        private readonly ISet<CommittedBacklogItem> _backlogItems; 

        public Sprint(TenantId tenantId, ProductId productId, SprintId sprintId, string name, string goals,
            DateTime begins, DateTime ends) {
            if(ends.Ticks<begins.Ticks) {
                throw new InvalidOperationException("Sprint must not end before it begins.");
            }
            this.TenantId = tenantId;
            this.ProductId = productId;
            this.SprintId = sprintId;
            this.Name = name;
            this.Goals = goals;
            this.Begins = begins;
            this.Ends = ends;

            _backlogItems = new HashSet<CommittedBacklogItem>();
        }

        public ICollection<CommittedBacklogItem> AllCommittedBacklogItems() {
            return new ReadOnlyCollection<CommittedBacklogItem>(this._backlogItems.ToArray());
        }

        public void AdjustGoals(string goals) {
            this.Goals = goals;

            // TODO: publish event / student assignment
        }

        public void CaptureRetrospectiveMeetinResults(string retrospective) {
            this.Retrospective = retrospective;

            // TODO: publish event / student assignment
        }

        public void Commit(BacklogItem backlogItem) {
            int ordering = this._backlogItems.Count + 1;
            this._backlogItems.Add(new CommittedBacklogItem(this.TenantId, this.SprintId, backlogItem.BacklogItemId,
                ordering));
        }

        public void NowBeginsOn(DateTime dateTime) {
            this.Begins = dateTime;
        }

        public void NowEndsOn(DateTime dateTime) {
            this.Ends = dateTime;
        }

        public void Rename(string name) {
            this.Name = name;

            // TODO: publish event
        }

        public void ReOrderFrom(BacklogItemId id, int orderOfPriority) {
            foreach(CommittedBacklogItem committedBacklogItem in this._backlogItems) {
                committedBacklogItem.ReOrderFrom(id, orderOfPriority);
            }
        }

        public void UnCommit(BacklogItem backlogItem) {
            CommittedBacklogItem committedBacklogItem = new CommittedBacklogItem(this.TenantId, this.SprintId,
                backlogItem.BacklogItemId);

            this._backlogItems.Remove(committedBacklogItem);
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.ProductId;
            yield return this.SprintId;
        }
    }
}