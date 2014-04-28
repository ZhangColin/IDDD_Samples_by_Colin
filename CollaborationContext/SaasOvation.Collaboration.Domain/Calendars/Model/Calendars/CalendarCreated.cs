using System;
using System.Collections.Generic;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.Calendars {
    public class CalendarCreated :IDomainEvent{
        public TenantId TenantId { get; private set; }
        public CalendarId CalendarId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Owner Owner { get; private set; }
        public IEnumerable<CalendarSharer> SharedWith { get; private set; }

        public CalendarCreated(TenantId tenantId, CalendarId calendarId, string name, string description, Owner owner, IEnumerable<CalendarSharer> sharedWith) {
            this.TenantId = tenantId;
            this.CalendarId = calendarId;
            this.Name = name;
            this.Description = description;
            this.Owner = owner;
            this.SharedWith = sharedWith;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}