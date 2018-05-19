using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace OstrichRenderer
{
    /// 为了解决WPF恶心的bitmapsource弄的，不必理会
    static class Bitmap2WriteableBitmap
    {
        public static void BitmapToWriteableBitmap(WriteableBitmap source, Bitmap bitmap)
        {
            try
            {
                CopyFrom(source, bitmap);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void CopyFrom(WriteableBitmap wb, Bitmap bitmap)
        {
            if (wb == null)
                throw new ArgumentNullException(nameof(wb));
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            var ws = wb.PixelWidth;
            var hs = wb.PixelHeight;
            var wt = bitmap.Width;
            var ht = bitmap.Height;
            if (ws != wt || hs != ht)
                throw new ArgumentException("暂时只支持相同尺寸图片的复制。");

            var width = ws;
            var height = hs;
            var bytes = ws * hs * wb.Format.BitsPerPixel / 8;

            var rBitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            wb.Lock();
            unsafe
            {
                Buffer.MemoryCopy(rBitmapData.Scan0.ToPointer(), wb.BackBuffer.ToPointer(), bytes, bytes);
            }

            wb.AddDirtyRect(new Int32Rect(0, 0, width, height));
            wb.Unlock();

            bitmap.UnlockBits(rBitmapData);
        }

        public static BitmapSource GetbBitmapSource() => Bitmap2BitmapImage(Renderer.GetBitmap());

        private static BitmapSource Bitmap2BitmapImage(Bitmap bitmap) =>
            Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
    }
}
