using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ServiceStack.Extensions
{
    public static class ImageExtensions
    {
        #region 保存图片

        #endregion

        #region 图片处理

        /// <summary>
        ///     计算图片宽高比。
        /// </summary>
        public static float GetAspectRatio(this byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                using (var image = (Bitmap) Image.FromStream(stream))
                {
                    return image.Width / (float) image.Height;
                }
            }
        }

        /// <summary>
        ///     计算图片宽高比。
        /// </summary>
        public static float GetAspectRatio(this string file)
        {
            using (var image = (Bitmap) Image.FromFile(file))
            {
                return image.Width / (float) image.Height;
            }
        }

        /// <summary>
        ///     调整图片大小。
        /// </summary>
        public static byte[] ResizeImage(this byte[] buffer, int? width, int? height, bool preservePerspective, ImageFormat format)
        {
            using (var stream = new MemoryStream(buffer))
            {
                using (var image = (Bitmap) Image.FromStream(stream))
                {
                    using (var outputImage = Resize(image, width, height, preservePerspective))
                    {
                        return Encode(outputImage, format);
                    }
                }
            }
        }

        /// <summary>
        ///     自动旋转图像。
        /// </summary>
        public static byte[] AutoRotateImage(this byte[] buffer, ImageFormat format)
        {
            using (var stream = new MemoryStream(buffer))
            {
                using (var image = (Bitmap) Image.FromStream(stream))
                {
                    // http://msdn.microsoft.com/en-us/library/system.drawing.imaging.propertyitem.id.aspx
                    const int propertyTagOrientation = 0x0112;
                    var propertyItem = image.PropertyItems.SingleOrDefault(item => item.Id == propertyTagOrientation);
                    var orientation = propertyItem?.Value[0] ?? 1;
                    // http://sylvana.net/jpegcrop/exif_orientation.html
                    var transform = new List<RotateFlipType>
                                    {
                                        RotateFlipType.RotateNoneFlipNone, // 1 
                                        RotateFlipType.RotateNoneFlipX, // 2
                                        RotateFlipType.Rotate180FlipNone, // 3
                                        RotateFlipType.RotateNoneFlipY, // 4
                                        RotateFlipType.Rotate90FlipX, // 5
                                        RotateFlipType.Rotate90FlipNone, // 6
                                        RotateFlipType.Rotate270FlipX, // 7
                                        RotateFlipType.Rotate270FlipNone // 8
                                    };
                    if (transform[orientation - 1] != RotateFlipType.RotateNoneFlipNone)
                    {
                        image.RotateFlip(transform[orientation - 1]);
                        if (propertyItem != null)
                        {
                            propertyItem.Value[0] = 1;
                            image.SetPropertyItem(propertyItem);
                        }
                    }
                    return Encode(image, format);
                }
            }
        }

        #endregion

        #region 内部图片处理

        private static Bitmap Resize(Bitmap sourceBitmap, int? width, int? height, bool preservePerspective)
        {
            var newWidth = width.HasValue ? Convert.ToDouble(width.Value) : 0;
            var newHeight = height.HasValue ? Convert.ToDouble(height.Value) : 0;
            if (preservePerspective)
            {
                if (newWidth > 0 && newHeight <= 0)
                {
                    newHeight = newWidth / sourceBitmap.Width * sourceBitmap.Height;
                }
                else if (newHeight > 0 && newWidth <= 0)
                {
                    newWidth = newHeight / sourceBitmap.Height * sourceBitmap.Width;
                }
            }
            if (newHeight <= 0)
            {
                newHeight = 1;
            }
            if (newWidth <= 0)
            {
                newWidth = 1;
            }
            var bitmap = new Bitmap((int) newWidth, (int) newHeight);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(sourceBitmap, 0, 0, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));
                return bitmap;
            }
        }

        private static Bitmap Zoom(Bitmap sourceBitmap, float zoomFactor)
        {
            var width = Convert.ToInt32(sourceBitmap.Width * zoomFactor);
            var height = Convert.ToInt32(sourceBitmap.Height * zoomFactor);
            return Resize(sourceBitmap, width, height, true);
        }

        private static Bitmap ToGreyScale(Bitmap sourceBitmap)
        {
            var tempBitmap = new Bitmap(sourceBitmap, sourceBitmap.Width, sourceBitmap.Height);
            using (var newGraphics = Graphics.FromImage(tempBitmap))
            {
                float[][] floatColorMatrix = { new[] { .3f, .3f, .3f, 0, 0 }, new[] { .59f, .59f, .59f, 0, 0 }, new[] { .11f, .11f, .11f, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, 0, 1 } };
                var newColorMatrix = new ColorMatrix(floatColorMatrix);
                using (var attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(newColorMatrix);
                    newGraphics.DrawImage(tempBitmap, new Rectangle(0, 0, tempBitmap.Width, tempBitmap.Height), 0, 0, tempBitmap.Width, tempBitmap.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return tempBitmap;
        }

        private static byte[] Encode(Bitmap bitmap, ImageFormat format)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, format);
                return stream.ToArray();
            }
        }

        private static Bitmap Rotate(Bitmap bitmap, float angle)
        {
            const double pi2 = Math.PI / 2.0D;
            double oldWidth = bitmap.Width;
            double oldHeight = bitmap.Height;

            // Convert degrees to radians
            var theta = angle * Math.PI / 180.0D;
            var lockedTheta = theta;
            // Ensure theta is now [0, 2pi)
            while (lockedTheta < 0.0D)
            {
                lockedTheta += 2.0D * Math.PI;
            }
            double adjacentTop, oppositeTop;
            double adjacentBottom, oppositeBottom;
            // We need to calculate the sides of the triangles based
            // on how much rotation is being done to the bitmap.
            //   Refer to the first paragraph in the explaination above for 
            //   reasons why.
            if (lockedTheta >= 0.0D && lockedTheta < pi2 || lockedTheta >= Math.PI && lockedTheta < Math.PI + pi2)
            {
                adjacentTop = Math.Abs(Math.Cos(lockedTheta)) * oldWidth;
                oppositeTop = Math.Abs(Math.Sin(lockedTheta)) * oldWidth;
                adjacentBottom = Math.Abs(Math.Cos(lockedTheta)) * oldHeight;
                oppositeBottom = Math.Abs(Math.Sin(lockedTheta)) * oldHeight;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(lockedTheta)) * oldHeight;
                oppositeTop = Math.Abs(Math.Cos(lockedTheta)) * oldHeight;
                adjacentBottom = Math.Abs(Math.Sin(lockedTheta)) * oldWidth;
                oppositeBottom = Math.Abs(Math.Cos(lockedTheta)) * oldWidth;
            }
            var newWidth = adjacentTop + oppositeBottom;
            var newHeight = adjacentBottom + oppositeTop;
            var nWidth = (int) newWidth;
            var nHeight = (int) newHeight;
            var rotatedBmp = new Bitmap(nWidth, nHeight);
            using (var graphics = Graphics.FromImage(rotatedBmp))
            {
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // This array will be used to pass in the three points that 
                // make up the rotated source
                PointF[] points;
                if (lockedTheta >= 0.0D && lockedTheta < pi2)
                {
                    points = new[] { new PointF((float) oppositeBottom, 0.0F), new PointF((float) newWidth, (float) oppositeTop), new PointF(0.0F, (float) adjacentBottom) };
                }
                else if (lockedTheta >= pi2 && lockedTheta < Math.PI)
                {
                    points = new[] { new PointF((float) newWidth, (float) oppositeTop), new PointF((float) adjacentTop, (float) newHeight), new PointF((float) oppositeBottom, 0.0F) };
                }
                else if (lockedTheta >= Math.PI && lockedTheta < Math.PI + pi2)
                {
                    points = new[] { new PointF((float) adjacentTop, (float) newHeight), new PointF(0.0F, (float) adjacentBottom), new PointF((float) newWidth, (float) oppositeTop) };
                }
                else
                {
                    points = new[] { new PointF(0.0F, (float) adjacentBottom), new PointF((float) oppositeBottom, 0.0F), new PointF((float) adjacentTop, (float) newHeight) };
                }
                graphics.DrawImage(bitmap, points);
            }
            return rotatedBmp;
        }

        #endregion
    }
}