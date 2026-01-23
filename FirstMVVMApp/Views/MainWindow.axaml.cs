using Avalonia.Controls;

namespace FirstMVVMApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new ViewModels.MainWindowViewModel();
    }
}