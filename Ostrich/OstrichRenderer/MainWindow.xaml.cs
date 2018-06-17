using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OstrichRenderer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Preview _preview;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Render_Click(object sender, RoutedEventArgs e)
        {
            Renderer.Init(int.Parse(WidthBox.Text), int.Parse(HeightBox.Text));
            _preview = new Preview();
            _preview.Show();
            _preview.Closed += Preview_Closed;
            Render.IsEnabled = false;
        }

        private void Preview_Closed(object sender, EventArgs e) => Render.IsEnabled = true;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(Bitmap2WriteableBitmap.GetbBitmapSource()));
            using (FileStream fileStream = new FileStream(
                DateTime.Now.ToString("yyyy-M-d-hh-mm-ss") + ".png", FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
    }
}
