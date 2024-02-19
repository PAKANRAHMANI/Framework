using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Framework.AspNetCore.Models;

namespace Framework.AspNetCore
{
    public static class ThumbnailHelper
    {
        public static string GenerateThumbnail(string filename, Dimension dimension)
        {
            if (dimension.Height > 0)
                return GenerateThumbnail(filename, dimension.Width, dimension.Height, true);

            return GenerateThumbnail(filename, dimension.Width, dimension.Width, false);
        }

        private static string GenerateThumbnail(string filename, int width, int height, bool force)
        {
            var thumb = CreateThumbnail(filename, width, height, force);

            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, 60L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            var directory = Path.GetDirectoryName(filename);
            var extension = Path.GetExtension(filename);
            var name = GetThumbnailName(filename, width, height, force);
            var fullname = Path.Combine(directory, name);

            var codecInfo = GetCodecInfo(extension);
            thumb.Save(fullname, codecInfo, myEncoderParameters);

            return fullname;
        }

        public static string GetThumbnailName(string filename, int width, int height,
            bool force)
        {
            var name = Path.GetFileNameWithoutExtension(filename);
            var extension = Path.GetExtension(filename);

            var forceStr = force ? "_Force" : "";
            name += $"_{width}_{height}_{forceStr}{extension}";
            return name;
        }

        private static ImageCodecInfo GetCodecInfo(string ext)
        {
            ext = ext.ToUpper();
            switch (ext)
            {
                case ".JPG":
                case ".JPEG":
                    return GetEncoder(ImageFormat.Jpeg);
                case ".PNG":
                    return GetEncoder(ImageFormat.Png);
                case ".GIF":
                    return GetEncoder(ImageFormat.Gif);
                case ".BMP":
                    return GetEncoder(ImageFormat.Bmp);
                case ".ICO":
                    return GetEncoder(ImageFormat.Icon);
                default:
                    return GetEncoder(ImageFormat.Jpeg);
            }
        }
        private static Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight, bool forceSize)
        {
            Bitmap bmpOut = null;
            try
            {

                var loBmp = new Bitmap(lcFilename);
                loBmp.SetResolution(72, 72);

                var lnNewWidth = 0;
                var lnNewHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (loBmp.Width < lnWidth && loBmp.Height < lnHeight)
                    return loBmp;

                if (!forceSize)
                {
                    decimal lnRatio;
                    if (loBmp.Width > loBmp.Height)
                    {
                        lnRatio = (decimal)lnWidth / loBmp.Width;
                        lnNewWidth = lnWidth;
                        var lnTemp = loBmp.Height * lnRatio;
                        lnNewHeight = (int)lnTemp;
                    }
                    else
                    {
                        lnRatio = (decimal)lnHeight / loBmp.Height;
                        lnNewHeight = lnHeight;
                        var lnTemp = loBmp.Width * lnRatio;
                        lnNewWidth = (int)lnTemp;
                    }
                }
                else
                {
                    lnNewWidth = lnWidth;
                    lnNewHeight = lnHeight;
                }
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                var graphics = Graphics.FromImage(bmpOut);
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                graphics.DrawImage(loBmp, 0, 0, lnNewWidth, lnNewHeight);
                loBmp.Dispose();
            }
            catch
            {
                return null;
            }

            return bmpOut;
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            var codecs = ImageCodecInfo.GetImageDecoders();

            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}
