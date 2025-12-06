using System.Collections.ObjectModel;
using System.Windows.Input;
using MedicalCharting.Models;
using MedicalCharting.Services;

namespace Maui.Charting.ViewModels
{
    public class AppointmentDetailViewModel : BaseViewModel
    {
        private readonly DataStore _store;
        private readonly AppointmentService _service;
        private readonly Appointment _appointment;
        private readonly AppointmentsViewModel _parent;

        public ObservableCollection<string> Diagnoses { get; } = new();
        public ObservableCollection<Treatment> Treatments { get; } = new();

        public ICommand AddDiagnosisCommand { get; }
        public ICommand DeleteDiagnosisCommand { get; }

        public ICommand AddTreatmentCommand { get; }
        public ICommand DeleteTreatmentCommand { get; }

        public string NewDiagnosis { get; set; } = "";
        public string NewTreatmentName { get; set; } = "";
        public decimal NewTreatmentCost { get; set; }

        public AppointmentDetailViewModel(DataStore store, AppointmentService service,
                                          Appointment appointment,
                                          AppointmentsViewModel parent)
        {
            _store = store;
            _service = service;
            _appointment = appointment;
            _parent = parent;

            foreach (var d in appointment.Diagnoses)
                Diagnoses.Add(d);

            foreach (var t in appointment.Treatments)
                Treatments.Add(t);

            AddDiagnosisCommand = new Command(AddDiagnosis);
            DeleteDiagnosisCommand = new Command<string>(DeleteDiagnosis);

            AddTreatmentCommand = new Command(AddTreatment);
            DeleteTreatmentCommand = new Command<Treatment>(DeleteTreatment);
        }

        private void AddDiagnosis()
        {
            if (string.IsNullOrWhiteSpace(NewDiagnosis)) return;

            Diagnoses.Add(NewDiagnosis);
            _appointment.Diagnoses.Add(NewDiagnosis);
            NewDiagnosis = "";
            OnPropertyChanged(nameof(NewDiagnosis));
        }

        private void DeleteDiagnosis(string d)
        {
            Diagnoses.Remove(d);
            _appointment.Diagnoses.Remove(d);
        }

        private void AddTreatment()
        {
            var t = new Treatment
            {
                Name = NewTreatmentName,
                Cost = NewTreatmentCost
            };

            Treatments.Add(t);
            _appointment.Treatments.Add(t);

            NewTreatmentName = "";
            NewTreatmentCost = 0;
            OnPropertyChanged(nameof(NewTreatmentName));
            OnPropertyChanged(nameof(NewTreatmentCost));
        }

        private void DeleteTreatment(Treatment t)
        {
            Treatments.Remove(t);
            _appointment.Treatments.Remove(t);
        }
    }
}
