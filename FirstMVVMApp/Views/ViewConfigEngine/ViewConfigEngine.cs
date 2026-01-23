using System;
using System.IO;
using Avalonia.Controls;
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

            // Set window title from config
            if (root.TryGetProperty(ConfigFileDataModel.Root.MainWindow, out JsonElement mainWindowProp) && mainWindowProp.ValueKind == JsonValueKind.Object)
            {
                if (mainWindowProp.TryGetProperty(ConfigFileDataModel.Window.Title, out JsonElement titleProp) && titleProp.ValueKind == JsonValueKind.String)
                {
                    var title = titleProp.GetString()?.Trim();
                    if (!string.IsNullOrEmpty(title))
                        window.Title = title;
                }
            }
                    window.Title = titleProp.GetString();
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