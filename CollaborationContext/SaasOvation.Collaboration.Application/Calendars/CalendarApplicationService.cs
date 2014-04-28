using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using SaasOvation.Collaboration.Application.Calendars.Data;
using SaasOvation.Collaboration.Domain.Calendars.Model;
using SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries;
using SaasOvation.Collaboration.Domain.Calendars.Model.Calendars;
using SaasOvation.Collaboration.Domain.Calendars.Repository;
using SaasOvation.Collaboration.Domain.Calendars.Service;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Application.Calendars {
    public class CalendarApplicationService {
        private readonly ICalendarRepository _calendarRepository;
        private readonly ICalendarEntryRepository _calendarEntryRepository;
        private readonly CalendarIdentityService _calendarIdentityService;
        private readonly ICollaboratorService _collaboratorService;

        public CalendarApplicationService(ICalendarRepository calendarRepository,
            ICalendarEntryRepository calendarEntryRepository, CalendarIdentityService calendarIdentityService,
            ICollaboratorService collaboratorService) {
            this._calendarRepository = calendarRepository;
            this._calendarEntryRepository = calendarEntryRepository;
            this._calendarIdentityService = calendarIdentityService;
            this._collaboratorService = collaboratorService;
        }

        public void ChangeCalendarDescription(string tenantId, string calendarId, string description) {
            Calendar calendar = this._calendarRepository.Get(new TenantId(tenantId), new CalendarId(calendarId));
            calendar.ChangeDescription(description);
            this._calendarRepository.Save(calendar);
        }

        public void CreateCalendar(string tenantId, string name, string description, string ownerId,
            ISet<string> participantsToShareWith, ICalendarCommandResult calendarCommandResult) {
            Owner owner = this._collaboratorService.GetOwnerFrom(new TenantId(tenantId), ownerId);
            IEnumerable<CalendarSharer> sharers = this.GetSharesFrom(new TenantId(tenantId), participantsToShareWith);

            Calendar calendar = new Calendar(new TenantId(tenantId), this._calendarRepository.GetNextIdentity(), name, description,
                owner, sharers);
            this._calendarRepository.Save(calendar);

            calendarCommandResult.SetResultingCalendarId(calendar.CalendarId.Id);
        }

        public void RenameCalendar(string tenantId, string calendarId, string name) {
            Calendar calendar = this._calendarRepository.Get(new TenantId(tenantId), new CalendarId(calendarId));
            calendar.Rename(name);
            this._calendarRepository.Save(calendar);
        }

        public void ScheduleCalendarEntry(string tenantId, string calendarId, string description, string location,
            string ownerId, DateTime timeSpanBegins, DateTime timeSpanEnds, string repeatType, DateTime repeatEndsOn,
            string alarmType, int alarmUnits, IEnumerable<string> participantsToInvite,
            ICalendarCommandResult calendarCommandResult) {
            Calendar calendar = this._calendarRepository.Get(new TenantId(tenantId), new CalendarId(calendarId));
            CalendarEntry calendarEntry = calendar.ScheduleCalendarEntry(this._calendarIdentityService, description,
                location, this._collaboratorService.GetOwnerFrom(new TenantId(tenantId), ownerId),
                new DateRange(timeSpanBegins, timeSpanEnds),
                new Repetition((RepeatType)Enum.Parse(typeof(RepeatType), repeatType), repeatEndsOn),
                new Alarm((AlarmunitsType)Enum.Parse(typeof(AlarmunitsType), alarmType), alarmUnits),
                GetInviteesFrom(new TenantId(tenantId), participantsToInvite));
            this._calendarEntryRepository.Save(calendarEntry);

            calendarCommandResult.SetResultingCalendarId(calendar.CalendarId.Id);
            calendarCommandResult.SetResultingCalendarEntryId(calendarEntry.CalendarEntryId.Id);
        }

        public void ShareCalendarWith(string tenantId, string calendarId, IEnumerable<string> participantsToShareWith) {
            Calendar calendar = this._calendarRepository.Get(new TenantId(tenantId), new CalendarId(calendarId));
            foreach(CalendarSharer sharer in this.GetSharesFrom(new TenantId(tenantId), participantsToShareWith)) {
                calendar.ShareCalendarWith(sharer);
            }

            this._calendarRepository.Save(calendar);
        }
        
        public void UnshareCalendarWith(string tenantId, string calendarId, IEnumerable<string> participantsToShareWith) {
            Calendar calendar = this._calendarRepository.Get(new TenantId(tenantId), new CalendarId(calendarId));
            foreach(CalendarSharer sharer in this.GetSharesFrom(new TenantId(tenantId), participantsToShareWith)) {
                calendar.UnshareCalendarWith(sharer);
            }

            this._calendarRepository.Save(calendar);
        }

        private IEnumerable<Participant> GetInviteesFrom(TenantId tenantId, IEnumerable<string> participantsToInvite) {
            HashSet<Participant> invitees = new HashSet<Participant>();
            foreach(string participantId in participantsToInvite) {
                Participant participant = this._collaboratorService.GetParticipantFrom(tenantId, participantId);
                invitees.Add(participant);
            }
            return invitees;
        }

        private IEnumerable<CalendarSharer> GetSharesFrom(TenantId tenantId, IEnumerable<string> participantsToShareWith) {
            HashSet<CalendarSharer> sharers = new HashSet<CalendarSharer>();
            foreach(string participantId in participantsToShareWith) {
                Participant participant = this._collaboratorService.GetParticipantFrom(tenantId, participantId);
                sharers.Add(new CalendarSharer(participant));
            }
            return sharers;
        }
    }
}