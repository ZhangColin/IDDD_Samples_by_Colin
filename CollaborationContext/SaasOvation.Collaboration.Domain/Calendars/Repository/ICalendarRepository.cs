using SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries;
using SaasOvation.Collaboration.Domain.Calendars.Model.Calendars;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Domain.Calendars.Repository {
    public interface ICalendarEntryRepository {
        CalendarEntry Get(TenantId tenantId, CalendarEntryId calendarEntryId);
        CalendarEntryId GetNextIdentity();
        void Save(CalendarEntry calendarEntry);
    }
}