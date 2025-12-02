using System.Collections.ObjectModel;
using System.Windows.Input;
using MedicalCharting.Models;
using MedicalCharting.Services;
using Maui.Charting.Views;
using Microsoft.Maui.Dispatching;

namespace Maui.Charting.ViewModels
{
    public class AppointmentsViewModel : BaseViewModel
    {
        private readonly DataStore _store;
        private readonly AppointmentService _service;

        public ObservableCollection<Patient> Patients { get; } = new();
        public ObservableCollection<Physician> Physicians { get; } = new();
        public ObservableCollection<Appointment> Appointments { get; } = new();

        // Inline Add fields
        public DateTime NewDate { get; set; } = DateTime.Today;
        public TimeSpan NewTime { get; set; } = TimeSpan.FromHours(8);
        public int NewDuration { get; set; } = 30;
        public string NewRoom { get; set; } = "";
        public Patient? NewSelectedPatient { get; set; }
        public Physician? NewSelectedPhysician { get; set; }

        // Sorting
        public List<string> SortOptions { get; } =
            new() { "Start Time", "Patient", "Physician" };

        private string _selectedSort = "Start Time";
        public string SelectedSort
        {
            get => _selectedSort;
            set
            {
                _selectedSort = value;
                ApplySort();
                OnPropertyChanged();
            }
        }

        private bool _ascending = true;

        // Commands
        public ICommand SortAscCommand { get; }
        public ICommand SortDescCommand { get; }
        public ICommand AddInlineCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public AppointmentsViewModel(DataStore store, AppointmentService service)
        {
            _store = store;
            _service = service;

            // subscribe to store change events
            _store.PatientsChanged += RefreshPatients;
            _store.PhysiciansChanged += RefreshPhysicians;
            _store.AppointmentsChanged += RefreshAppointments;

            SortAscCommand = new Command(() => { _ascending = true; ApplySort(); });
            SortDescCommand = new Command(() => { _ascending = false; ApplySort(); });
            AddInlineCommand = new Command(AddAppointmentInline);
            DeleteCommand = new Command<Appointment>(DeleteAppointment);
            EditCommand = new Command<Appointment>(OpenEditPage);

            Refresh(); // load initial data
        }

        // -----------------------------
        // Full refresh
        // -----------------------------
        public void Refresh()
        {
            RefreshPatients();
            RefreshPhysicians();
            RefreshAppointments();
        }

        // -----------------------------
        // Sub-refresh methods
        // -----------------------------
        private void RefreshPatients()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Patients.Clear();
                foreach (var p in _store.Patients)
                    Patients.Add(p);
            });
        }

        private void RefreshPhysicians()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Physicians.Clear();
                foreach (var p in _store.Physicians)
                    Physicians.Add(p);
            });
        }

        private void RefreshAppointments()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Appointments.Clear();
                foreach (var a in _store.Appointments)
                    Appointments.Add(a);

                ApplySort();
            });
        }

        // -----------------------------
        // Add inline appointment
        // -----------------------------
        private void AddAppointmentInline()
        {
            if (NewSelectedPatient == null || NewSelectedPhysician == null)
            {
                Application.Current.MainPage.DisplayAlert(
                    "Error", "Both patient and physician are required.", "OK");
                return;
            }

            var appt = new Appointment
            {
                Id = _store.Appointments.Any() ? _store.Appointments.Max(a => a.Id) + 1 : 1,
                PatientId = NewSelectedPatient.Id,
                PhysicianId = NewSelectedPhysician.Id,
                Room = NewRoom,
                Start = NewDate + NewTime,
                DurationMinutes = NewDuration
            };

            if (!_service.TrySchedule(appt, out string message))
            {
                Application.Current.MainPage.DisplayAlert("Error", message, "OK");
                return;
            }

            _store.Appointments.Add(appt);
            _store.NotifyAppointmentsChanged();
        }

        // -----------------------------
        // Edit existing appointment
        // -----------------------------
        private async void OpenEditPage(Appointment appt)
        {
            var vm = new AppointmentDetailViewModel(_store, _service, appt, this);
            await Application.Current.MainPage.Navigation.PushAsync(
                new EditAppointmentPage(vm));
        }

        // -----------------------------
        // Delete appointment
        // -----------------------------
        private void DeleteAppointment(Appointment appt)
        {
            _store.Appointments.Remove(appt);
            _store.NotifyAppointmentsChanged();
        }

        // -----------------------------
        // Sorting
        // -----------------------------
        public void ApplySort()
        {
            IEnumerable<Appointment> sorted = SelectedSort switch
            {
                "Patient" => Appointments.OrderBy(a => a.PatientId),
                "Physician" => Appointments.OrderBy(a => a.PhysicianId),
                _ => Appointments.OrderBy(a => a.Start)
            };

            if (!_ascending)
                sorted = sorted.Reverse();

            var temp = sorted.ToList();

            Appointments.Clear();
            foreach (var a in temp)
                Appointments.Add(a);
        }
    }
}
