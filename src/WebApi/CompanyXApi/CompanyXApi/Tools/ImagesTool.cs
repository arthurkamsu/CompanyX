using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace CompanyXApi.Tools
{
    public static class ImagesTool
    {
        public static Image Base64ToImage(string base64String)
        {          
            //check the OS type and apply a strategy pattern by converting, according to the OS type
            //runtime.osx.10.10-x64.CoreCompat.System.Drawing, and runtime.linux-x64.CoreCompat.System.Drawing

            // Convert base 64 string to byte[]
            
            byte[] imageBytes = Convert.FromBase64String(base64String.Substring(base64String.IndexOf(',') + 1));//if there is no , it starts at 0
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to base 64 string
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static Image ResizeImage(Image imageToResize,int neededSize,int quality)
        {
            int width, height;
            if (imageToResize.Width > imageToResize.Height)
            {
                width = neededSize;
                height = Convert.ToInt32(imageToResize.Height * neededSize / (double)imageToResize.Width);
            }
            else
            {
                width = Convert.ToInt32(imageToResize.Width * neededSize / (double)imageToResize.Height);
                height = neededSize;
            }
            var resized = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(resized))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(imageToResize, 0, 0, width, height);    
                
            }

            return resized;            
        }

    }
}
