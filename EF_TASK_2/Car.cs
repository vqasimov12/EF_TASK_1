using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EF_TASK_2;
public class Car : INotifyPropertyChanged
{
    private int id;
    private string make;
    private string model;
    private int? year;
    private int? st_number;

    public int Id { get => id; set { id = value; OnPropertyChanged(); } }
    public string Make { get => make; set { make = value; OnPropertyChanged(); } }
    public string Model { get => model; set { model = value; OnPropertyChanged(); } }
    public int? Year { get => year; set { year = value; OnPropertyChanged(); } }
    public int? St_number { get => st_number; set { st_number = value; OnPropertyChanged(); } }
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}