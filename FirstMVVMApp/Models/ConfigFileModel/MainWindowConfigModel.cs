using System.Collections.Generic;

namespace FirstMVVMApp.Models.ConfigFileModel
{
    public class MainWindowConfigModel
    {
        public string Title { get; set; } = string.Empty;
        public int Width { get; set; } = 50;
        public int Height { get; set; } = 50;
        public List<ControlConfigModel<object>> Children { get; set; } = new();

        public MainWindowConfigModel() { }

        public MainWindowConfigModel(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
        }
    }
}