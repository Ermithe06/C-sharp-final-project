using System;
using System.Linq;
using MedicalCharting.Models;

namespace MedicalCharting.Services
{
    public class AppointmentService
    {
        private readonly DataStore _store;

        public AppointmentService(DataStore store)
        {
            _store = store;
        }

        public bool TrySchedule(Appointment appt, out string message)
        {
            message = "";

            // 1️⃣ Validate weekday
            if (appt.Start.DayOfWeek == DayOfWeek.Saturday ||
                appt.Start.DayOfWeek == DayOfWeek.Sunday)
            {
                message = "Appointments must be scheduled Monday–Friday.";
                return false;
            }

            // 2️⃣ Validate business hours (8AM–5PM)
            TimeSpan open = TimeSpan.FromHours(8);
            TimeSpan close = TimeSpan.FromHours(17);

            if (appt.Start.TimeOfDay < open || appt.End.TimeOfDay > close)
            {
                message = "Appointments must be between 8 AM and 5 PM.";
                return false;
            }

            // 3️⃣ Check physician double-booking
            bool physicianConflict = _store.Appointments.Any(a =>
                a.Id != appt.Id &&
                a.PhysicianId == appt.PhysicianId &&
                TimesOverlap(a.Start, a.End, appt.Start, appt.End));

            if (physicianConflict)
            {
                message = "This physician already has an appointment at that time.";
                return false;
            }

            // 4️⃣ Check room double-booking (Assignment A requirement)
            bool roomConflict = _store.Appointments.Any(a =>
                a.Id != appt.Id &&
                a.Room == appt.Room &&
                TimesOverlap(a.Start, a.End, appt.Start, appt.End));

            if (roomConflict)
            {
                message = "This room is already booked at that time.";
                return false;
            }

            // 5️⃣ Check patient double-booking (optional but smart)
            bool patientConflict = _store.Appointments.Any(a =>
                a.Id != appt.Id &&
                a.PatientId == appt.PatientId &&
                TimesOverlap(a.Start, a.End, appt.Start, appt.End));

            if (patientConflict)
            {
                message = "This patient already has an appointment at that time.";
                return false;
            }

            // Everything OK
            return true;
        }

        private bool TimesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return start1 < end2 && start2 < end1;
        }
    }
}
