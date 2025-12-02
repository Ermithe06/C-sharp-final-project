using System;
using System.Linq;
using MedicalCharting.Models;

namespace MedicalCharting.Services
{
    public class AppointmentService
    {
        private readonly DataStore _store;
        private readonly TimeSpan _open = TimeSpan.FromHours(8);  // 8:00
        private readonly TimeSpan _close = TimeSpan.FromHours(17); // 17:00 (5pm)

        public AppointmentService(DataStore store) => _store = store;

        // returns true if scheduling succeeded
        public bool TrySchedule(Appointment appt, out string message)
        {
            // Validate day: only Mon-Fri
            var dow = appt.Start.DayOfWeek;
            if (dow == DayOfWeek.Saturday || dow == DayOfWeek.Sunday)
            {
                message = "Appointments allowed only Monday through Friday.";
                return false;
            }

            var startTime = appt.Start.TimeOfDay;
            var endTime = appt.End.TimeOfDay;

            // Ensure appointment fits fully between 8:00 and 17:00
            if (startTime < _open || endTime > _close)
            {
                message = "Appointment must be between 08:00 and 17:00 (end time <= 17:00).";
                return false;
            }

            // Check physician exists
            if (!_store.Physicians.Any(p => p.Id == appt.PhysicianId))
            {
                message = "Physician not found.";
                return false;
            }

            if (!_store.Patients.Any(p => p.Id == appt.PatientId))
            {
                message = "Patient not found.";
                return false;
            }

            // Check double booking: for same physician, check overlap
            var physicianAppts = _store.Appointments.Where(a => a.PhysicianId == appt.PhysicianId);
            foreach (var existing in physicianAppts)
            {
                if (appt.Start < existing.End && existing.Start < appt.End)
                {
                    message = $"Conflict with appointment #{existing.Id} ({existing.Start:yyyy-MM-dd HH:mm} - {existing.End:HH:mm}).";
                    return false;
                }
            }

            // Passed all checks; assign id and save
            appt.Id = (_store.Appointments.Any() ? _store.Appointments.Max(a => a.Id) : 0) + 1;
            _store.Appointments.Add(appt);
            message = "Appointment scheduled.";
            return true;
        }
    }
}

