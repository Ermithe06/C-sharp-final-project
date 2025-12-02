using System.IO;
using System.Text.Json;
using MedicalCharting.Models;

namespace MedicalCharting.Services
{
    public class DataStore
    {
        public List<Patient> Patients { get; set; } = new();
        public List<Physician> Physicians { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
    }

    public static class Persistence
    {
        private static JsonSerializerOptions _opts = new JsonSerializerOptions { WriteIndented = true };

        public static void Save(DataStore store, string folder = "data")
        {
            Directory.CreateDirectory(folder);
            File.WriteAllText(Path.Combine(folder, "patients.json"), JsonSerializer.Serialize(store.Patients, _opts));
            File.WriteAllText(Path.Combine(folder, "physicians.json"), JsonSerializer.Serialize(store.Physicians, _opts));
            File.WriteAllText(Path.Combine(folder, "appointments.json"), JsonSerializer.Serialize(store.Appointments, _opts));
        }

        public static DataStore Load(string folder = "data")
        {
            var ds = new DataStore();
            if (!Directory.Exists(folder)) return ds;

            string Read<T>(string file) where T : new()
            {
                var p = Path.Combine(folder, file);
                return File.Exists(p) ? File.ReadAllText(p) : null;
            }

            var pJson = Read<List<Patient>>("patients.json");
            if (pJson != null) ds.Patients = JsonSerializer.Deserialize<List<Patient>>(pJson) ?? new();
            var phJson = Read<List<Physician>>("physicians.json");
            if (phJson != null) ds.Physicians = JsonSerializer.Deserialize<List<Physician>>(phJson) ?? new();
            var aJson = Read<List<Appointment>>("appointments.json");
            if (aJson != null) ds.Appointments = JsonSerializer.Deserialize<List<Appointment>>(aJson) ?? new();

            return ds;
        }
    }
}

