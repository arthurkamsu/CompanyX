using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace System.Drawing
{
    public static class ImageExtension
    {

        public static void SaveWithEncoder(this Image image,string path, int quality)
        {
            var qualityParamId = Encoder.Quality;
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
            var codec = ImageCodecInfo.GetImageDecoders()
                .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
            image.Save(path,codec,encoderParameters);
        }

    }
}
