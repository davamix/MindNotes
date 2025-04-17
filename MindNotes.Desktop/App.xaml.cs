using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using MindNotes.Desktop.Bootstrap;
using MindNotes.Desktop.Views;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindNotes.Desktop {
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .RegisterProviders()
                .RegisterServices()
                .RegisterViewModels()
                .RegisterConfiguration()
                .RegisterApplicationServices()
                .BuildServiceProvider());

            ShowWindow();
        }

        private void ShowWindow() {
            m_window = new MainWindow();
            m_window.CenterOnScreen();
            m_window.Activate();

            var windowManager = WindowManager.Get(m_window);
            windowManager.MinHeight = 600;
            windowManager.MinWidth = 600;
            windowManager.Width = 1920;
            windowManager.Height = 1080;
        }

        private Window? m_window;
    }
}
