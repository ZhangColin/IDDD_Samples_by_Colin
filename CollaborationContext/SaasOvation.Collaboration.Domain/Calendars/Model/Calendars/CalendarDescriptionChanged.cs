using System;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Calendars.Model.Calendars {
    public class CalendarDescriptionChanged : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public CalendarId CalendarId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public CalendarDescriptionChanged(TenantId tenantId, CalendarId calendarId, string name, string description) {
            this.TenantId = tenantId;
            this.CalendarId = calendarId;
            this.Name = name;
            this.Description = description;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}