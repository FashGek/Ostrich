using System.Windows;
using System.Windows.Media.Imaging;

namespace OstrichRenderer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Preview Preview;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Render_Click(object sender, RoutedEventArgs e)
        {
            Renderer.Init(int.Parse(WidthBox.Text), int.Parse(HeightBox.Text), int.Parse(SPPBox.Text));
            Preview = new Preview();
            Preview.Show();
            Preview.Closed += Preview_Closed;
            Render.IsEnabled = false;
        }

        private void Preview_Closed(object sender, System.EventArgs e)
        {
            Render.IsEnabled = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(Renderer.GetbBitmapSource()));
            using (var fileStream = new System.IO.FileStream(FileNameBox.Text, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
    }
}
