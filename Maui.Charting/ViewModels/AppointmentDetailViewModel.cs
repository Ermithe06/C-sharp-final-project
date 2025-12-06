using MedicalCharting.Models;
using MedicalCharting.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maui.Charting.ViewModels;

public class AppointmentDetailViewModel : BaseViewModel
{
    private readonly DataStore _store;
    private readonly AppointmentService _service;
    private readonly AppointmentsViewModel _parent;

    public Appointment Appointment { get; }

    public ObservableCollection<string> Diagnoses { get; }
    public ObservableCollection<Treatment> Treatments { get; }

    public string NewDiagnosis { get; set; } = "";
    public string NewTreatmentName { get; set; } = "";
    public decimal NewTreatmentCost { get; set; }

    public ICommand AddDiagnosisCommand { get; }
    public ICommand RemoveDiagnosisCommand { get; }
    public ICommand AddTreatmentCommand { get; }
    public ICommand RemoveTreatmentCommand { get; }
    public ICommand SaveCommand { get; }

    public AppointmentDetailViewModel(
        DataStore store,
        AppointmentService service,
        Appointment appt,
        AppointmentsViewModel parent)
    {
        _store = store;
        _service = service;
        _parent = parent;

        Appointment = appt;

        Diagnoses = new(appt.Diagnoses);
        Treatments = new(appt.Treatments);

        AddDiagnosisCommand = new Command(AddDiagnosis);
        RemoveDiagnosisCommand = new Command<string>(d => Diagnoses.Remove(d));
        AddTreatmentCommand = new Command(AddTreatment);
        RemoveTreatmentCommand = new Command<Treatment>(t => Treatments.Remove(t));
        SaveCommand = new Command(Save);
    }

    void AddDiagnosis()
    {
        if (!string.IsNullOrWhiteSpace(NewDiagnosis))
            Diagnoses.Add(NewDiagnosis);
        NewDiagnosis = "";
        OnPropertyChanged(nameof(NewDiagnosis));
    }

    void AddTreatment()
    {
        Treatments.Add(new Treatment
        {
            Name = NewTreatmentName,
            Cost = NewTreatmentCost
        });
        NewTreatmentName = "";
        NewTreatmentCost = 0;
        OnPropertyChanged(nameof(NewTreatmentName));
        OnPropertyChanged(nameof(NewTreatmentCost));
    }

    async void Save()
    {
        Appointment.Diagnoses = Diagnoses.ToList();
        Appointment.Treatments = Treatments.ToList();

        _store.NotifyAppointmentsChanged();
        _parent.Refresh();

        await Application.Current.MainPage.DisplayAlert(
            "Saved", "Appointment updated.", "OK");
    }
}
