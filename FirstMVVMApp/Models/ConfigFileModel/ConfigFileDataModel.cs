namespace FirstMVVMApp.Models.ConfigFileModel
{
    public static class ConfigFileDataModel
    {
        public static class Root
        {
            public const string CustomControls = "CustomControls";
            public const string MainWindow = "MainWindow";
        }

        public static class Controls
        {
            public const string Type = "Type";
            public const string Orientation = "Orientation";
            public const string Margin = "Margin";
        }

        public static class StackPanel
        {
            public const string name = "StackPanel";
            public const string Orientation = "Orientation";
            public const string Margin = "Margin";
            public const string Spacing = "Spacing";
            public const string Children = "Children";
            public const string HorizontalAlignment = "HorizontalAlignment";
            public const string VerticalAlignment = "VerticalAlignment";
        }

        public static class Window
        {
            public const string Title = "Title";
            public const string Width = "Width";
            public const string Height = "Height";
        }

        public static class Panel
        {
            public const string StackPanel = "StackPanel";
            public const string Orientation = "Orientation";
            public const string Margin = "Margin";
            public const string Spacing = "Spacing";
            public const string Children = "Children";
        }


    }
}