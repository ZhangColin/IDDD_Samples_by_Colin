using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using SaasOvation.Collaboration.Application.Calendars.Data;
using SaasOvation.Common.Port.Adapter.Persistence;

namespace SaasOvation.Collaboration.Application.Calendars {
    public class CalendarQueryService: AbstractQueryService {
        public CalendarQueryService(string connectionString): base(connectionString) {}

        public IList<CalendarData> GetAllCalendarsDataByTenant(string tenantId) {
            using(DbConnection connection = this.GetConnection()) {
                Dictionary<string, CalendarData> lookup = new Dictionary<string, CalendarData>();
                connection.Query<CalendarData, CalendarSharerData, CalendarData>(
                    @"select cal.CalendarId, cal.Description, cal.Name, cal.OwnerEmailAddress, cal.OwnerIdentity, cal.OwnerName, cal.TenantId,
	                    sharer.CalendarId, sharer.ParticipantEmailAddress, sharer.ParticipantIdentity, sharer.ParticipantName, sharer.TenantId 
                    from dbo.VwCalendars as cal 
	                    left join VwCalendarSharers as sharer on cal.CalendarId = sharer.CalendarId
                    where cal.TenantId=@tenantId",
                    (cal, sharer) => {
                        CalendarData calendarData;
                        if(!lookup.TryGetValue(cal.CalendarId, out calendarData)) {
                            lookup.Add(cal.CalendarId, calendarData = cal);
                        }
                        if(calendarData.Sharers==null) {
                            calendarData.Sharers = new HashSet<CalendarSharerData>();
                        }
                        calendarData.Sharers.Add(sharer);
                        return calendarData;
                    }, new {tenantId}, splitOn: "CalendarId");

                return lookup.Values.ToList();
            }
        }

        public CalendarData GetCalendarDataById(string tenantId, string calendarId) {
            using (DbConnection connection = this.GetConnection()) {
                CalendarData calendarData = null;
                connection.Query<CalendarData, CalendarSharerData, CalendarData>(
                    @"select cal.CalendarId, cal.Description, cal.Name, cal.OwnerEmailAddress, cal.OwnerIdentity, cal.OwnerName, cal.TenantId,
	                    sharer.CalendarId, sharer.ParticipantEmailAddress, sharer.ParticipantIdentity, sharer.ParticipantName, sharer.TenantId 
                    from dbo.VwCalendars as cal 
	                    left join VwCalendarSharers as sharer on cal.CalendarId = sharer.CalendarId
                    where cal.TenantId=@tenantId and cal.CalendarId=WcalendarId",
                    (cal, sharer) => {
                        if (calendarData == null) {
                            calendarData = cal;
                            cal.Sharers = new HashSet<CalendarSharerData>();
                        }
                        calendarData.Sharers.Add(sharer);
                        return calendarData;
                    }, new { tenantId }, splitOn : "CalendarId");

                return calendarData;
            }
        }
    }
}