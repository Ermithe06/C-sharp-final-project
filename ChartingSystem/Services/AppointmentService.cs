using MedicalCharting.Models;
using System;
using System.Linq;

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
           
            if (appt.Start.Hour < 8 || appt.End.Hour > 17)
            {
                message = "Appointments must be between 8 AM and 5 PM.";
                return false;
            }

         
            if (appt.Start.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                message = "Appointments may only occur Monday–Friday.";
                return false;
            }

            var physicianConflict = _store.Appointments.Any(a =>
                a.Id != appt.Id &&
                a.PhysicianId == appt.PhysicianId &&
                a.Room == appt.Room &&
                a.Start < appt.End &&
                appt.Start < a.End
            );

            if (physicianConflict)
            {
                message = "Physician is already booked at this time.";
                return false;
            }

         
            var roomConflict = _store.Appointments.Any(a =>
                a.Id != appt.Id &&
                a.Room == appt.Room &&
                a.Start < appt.End &&
                appt.Start < a.End
            );

            if (roomConflict)
            {
                message = "Room is already being used at this time.";
                return false;
            }

            message = "OK";
            return true;
        }
    }
}
