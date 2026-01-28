using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FirstMVVMApp.Views;

public partial class ErrorWindow : Window
{
    public ErrorWindow()
    {
        InitializeComponent();
    }

    public ErrorWindow(string message) : this()
    {
        MessageText.Text = message;
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
        Close(true);
    }
}
