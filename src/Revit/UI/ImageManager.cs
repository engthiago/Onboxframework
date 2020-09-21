using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Onbox.Revit.V7.UI
{
    /// <summary>
    /// Deals with image conversions
    /// </summary>
    public class ImageManager
    {
        /// <summary>
        /// Converts an assembly resource to a BitmapImage
        /// </summary>
        public BitmapImage ConvertBitmapSource(string targetResourceName, Assembly assembly)
        {
            var stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(targetResourceName)));
            return ConvertToBitmapSource(stream) as BitmapImage;
        }

        /// <summary>
        /// Converts a stream to a BitmapSource
        /// </summary>
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

        /// <summary>
        /// Converts a Bitmap to a BitmapSource
        /// </summary>
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
