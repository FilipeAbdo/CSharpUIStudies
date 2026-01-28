using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using FirstMVVMApp.Views;
using FirstMVVMApp.ViewModels;
using System.Text.Json;

using FirstMVVMApp.Models.ConfigFileModel;

namespace FirstMVVMApp.Views.ViewConfigEngine;

public class ViewConfigEngine
{
    private readonly string _configFilePath;

    public ViewConfigEngine(string configFilePath)
    {
        if (string.IsNullOrWhiteSpace(configFilePath))
            throw new ArgumentException("Config file path cannot be null or empty.", nameof(configFilePath));

        _configFilePath = configFilePath;
    }

    public Window CreateMainWindow()
    {
        if (!File.Exists(_configFilePath))
            throw new FileNotFoundException("Configuration file not found.", _configFilePath);

        try
        {
            var configJson = File.ReadAllText(_configFilePath);
            using var doc = JsonDocument.Parse(configJson);
            var root = doc.RootElement;

            var window = new MainWindow();
            var vm = window.DataContext as FirstMVVMApp.ViewModels.MainWindowViewModel;

            // Placeholder replacement inside CreateMainWindow:
            RegisterVmErrorHandler(window);

            // Set window title from config
            if (root.TryGetProperty(ConfigFileDataModel.Root.MainWindow, out JsonElement mainWindowProp) && mainWindowProp.ValueKind == JsonValueKind.Object)
            {
                if (mainWindowProp.TryGetProperty(ConfigFileDataModel.Window.Title, out JsonElement titleProp) && titleProp.ValueKind == JsonValueKind.String)
                {
                    var title = titleProp.GetString()?.Trim();
                    if (!string.IsNullOrEmpty(title))
                    {
                        if (title.StartsWith("@"))
                        {
                            window.Title = vm?.GetValueByPropertyName(title.Substring(1)) ?? "Default Title";
                        }
                        else
                        {
                            window.Title = title;
                        }
                    }
                }
            }
            // Additional configuration can be added here

            return window;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Invalid JSON format in configuration file.", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException("Error reading configuration file.", ex);
        }
    }

    // Add these methods at class level (outside CreateMainWindow):
    private void RegisterVmErrorHandler(Window window)
    {
        var vm = window.DataContext as FirstMVVMApp.ViewModels.MainWindowViewModel;
        if (vm != null)
        {
            vm.ErrorRequested += (msg) => HandleErrorRequested(window, msg);
        }
    }

    private void HandleErrorRequested(Window window, string msg)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
        {
            var dlg = new FirstMVVMApp.Views.ErrorWindow(msg);
            await dlg.ShowDialog<bool?>(window);
            (Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown();
        });
    }
}