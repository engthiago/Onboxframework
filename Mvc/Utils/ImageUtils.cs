using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Onbox.Mvc.V6.Utils
{
    public static class ImageUtils
    {
        public static BitmapSource BitmapSourceToGrayScale(Bitmap originalBitmap)
        {
            try
            {
                if (originalBitmap == null) return null;
                if (originalBitmap.VerticalResolution == 0) return null;

                int width = originalBitmap.Width;
                int height = originalBitmap.Height;

                var bitmap = new Bitmap(width, height);

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var color = originalBitmap.GetPixel(i, j);
                        if (color.A != 0)
                        {
                            var newColorData = (color.R + color.G + color.B) / 3;
                            var newColor = System.Drawing.Color.FromArgb(color.A, newColorData, newColorData, newColorData);
                            bitmap.SetPixel(i, j, newColor);
                        }
                    }
                }

                originalBitmap.Dispose();
                var newImg = ConvertToBitmapSource(bitmap, ImageFormat.Png);
                return newImg;
            }
            catch
            {
            }

            return null;
        }

        public static BitmapSource BitmapSourceToGrayScale(BitmapImage image)
        {
            try
            {
                var originalBitmap = ConvertToBitmap(image);

                if (originalBitmap == null) return null;
                if (originalBitmap.VerticalResolution == 0) return null;

                int width = originalBitmap.Width;
                int height = originalBitmap.Height;

                var bitmap = new Bitmap(width, height);

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var color = originalBitmap.GetPixel(i, j);
                        if (color.A != 0)
                        {
                            var newColorData = (color.R + color.G + color.B) / 3;
                            var newColor = System.Drawing.Color.FromArgb(color.A, newColorData, newColorData, newColorData);
                            bitmap.SetPixel(i, j, newColor);
                        }
                    }
                }

                originalBitmap.Dispose();
                var newImg = ConvertToBitmapSource(bitmap, ImageFormat.Png);
                return newImg;
            }
            catch
            {
            }

            return null;
        }

        public static Bitmap ConvertToBitmap(BitmapSource bitmapsource)
        {
            try
            {
                Bitmap bitmap;
                using (MemoryStream outStream = new MemoryStream())
                {
                    PngBitmapEncoder enc = new PngBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                    enc.Save(outStream);
                    bitmap = new Bitmap(outStream);
                }
                return bitmap;
            }
            catch
            {
            }

            return null;
        }

        public static Bitmap ConvertToBitmap(BitmapFrame frame)
        {
            try
            {
                Bitmap bitmap;
                using (MemoryStream outStream = new MemoryStream())
                {
                    PngBitmapEncoder enc = new PngBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(frame));
                    enc.Save(outStream);
                    bitmap = new Bitmap(outStream);
                }
                return bitmap;
            }
            catch
            {
            }

            return null;
        }

        static public BitmapSource ConvertToBitmapSource(Bitmap src)
        {
            return ConvertToBitmapSource(src, ImageFormat.Png);
        }

        static public Bitmap ConvertToBitmap(Stream stream)
        {
            try
            {
                using (Bitmap bitmap = Image.FromStream(stream) as Bitmap)
                {
                    return bitmap;
                }
            }
            catch
            {
                return null;
            }
        }

        static public BitmapSource ConvertToBitmapSource(Stream stream)
        {
            try
            {
                using (Bitmap bitmap = Image.FromStream(stream) as Bitmap)
                {
                    return ConvertToBitmapSource(bitmap);
                }
            }
            catch
            {
                return null;
            }
        }

        static public BitmapSource ConvertToBitmapSource(Bitmap src, ImageFormat imageFormat)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                src.Save(ms, imageFormat);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
            catch
            {
            }

            return null;
        }

        static public BitmapImage FromPath(string path)
        {
            var btmImage = new BitmapImage(new Uri(path));
            return btmImage;
        }
    }
}
