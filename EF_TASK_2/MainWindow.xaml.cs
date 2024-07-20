using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace EF_TASK_2;
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private Car car = new();
    private ObservableCollection<Car> cars;
    private readonly IConfiguration _configuration;
    public Car Car { get => car; set { car = value; OnPropertyChanged(); } }
    public ObservableCollection<Car> Cars { get => cars; set { cars = value; OnPropertyChanged(); } }
    public MainWindow(IConfiguration configuration)
    {
        InitializeComponent();
        _configuration = configuration;
        DataContext = this;
        RefreshSource();
    }

    void RefreshSource()
    {
        using CarContext db = new(_configuration.GetConnectionString("DbConnection")!);
        Cars = new();
        foreach (var c in db.Cars)
            Cars.Add(c);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (lb.SelectedItem is null) return;
        var dialog = MessageBox.Show("Are sure to delete this item?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (dialog == MessageBoxResult.Yes)
        {
            using var db = new CarContext(_configuration.GetConnectionString("DbConnection")!);
            foreach (var c in db.Cars)
                if (c.Id == Car.Id)
                {
                    db.Cars.Remove(c);
                    break;
                }
            db.SaveChanges();
            RefreshSource();
        lb.SelectedItems.Clear();
        }

        Car = new();
    }

    private void Update_Click(object sender, RoutedEventArgs e)
    {
        if (lb.SelectedItem is null) return;
        var dialog = MessageBox.Show("Are sure to update this item?", "Update", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (dialog == MessageBoxResult.Yes)
        {
            using var db = new CarContext(_configuration.GetConnectionString("DbConnection")!);
            foreach (var c in db.Cars)
                if (c.Id == Car.Id)
                {
                    c.Make = Car.Make;
                    c.Model = Car.Model;
                    c.Year = Car.Year;
                    c.Id = Car.Id;
                    c.St_number = Car.St_number;
                }
            db.SaveChanges();
            RefreshSource();
            lb.SelectedItems.Clear();

        }
        Car = new();
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (Car.Make == "" || Car.Make == "" || Car.Year > 2024 || Car.Year < 1970 || Car.St_number is null)
        {
            MessageBox.Show("Please Enter all Fields In correct form", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        foreach (var c in Cars)
            if (c.St_number == Car.St_number)
            {
                MessageBox.Show("This State Number has already ben used change it please!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        using var db = new CarContext(_configuration.GetConnectionString("DbConnection")!);
        db.Cars.Add(Car);
        db.SaveChanges();
        Car = new();
        RefreshSource();
    }

    private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        var lb = sender as ListBox;
        var carSelected = lb?.SelectedItem as Car;
        if (carSelected is null) return;
        Car.Make = carSelected.Make;
        Car.Model = carSelected.Model;
        Car.Year = carSelected.Year;
        Car.Id = carSelected.Id;
        Car.St_number = carSelected.St_number;
    }
}