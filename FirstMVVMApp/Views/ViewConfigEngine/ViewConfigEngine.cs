using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using FirstMVVMApp.Views;
using FirstMVVMApp.ViewModels;
using System.Text.Json;

using FirstMVVMApp.ConfigFileModel;

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

            // Subscribe to ViewModel error requests so the view can show a modal dialog
            var vm = window.DataContext as FirstMVVMApp.ViewModels.MainWindowViewModel;
            if (vm != null)
            {
                vm.ErrorRequested += (msg) =>
                {
                    // Ensure UI thread execution and avoid showing modal on a non-visible owner.
                    Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
                    {
                        var dlg = new ErrorWindow(msg);

                        if (window.IsVisible)
                        {
                            await dlg.ShowDialog<bool?>(window);
                        }
                        else
                        {
                            // Wait until window is opened, then show modal with owner
                            void openedHandler(object? s, EventArgs e)
                            {
                                window.Opened -= openedHandler;
                                // Fire-and-forget the dialog show so we don't block initialization
                                _ = dlg.ShowDialog<bool?>(window);
                            }

                            window.Opened += openedHandler;
                        }

                        (Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown();
                    });
                };
            }

            // Set window title from config
            if (root.TryGetProperty(ConfigFileDataModel.Root.MainWindow, out JsonElement mainWindowProp) && mainWindowProp.ValueKind == JsonValueKind.Object)
            {
                if (mainWindowProp.TryGetProperty(ConfigFileDataModel.Window.Title, out JsonElement titleProp) && titleProp.ValueKind == JsonValueKind.String)
                {
                    var title = titleProp.GetString()?.Trim();
                    if (!string.IsNullOrEmpty(title)){
                        if (title.StartsWith("@")){
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
}