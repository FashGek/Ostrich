using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OstrichRenderer
{
    /// <summary>
    /// Preview.xaml 的交互逻辑
    /// </summary>
    public partial class Preview : Window
    {
        public WriteableBitmap WriteableBitmap;

        public Preview()
        {
            InitializeComponent();
            Width = Renderer.Width;
            Height = Renderer.Height;
            Img.Width = Renderer.Width;
            Img.Height = Renderer.Height;

            Timer timer = new Timer();
            timer.Interval = 500;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            Img.Source = Bitmap2WriteableBitmap.GetbBitmapSource();
            WriteableBitmap = new WriteableBitmap((BitmapSource) Img.Source);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) => Img.Dispatcher.BeginInvoke(new Action(() =>
        {
            Bitmap2WriteableBitmap.BitmapToWriteableBitmap(WriteableBitmap,
                Renderer.GetBitmap());
            Img.Source = WriteableBitmap;
        }));

        private void Window_Loaded(object sender, RoutedEventArgs e) => Renderer.Start();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(Img);
            Console.WriteLine(p);
            Renderer.DrawRoute((int)p.X, (int)p.Y);
        }
    }
}
