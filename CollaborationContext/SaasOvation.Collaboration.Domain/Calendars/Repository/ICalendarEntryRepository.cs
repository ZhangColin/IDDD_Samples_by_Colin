using SaasOvation.Collaboration.Domain.Calendars.Model.Calendars;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Domain.Calendars.Repository {
    public interface ICalendarRepository {
        Calendar Get(TenantId tenantId, CalendarId calendarId);
        CalendarId GetNextIdentity();
        void Save(Calendar calendar);
    }
}