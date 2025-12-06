using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MedicalCharting.Models;

public class Physician : INotifyPropertyChanged
{
    public int Id { get; set; }

    private string _firstName = "";
    public string FirstName
    {
        get => _firstName;
        set { _firstName = value; OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); }
    }

    private string _lastName = "";
    public string LastName
    {
        get => _lastName;
        set { _lastName = value; OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); }
    }

    public string LicenseNumber { get; set; } = "";
    public DateTime GraduationDate { get; set; }
    public string Specialization { get; set; } = "";

    public string FullName => $"Dr. {FirstName} {LastName}";

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
