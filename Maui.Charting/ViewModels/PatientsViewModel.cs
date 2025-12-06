using System.Collections.ObjectModel;
using System.Windows.Input;
using MedicalCharting.Models;
using MedicalCharting.Services;
using Maui.Charting.Views;

namespace Maui.Charting.ViewModels
{
    public class PatientsViewModel : BaseViewModel
    {
        private readonly DataStore _store;

        public ObservableCollection<Patient> Patients { get; } = new();

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
            var p = new Patient
            {
                Id = _store.Patients.Any() ? _store.Patients.Max(x => x.Id) + 1 : 1,
                FirstName = NewFirstName,
                LastName = NewLastName,
                Address = NewAddress,
                Race = NewRace,
                Gender = NewGender,
                BirthDate = NewBirthdate
            };

            _store.Patients.Add(p);
            _store.NotifyPatientsChanged();

            Clear();
        }

        private void DeletePatient(Patient p)
        {
            if (p == null) return;

            _store.Patients.Remove(p);
            _store.NotifyPatientsChanged();
        }

        private async void OpenEditPage(Patient p)
        {
            if (p == null) return;

            var vm = new PatientDetailViewModel(_store, p, this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditPatientPage(vm));
        }

        // Form fields
        public string NewFirstName { get; set; } = "";
        public string NewLastName { get; set; } = "";
        public string NewAddress { get; set; } = "";
        public string NewRace { get; set; } = "";
        public Gender NewGender { get; set; } = Gender.Unknown;
        public DateTime NewBirthdate { get; set; } = DateTime.Today;

        private void Clear()
        {
            NewFirstName = "";
            NewLastName = "";
            NewAddress = "";
            NewRace = "";
            NewGender = Gender.Unknown;
            NewBirthdate = DateTime.Today;

            OnPropertyChanged(nameof(NewFirstName));
            OnPropertyChanged(nameof(NewLastName));
            OnPropertyChanged(nameof(NewAddress));
            OnPropertyChanged(nameof(NewRace));
            OnPropertyChanged(nameof(NewGender));
            OnPropertyChanged(nameof(NewBirthdate));
        }
    }
}
