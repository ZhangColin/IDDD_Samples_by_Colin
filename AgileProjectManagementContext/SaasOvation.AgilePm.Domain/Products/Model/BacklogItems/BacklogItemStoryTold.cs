﻿using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemStoryTold: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public string Story { get; private set; }

        public BacklogItemStoryTold(TenantId tenantId, BacklogItemId backlogItemId, string story) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.Story = story;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}