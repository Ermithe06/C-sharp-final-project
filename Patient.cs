using System;
using System.Collections.Generic;

namespace MedicalCharting.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string Race { get; set; } = "";
        public Gender Gender { get; set; } = Gender.Unknown;
        public List<MedicalNote> Notes { get; set; } = new();

        public string FullName => $"{FirstName} {LastName}";
    }

    public enum Gender { Unknown, Male, Female, NonBinary, Other }
}

