using System;
using System.Collections.Generic;

namespace MedicalCharting.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PhysicianId { get; set; }
        public int PatientId { get; set; }

        public DateTime Start { get; set; }
        public int DurationMinutes { get; set; }
        public string Room { get; set; } = "";

        // A-level additions
        public List<string> Diagnoses { get; set; } = new();
        public List<Treatment> Treatments { get; set; } = new();

        public DateTime End => Start.AddMinutes(DurationMinutes);
    }
}
