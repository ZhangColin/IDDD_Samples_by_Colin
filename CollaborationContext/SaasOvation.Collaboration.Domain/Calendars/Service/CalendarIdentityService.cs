using SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries;
using SaasOvation.Collaboration.Domain.Calendars.Model.Calendars;
using SaasOvation.Collaboration.Domain.Calendars.Repository;

namespace SaasOvation.Collaboration.Domain.Calendars.Service {
    public class CalendarIdentityService {
        private readonly ICalendarRepository _calendarRepository;
        private readonly ICalendarEntryRepository _calendarEntryRepository;

        public CalendarIdentityService(ICalendarRepository calendarRepository, ICalendarEntryRepository calendarEntryRepository) {
            this._calendarRepository = calendarRepository;
            this._calendarEntryRepository = calendarEntryRepository;
        }

        public CalendarId GetNextCalendarId() {
            return this._calendarRepository.GetNextIdentity();
        }

        public CalendarEntryId GetNextCalendarEntryId() {
            return this._calendarEntryRepository.GetNextIdentity();
        }
    }
}