using System;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class TaskId: Identity {
        public TaskId() : base(Guid.NewGuid().ToString().ToUpper().Substring(0, 8)) { }
        public TaskId(string id): base(id) {}

        protected override void ValidateId(string id) {
            AssertionConcern.NotEmpty(id, "The id must be provided.");
            AssertionConcern.Length(id, 8, "The id must be 8 characters or less.");
        }
    }
}