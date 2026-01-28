using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;

namespace FirstMVVMApp.Views.ViewConfigEngine
{
    public class ErrorWindow : Window
    {
        public ErrorWindow()
        {
            Width = 420;
            Height = 150;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            // Build UI
            var icon = new TextBlock
            {
                Text = "❗",
                FontSize = 36,
                Foreground = Brushes.DarkRed,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };

            var messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4, 0)
            };
            messageText.Bind(TextBlock.TextProperty, new Binding("Message"));

            var okButton = new Button
            {
                Content = "OK",
                Width = 80,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };
            okButton.Bind(Button.CommandProperty, new Binding("OkCommand"));

            var contentPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(12)
            };

            var topRow = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            topRow.Children.Add(icon);
            topRow.Children.Add(messageText);

            contentPanel.Children.Add(topRow);
            contentPanel.Children.Add(okButton);

            Content = contentPanel;
        }

        /// <summary>
        /// Shows the error dialog modally and returns when OK is pressed.
        /// </summary>
        /// <param name="owner">Owner window (can be null)</param>
        /// <param name="message">Error message to display</param>
        /// <returns>Task that completes when user presses OK</returns>
        public static Task<bool> ShowDialogAsync(Window owner, string message)
        {
            var vm = new ErrorWindowViewModel(message);
            var win = new ErrorWindow
            {
                DataContext = vm
            };

            // Close the window with result true when VM requests OK
            vm.OkRequested += () => win.Close(true);

            return win.ShowDialog<bool>(owner);
        }
    }

    internal class ErrorWindowViewModel
    {
        public string Message { get; }
        public ICommand OkCommand { get; }

        internal event Action? OkRequested;

        public ErrorWindowViewModel(string message)
        {
            Message = message ?? string.Empty;
            OkCommand = new RelayCommand(_ => OnOk());
        }

        private void OnOk() => OkRequested?.Invoke();
    }

    internal class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(object? parameter) => _execute(parameter);
        // Avalonia does not provide WPF's CommandManager. Provide a simple event implementation.
        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}