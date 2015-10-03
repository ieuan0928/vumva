using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Drawing;
using System.Reflection;

namespace BOMBS.UI.Foundation
{
    public class Helper
    {
        public static BitmapSource GetEmbeddedBitmapSource(Assembly assembly, string resourceString)
        {
            //return null;
            return Imaging.CreateBitmapSourceFromHBitmap(new Bitmap(assembly.GetManifestResourceStream(resourceString)).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());            
        }

        public static BitmapImage GetResourceBitmapSource(string uriString)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(string.Format("pack://application:,,,/{0}", uriString));
            logo.EndInit();

            return logo;
        }
    }
}
