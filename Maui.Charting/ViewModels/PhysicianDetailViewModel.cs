using MedicalCharting.Models;
using MedicalCharting.Services;
using System.Windows.Input;

namespace Maui.Charting.ViewModels;

public class PhysicianDetailViewModel : BaseViewModel
{
    private readonly DataStore _store;
    private readonly PhysiciansViewModel _parent;

    public Physician Physician { get; }

    public ICommand SaveCommand { get; }

    public PhysicianDetailViewModel(DataStore store, Physician physician, PhysiciansViewModel parent)
    {
        _store = store;
        _parent = parent;
        Physician = physician;

        SaveCommand = new Command(Save);
    }

    private async void Save()
    {
        _store.NotifyPhysiciansChanged();
        _parent.Refresh();

        await Application.Current.MainPage.DisplayAlert(
            "Saved", "Physician updated successfully.", "OK");
    }
}
