using System;
using System.Collections.Generic;
using SaasOvation.Collaboration.Domain.Calendars.Model;
using SaasOvation.Collaboration.Domain.Calendars.Model.CalendarEntries;
using SaasOvation.Collaboration.Domain.Calendars.Repository;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Application.Calendars {
    public class CalendarEntryApplicationService {
        private readonly ICalendarEntryRepository _calendarEntryRepository;
        private readonly ICollaboratorService _collaboratorService;

        public CalendarEntryApplicationService(ICalendarEntryRepository calendarEntryRepository,
            ICollaboratorService collaboratorService) {
            this._calendarEntryRepository = calendarEntryRepository;
            this._collaboratorService = collaboratorService;
        }

        public void ChangeCalendarEntryDescription(string tenantId, string calendarEntryId, string description) {
            CalendarEntry calendarEntry = this._calendarEntryRepository.Get(new TenantId(tenantId),
                new CalendarEntryId(calendarEntryId));
            calendarEntry.ChangeDescription(description);
            this._calendarEntryRepository.Save(calendarEntry);
        }

        public void InviteCalendarEntryParticipant(string tenantId, string calendarEntryId,
            IEnumerable<string> participantsToInvite) {
            CalendarEntry calendarEntry = this._calendarEntryRepository.Get(new TenantId(tenantId),
                new CalendarEntryId(calendarEntryId));

            foreach(Participant participant in GetInviteesFrom(new TenantId(tenantId), participantsToInvite)) {
                calendarEntry.Invite(participant);
            }

            this._calendarEntryRepository.Save(calendarEntry);
        }

        public void RelocateCalendarEntry(string tenantId, string calendarEntryId, string location) {
            CalendarEntry calendarEntry = this._calendarEntryRepository.Get(new TenantId(tenantId),
                new CalendarEntryId(calendarEntryId));
            calendarEntry.Relocate(location);
            this._calendarEntryRepository.Save(calendarEntry);
        }

        public void RescheduleCalendarEntry(string tenantId, string calendarEntryId, string description, string location,
            DateTime timeSpanBegins, DateTime timeSpanEnds, string repeatType, DateTime repeatEndsOn, string alarmType, 
            int alarmUnits) {
            CalendarEntry calendarEntry = this._calendarEntryRepository.Get(new TenantId(tenantId),
                new CalendarEntryId(calendarEntryId));
            calendarEntry.Reshedule(description, location, new DateRange(timeSpanBegins, timeSpanEnds),
                new Repetition((RepeatType)Enum.Parse(typeof(RepeatType), repeatType), repeatEndsOn),
                new Alarm((AlarmunitsType)Enum.Parse(typeof(AlarmunitsType), alarmType), alarmUnits));
            this._calendarEntryRepository.Save(calendarEntry);
        }

        public void UninviteCalendarEntryParticipant(string tenantId, string calendarEntryId,
            IEnumerable<string> participantsToUninvite) {
            CalendarEntry calendarEntry = this._calendarEntryRepository.Get(new TenantId(tenantId),
                new CalendarEntryId(calendarEntryId));

            foreach (Participant participant in GetInviteesFrom(new TenantId(tenantId), participantsToUninvite)) {
                calendarEntry.Uninvite(participant);
            }

            this._calendarEntryRepository.Save(calendarEntry);
        }

        private IEnumerable<Participant> GetInviteesFrom(TenantId tenantId, IEnumerable<string> participantsToInvite) {
            HashSet<Participant> invitees = new HashSet<Participant>();
            foreach(string participantId in participantsToInvite) {
                Participant participant = this._collaboratorService.GetParticipantFrom(tenantId, participantId);
                invitees.Add(participant);
            }
            return invitees;
        }
    }
}