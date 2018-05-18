using System.Windows;
using System.Windows.Threading;

namespace OstrichRenderer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message + " " + e.Exception.TargetSite,
                "出错", MessageBoxButton.OK, MessageBoxImage.Information);
            e.Handled = true;
        }
    }
}
