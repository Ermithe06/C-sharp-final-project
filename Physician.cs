using System;
using System.Collections.Generic;

namespace MedicalCharting.Models
{
    public class Physician
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string LicenseNumber { get; set; } = "";
        public DateTime GraduationDate { get; set; }
        public List<string> Specializations { get; set; } = new();

        public string FullName => $"{FirstName} {LastName}";
    }
}

