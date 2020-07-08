using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Onbox.Revit.V7.UI
{
    public class ImageManager
    {
        public BitmapImage ConvertBitmapSource(string targetResourceName, Assembly assembly)
        {
            var stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(targetResourceName)));
            return ConvertToBitmapSource(stream) as BitmapImage;
        }

        public BitmapSource ConvertToBitmapSource(Stream stream)
        {
            try
            {
                using (Bitmap bitmap = Image.FromStream(stream) as Bitmap)
                {
                    return ConvertToBitmapSource(bitmap, ImageFormat.Png);
                }
            }
            catch
            {
                return null;
            }
        }

        public BitmapSource ConvertToBitmapSource(Bitmap src, ImageFormat imageFormat)
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
    }
}
