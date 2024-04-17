#nullable enable

namespace Server {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState) {
            Window window = base.CreateWindow(activationState);

            window.Width = 800;
            window.Height = 600;

            DisplayInfo displayInfo = DeviceDisplay.Current.MainDisplayInfo;

            window.X = 400;
            window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 2;

            return window;
        }
    }
}

#nullable disable
