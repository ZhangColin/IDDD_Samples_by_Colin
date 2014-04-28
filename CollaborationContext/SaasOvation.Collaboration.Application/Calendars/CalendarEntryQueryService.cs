using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using SaasOvation.Collaboration.Application.Calendars.Data;
using SaasOvation.Common.Port.Adapter.Persistence;

namespace SaasOvation.Collaboration.Application.Calendars {
    public class CalendarEntryQueryService: AbstractQueryService {
        public CalendarEntryQueryService(string connectionString): base(connectionString) {}

        public IList<CalendarEntryData> GetCalendarEntryDataByCalendarId(string tenantId, string calendarId) {
            using (DbConnection connection = this.GetConnection()) {
                Dictionary<string, CalendarEntryData> lookup = new Dictionary<string, CalendarEntryData>();
                connection.Query<CalendarEntryData, CalendarEntryInviteeData, CalendarEntryData>(
                    @"seselect entry.CalendarEntryId, entry.CalendarId, entry.TenantId, entry.AlarmUnits, entry.AlarmUnitsType, 
	                    entry.Description, entry.Location, entry.OwnerEmailAddress, entry.OwnerIdentity, entry.OwnerName,
	                    entry.RepetitionEnds, entry.RepetitionType, entry.TimeSpanBegins, entry.TimeSpanEnds,
	                    invitee.CalendarEntryId, invitee.ParticipantEmailAddress, invitee.ParticipantIdentity, invitee.ParticipantName,
	                    invitee.TenantId 
                    from dbo.VwCalendarEntries as entry left join dbo.VwCalendarEntryInvitees as invitee on entry.CalendarEntryId=invitee.CalendarEntryId
                    where entry.TenantId=@tenantId and entry.CalendarId=@calendarId",
                    (entry, invitee) => {
                        CalendarEntryData calendarEntryData;
                        if (!lookup.TryGetValue(entry.CalendarEntryId, out calendarEntryData)) {
                            lookup.Add(entry.CalendarEntryId, calendarEntryData = entry);
                        }
                        if (calendarEntryData.Invitees == null) {
                            calendarEntryData.Invitees = new HashSet<CalendarEntryInviteeData>();
                        }
                        calendarEntryData.Invitees.Add(invitee);
                        return calendarEntryData;
                    }, new { tenantId, calendarId }, splitOn : "CalendarEntryId");

                return lookup.Values.ToList();
            }
        }
        
        public IList<CalendarEntryData> GetCalendarEntriesSpanning(string tenantId, string calendarId, DateTime beginsOn, DateTime endsOn) {
            using (DbConnection connection = this.GetConnection()) {
                Dictionary<string, CalendarEntryData> lookup = new Dictionary<string, CalendarEntryData>();
                connection.Query<CalendarEntryData, CalendarEntryInviteeData, CalendarEntryData>(
                    @"seselect entry.CalendarEntryId, entry.CalendarId, entry.TenantId, entry.AlarmUnits, entry.AlarmUnitsType, 
	                    entry.Description, entry.Location, entry.OwnerEmailAddress, entry.OwnerIdentity, entry.OwnerName,
	                    entry.RepetitionEnds, entry.RepetitionType, entry.TimeSpanBegins, entry.TimeSpanEnds,
	                    invitee.CalendarEntryId, invitee.ParticipantEmailAddress, invitee.ParticipantIdentity, invitee.ParticipantName,
	                    invitee.TenantId 
                    from dbo.VwCalendarEntries as entry left join dbo.VwCalendarEntryInvitees as invitee on entry.CalendarEntryId=invitee.CalendarEntryId
                    where entry.TenantId=@tenantId and entry.CalendarId=@calendarId and 
                        ((entry.TimeSpanBegins >= @beginsOn and entry.TimeSpanBegins <= @endsOn) or (entry.RepetitionEnds >= @beginsOn and entry.RepetitionEnds <= @endsOn))",
                    (entry, invitee) => {
                        CalendarEntryData calendarEntryData;
                        if (!lookup.TryGetValue(entry.CalendarEntryId, out calendarEntryData)) {
                            lookup.Add(entry.CalendarEntryId, calendarEntryData = entry);
                        }
                        if (calendarEntryData.Invitees == null) {
                            calendarEntryData.Invitees = new HashSet<CalendarEntryInviteeData>();
                        }
                        calendarEntryData.Invitees.Add(invitee);
                        return calendarEntryData;
                    }, new { tenantId, calendarId, beginsOn, endsOn }, splitOn : "CalendarEntryId");

                return lookup.Values.ToList();
            }
        }

        public CalendarEntryData GetCalendarEntryDataById(string tenantId, string calendarEntryId) {
            using (DbConnection connection = this.GetConnection()) {
                CalendarEntryData calendarEntryData = null;
                connection.Query<CalendarEntryData, CalendarEntryInviteeData, CalendarEntryData>(
                    @"select entry.CalendarEntryId, entry.CalendarId, entry.TenantId, entry.AlarmUnits, entry.AlarmUnitsType, 
	                    entry.Description, entry.Location, entry.OwnerEmailAddress, entry.OwnerIdentity, entry.OwnerName,
	                    entry.RepetitionEnds, entry.RepetitionType, entry.TimeSpanBegins, entry.TimeSpanEnds,
	                    invitee.CalendarEntryId, invitee.ParticipantEmailAddress, invitee.ParticipantIdentity, invitee.ParticipantName,
	                    invitee.TenantId 
                    from dbo.VwCalendarEntries as entry left join dbo.VwCalendarEntryInvitees as invitee on entry.CalendarEntryId=invitee.CalendarEntryId
                    where entry.TenantId=@tenantId and entry.CalendarEntryId=@calendarEntryId",
                    (entry, invitee) => {
                        if (calendarEntryData == null) {
                            calendarEntryData = entry;
                            calendarEntryData.Invitees = new HashSet<CalendarEntryInviteeData>();
                        }
                        calendarEntryData.Invitees.Add(invitee);
                        return calendarEntryData;
                    }, new { tenantId, calendarEntryId }, splitOn : "CalendarEntryId");

                return calendarEntryData;
            }
        }
    }
}