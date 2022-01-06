using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GreenScreenUI
{
    class ImageUtilities
    {
        //Required format to show result in GUI
        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        //Convert pixels from byte array to bitmap object
        public static Bitmap ToOutputBitmap(byte[] pixels, int width, int height)
        {
            Bitmap outputBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            int pixelIndex = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var pixelColor = Color.FromArgb(pixels[pixelIndex], pixels[pixelIndex + 1], pixels[pixelIndex + 2], pixels[pixelIndex + 3]);
                    outputBitmap.SetPixel(j, i, pixelColor);
                    pixelIndex += 4;
                }
            }
            return outputBitmap;
        }

        //Gets pixel ARGB values as Byte array
        public static byte[] ToPixels(Bitmap inputBitmap)
        {
            int width = inputBitmap.Width;
            int height = inputBitmap.Height;
            int arraySize = width * height * 4;

            byte[] pixels = new byte[arraySize];

            int pixelIndex = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pixels[pixelIndex] = inputBitmap.GetPixel(j, i).A;
                    pixels[pixelIndex + 1] = inputBitmap.GetPixel(j, i).R;
                    pixels[pixelIndex + 2] = inputBitmap.GetPixel(j, i).G;
                    pixels[pixelIndex + 3] = inputBitmap.GetPixel(j, i).B;

                    pixelIndex += 4;
                }
            }
            return pixels;
        }

        public static byte[] GetRGB(Color color)
        {
            if (color != null)
            {
                byte[] colorRgbBytes = new byte[3];
                colorRgbBytes[0] = color.R;
                colorRgbBytes[1] = color.G;
                colorRgbBytes[2] = color.B;

                return colorRgbBytes;
            }
            else
            {
                return null;
            }
        }
    }
}
