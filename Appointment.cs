using System;
using System.Text.Json.Serialization;

namespace MedicalCharting.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PhysicianId { get; set; }
        public int PatientId { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(30);

        [JsonIgnore]
        public DateTime End => Start + Duration;

        public override string ToString()
            => $"Appt #{Id}: Physician {PhysicianId}, Patient {PatientId}, {Start:yyyy-MM-dd HH:mm} - {End:HH:mm}";
    }
}

