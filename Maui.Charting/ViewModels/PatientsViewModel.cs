using Maui.Charting.Views;
using MedicalCharting.Models;
using MedicalCharting.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maui.Charting.ViewModels
{
    public class PatientsViewModel : BaseViewModel
    {
        private readonly DataStore _store;

        public ObservableCollection<Patient> Patients { get; } = new();

        // Fields for Add Patient
        public string NewFirstName { get; set; } = "";
        public string NewLastName { get; set; } = "";
        public string NewAddress { get; set; } = "";
        public DateTime NewBirthDate { get; set; } = DateTime.Today;
        public string NewRace { get; set; } = "";
        public Gender NewGender { get; set; } = Gender.Unknown;

        public IEnumerable<Gender> GenderOptions =>
            Enum.GetValues(typeof(Gender)).Cast<Gender>();

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public PatientsViewModel(DataStore store)
        {
            _store = store;

            _store.PatientsChanged += Refresh;

            AddCommand = new Command(AddPatient);
            DeleteCommand = new Command<Patient>(DeletePatient);
            EditCommand = new Command<Patient>(OpenEditPage);

            Refresh();
        }

        public void Refresh()
        {
            Patients.Clear();
            foreach (var p in _store.Patients)
                Patients.Add(p);
        }

        private void AddPatient()
        {
            if (string.IsNullOrWhiteSpace(NewFirstName) ||
                string.IsNullOrWhiteSpace(NewLastName))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Name fields are required.", "OK");
                return;
            }

            var patient = new Patient
            {
                Id = _store.Patients.Any() ? _store.Patients.Max(p => p.Id) + 1 : 1,
                FirstName = NewFirstName,
                LastName = NewLastName,
                Address = NewAddress,
                BirthDate = NewBirthDate,
                Race = NewRace,
                Gender = NewGender
            };

            _store.Patients.Add(patient);
            _store.NotifyPatientsChanged();

            ClearForm();
        }

        private void DeletePatient(Patient p)
        {
            if (p == null) return;

            _store.Patients.Remove(p);
            _store.NotifyPatientsChanged();
        }

        private async void OpenEditPage(Patient patient)
        {
            if (patient == null) return;

            var vm = new PatientDetailViewModel(_store, patient, this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditPatientPage(vm));
        }

        private void ClearForm()
        {
            NewFirstName = "";
            NewLastName = "";
            NewAddress = "";
            NewRace = "";
            NewGender = Gender.Unknown;
            NewBirthDate = DateTime.Today;

            OnPropertyChanged(nameof(NewFirstName));
            OnPropertyChanged(nameof(NewLastName));
            OnPropertyChanged(nameof(NewAddress));
            OnPropertyChanged(nameof(NewRace));
            OnPropertyChanged(nameof(NewGender));
            OnPropertyChanged(nameof(NewBirthDate));
        }
    }
}
