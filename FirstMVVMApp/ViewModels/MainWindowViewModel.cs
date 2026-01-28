using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using FirstMVVMApp.Views;

namespace FirstMVVMApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; set; } = "Welcome to Avalonia!";
    public string AppTitle { get; set; } = "Tesing Dynamic Title Binding";

    public ICommand ClickMeCommand => new RelayCommand(() =>
    {
        Greeting = "Button Clicked!";
        OnPropertyChanged(nameof(Greeting));
    });

    public string GetValueByPropertyName(string propertyName)
    {
        var property = this.GetType().GetProperty(propertyName);
        if (property != null)
        {
            var value = property.GetValue(this) as string;
            if (value != null)
            {
                return value;
            }
            throw new InvalidOperationException(
                $"Property {propertyName} exists but returned null.");
        }
        var message = $"Error: Property {propertyName} not found";
        ErrorRequested?.Invoke(message);
        return string.Empty;
    }

    public event Action<string>? ErrorRequested;

}
