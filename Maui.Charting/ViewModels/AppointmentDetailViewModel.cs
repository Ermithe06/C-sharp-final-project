using System.Collections.ObjectModel;
using System.Windows.Input;
using MedicalCharting.Models;
using MedicalCharting.Services;

namespace Maui.Charting.ViewModels;

public class AppointmentDetailViewModel : BaseViewModel
{
    private readonly DataStore _store;
    private readonly AppointmentService _service;
    private readonly AppointmentsViewModel _parent;

    public Appointment Appointment { get; }

    public ObservableCollection<Patient> Patients => new(_store.Patients);
    public ObservableCollection<Physician> Physicians => new(_store.Physicians);

    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }

    public AppointmentDetailViewModel(DataStore store, AppointmentService service, Appointment existing, AppointmentsViewModel parent)
    {
        _store = store;
        _service = service;
        _parent = parent;

        Appointment = existing;

        SaveCommand = new Command(Save);
        DeleteCommand = new Command(Delete);
    }

    private async void Save()
    {
        if (!_service.TrySchedule(Appointment, out string msg))
        {
            await Application.Current.MainPage.DisplayAlert("Error", msg, "OK");
            return;
        }

        _parent.Refresh();
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    private async void Delete()
    {
        _store.Appointments.Remove(Appointment);
        _parent.Refresh();
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}
