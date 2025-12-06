using MedicalCharting.Models;
using MedicalCharting.Services;
using System.Windows.Input;

namespace Maui.Charting.ViewModels;

public class PatientDetailViewModel : BaseViewModel
{
    private readonly DataStore _store;
    private readonly PatientsViewModel _parent;

    public Patient Patient { get; }

    public ICommand SaveCommand { get; }

    public PatientDetailViewModel(DataStore store, Patient patient, PatientsViewModel parent)
    {
        _store = store;
        _parent = parent;
        Patient = patient;

        SaveCommand = new Command(Save);
    }

    private async void Save()
    {
        _store.NotifyPatientsChanged();
        _parent.Refresh();

        await Application.Current.MainPage.DisplayAlert(
            "Saved", "Patient updated successfully.", "OK");
    }
}
