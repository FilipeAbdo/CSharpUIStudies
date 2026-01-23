using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace FirstMVVMApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; set;} = "Welcome to Avalonia!";

    public ICommand ClickMeCommand => new RelayCommand(() => 
    {
        Greeting = "Button Clicked!";
        OnPropertyChanged(nameof(Greeting));
    });

}
