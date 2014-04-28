namespace SaasOvation.Collaboration.Application.Calendars.Data {
    public interface ICalendarCommandResult {
        void SetResultingCalendarId(string calendarId);
        void SetResultingCalendarEntryId(string calendarEntryId);
    }
}