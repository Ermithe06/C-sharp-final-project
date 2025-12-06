using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MedicalCharting.Models;

public class Patient : INotifyPropertyChanged
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

    public DateTime BirthDate { get; set; }
    public string Address { get; set; } = "";
    public string Race { get; set; } = "";
    public Gender Gender { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateTime.Today.Year - BirthDate.Year;

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
