This project is a complete multi-stage medical charting system built across Assignments 1–3 in the course.
The application includes:

A console-based charting system (Assignment 1)

A .NET MAUI front-end application with CRUD functionality (Assignment 2)

Full front-end enhancements including sorting, inline CRUD, validations, color coding, diagnoses, and treatments (Assignment 3 — A level)

📁 Project Structure
/ChartingSystem         // Class library: Models + Services
  /Models
    Patient.cs
    Physician.cs
    Appointment.cs
    Treatment.cs
    MedicalNote.cs
  /Services
    DataStore.cs
    AppointmentService.cs
  Program.cs (console app for Assignment 1)

/Maui.Charting          // .NET MAUI UI application
  /Views
    PatientsPage.xaml
    EditPatientPage.xaml
    PhysiciansPage.xaml
    EditPhysicianPage.xaml
    AppointmentsPage.xaml
    EditAppointmentPage.xaml
  /ViewModels
    PatientsViewModel.cs
    PatientDetailViewModel.cs
    PhysiciansViewModel.cs
    PhysicianDetailViewModel.cs
    AppointmentsViewModel.cs
    AppointmentDetailViewModel.cs
  /Converters
    AgeColorConverter.cs     // Minor highlighting
    AppointmentDateColorConverter.cs // Appointments happening today

✨ Completed Features by Assignment
🧱 Assignment 1 — Console Application 

Completed:
✔ Create & manage patients
✔ Track demographics (name, address, birthdate, race, gender)
✔ Track medical notes, diagnoses, prescriptions
✔ Create physicians
✔ Track demographics (name, license number, graduation date, specialization)
✔ Create appointments
✔ Prevent physician double-booking
✔ Restrict appointments to Mon–Fri, 8 AM–5 PM

📱 Assignment 2 — Basic MAUI App 

Completed:
✔ Full CRUD for patients, physicians, appointments
✔ Pickers for selecting valid patients & physicians
✔ Enforced scheduling constraints
✔ Navigation using AppShell
✔ Inline and dialog editing
✔ Validation + error messaging

⚡ Assignment 3 — Front-End Enhancements 
B-Level Requirements

✔ Inline + dialog CRUD everywhere
✔ Sorting (ascending/descending)
✔ In-line buttons in ListViews
✔ Pickers for patient/physician selection in appointments


✔ Diagnoses management
✔ Treatments + total cost (treatment cost aggregator)
✔ Rooms for appointments + room conflict prevention
✔ Color coding:
Appointments occurring today → highlight

✔ Strong UI polish & refreshed ViewModels


🚀 How to Run
Prerequisites

Visual Studio 2022

.NET 8

MAUI workload installed

Running the MAUI app
Set "Maui.Charting" as Startup Project  
Run ➝ Build and Deploy  

🧪 Testing Checklist

Add/Edit/Delete patients → updates immediately

Add/Edit/Delete physicians → updates immediately

Add/Edit/Delete appointments → validates constraints

Diagnoses & treatments persist

Room conflict detection works

Color coding works

Sorting works

🎓 Grade Justification (as shown in video)

I am submitting for an A (Assignment 3 — A-Level Front-End completion).

I completed:

All Assignment 3 B-level requirements

All Assignment 3 A-level requirements

A fully functional, polished UI

Strong validation and business rules

✔ Final Notes

The .sln file is NOT required for grading as long as the repository contains:
✔ all .csproj files
✔ all source files
✔ instructions for how to run

Visual Studio can open the project by using "Open a project or solution" on the .csproj files if needed.
