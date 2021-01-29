using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace MarkAble2
{

    /// <summary>
    /// Following code is based on "Resizing a Photographic image with GDI+ for .NET" By Joel Neubeck
    /// http://www.codeproject.com/csharp/imageresize.asp
    /// </summary>
    public static class ImageTools
    {
        /// <summary>
        /// Dimensions of the image
        /// </summary>
        public enum Dimensions
        {
            /// <summary>
            /// Width
            /// </summary>
            Width,
            /// <summary>
            /// Height
            /// </summary>
            Height
        }

        /// <summary>
        /// The anchor position from which to crop an image
        /// </summary>
        public enum AnchorPosition
        {
            /// <summary>
            /// Top
            /// </summary>
            Top,
            /// <summary>
            /// Centre
            /// </summary>
            Centre,
            /// <summary>
            /// Bottom
            /// </summary>
            Bottom,
            /// <summary>
            /// Left
            /// </summary>
            Left,
            /// <summary>
            /// Right
            /// </summary>
            Right
        }

        /// <summary>
        /// Simple scale of image by a percentage
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <param name="Percent"></param>
        /// <returns></returns>
        public static Image ScaleByPercent(Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        /// <summary>
        /// Scale to fit within a set dimension but preserve aspect ratio
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <param name="Size"></param>
        /// <param name="Dimension"></param>
        /// <returns></returns>
        public static Image ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
        {
            try
            {
                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;
                int sourceX = 0;
                int sourceY = 0;
                int destX = 0;
                int destY = 0;
                float nPercent = 0;

                switch (Dimension)
                {
                    case Dimensions.Width:
                        nPercent = ((float)Size / (float)sourceWidth);
                        break;
                    default:
                        nPercent = ((float)Size / (float)sourceHeight);
                        break;
                }

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

                grPhoto.Dispose();
                return bmPhoto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Scale preserving aspect ratio but pad appropriately with colour to fill set width and height
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static Image FixedSize(Image imgPhoto, int Width, int Height, Color colour )
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            //if we have to pad the height pad both the top and the bottom
            //with the difference between the scaled height and the desired height
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = (int)((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = (int)((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(colour);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        /// <summary>
        /// Crop the image to size, with given anchor position
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Anchor"></param>
        /// <returns></returns>
        public static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                switch (Anchor)
                {
                    case AnchorPosition.Top:
                        destY = 0;
                        break;
                    case AnchorPosition.Bottom:
                        destY = (int)(Height - (sourceHeight * nPercent));
                        break;
                    default:
                        destY = (int)((Height - (sourceHeight * nPercent)) / 2);
                        break;
                }
            }
            else
            {
                nPercent = nPercentH;
                switch (Anchor)
                {
                    case AnchorPosition.Left:
                        destX = 0;
                        break;
                    case AnchorPosition.Right:
                        destX = (int)(Width - (sourceWidth * nPercent));
                        break;
                    default:
                        destX = (int)((Width - (sourceWidth * nPercent)) / 2);
                        break;
                }
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static string SafeFileName(string aName)
        {
            StringBuilder sb = new StringBuilder(aName.Length);
            sb.Append(aName);
            sb.Replace(':', '_');
            sb.Replace('\\', '_');
            sb.Replace('/', '_');
            sb.Replace('?', '_');
            sb.Replace('*', '_');
            return sb.ToString();
        }
    }
}
