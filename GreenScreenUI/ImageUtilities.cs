/*
 *  Temat projektu: GreenScreen - usuwanie wybranego koloru ze zdjecia
 *  Autor:          Jakub Polczyk
 *  Semestr:        5
 *  Rok Akademicki: 2021/2022
 *  Opis algorytmu: Zdjecie konwertowane jest na postac Bitmapy, a nastepnie na ciag pikseli w postaci wartosci ARGB
 *                  Dla wielowatkowosci - wartosci te sa rowno dzielone na tablice wedlug ilosci uzuwanych watkow.
 *                  Nastepnie dla kazdej takiej tablicy kazdy piksel (RGB) porownywany jest z kolorem (RGB) wybranym przez uzytkownika.
 *                  Jezeli kazda z wartosci jest taka sama, wszystkie wartosci ARGB sa ustawiane na 0.
 *                  W kolenym kroku tablice sa ze soba zlaczane, konwertowane do postaci Bitmapy i do docelowego formatu zdjecia
 *  Wersja:         Final Ultimate 1.0
 */

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace GreenScreenUI
{
    /*
     *  Klasa zawiera przydatne metody do operowania na zdjeciach
     */
    class ImageUtilities
    {
        /*
         *  Metoda przyjmuje bitmape
         *  Dostosowywuje bitmape pod GUI
         */
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

        /*
         *  Metoda przyjmuje bitmape
         *  Zwraca ciag pikseli ARGBARGB
         */
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

        /*
         *  Metoda przyjmuje kolor wskazany przez uzytkownika przez GUI
         *  Zwraca ciag wartosci RGB
         */
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

        /*
         *  Metoda przyjmuje ciag pikseli ARGB, szerokosc oraz wysokosc zdjecia
         *  Zwraca bitmape
         */
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

        /*
         *  Metoda przyjmuje sciezke z rozszerzeniem pliku oraz bitmape
         *  Zapisuje zdjecie w wybranym formacie do wskazanej lokalizacji
         */
        public static void SaveImageToFile(string pathToSave, Bitmap bitmap)
        {
            //If not null
            if (string.IsNullOrEmpty(pathToSave) || bitmap is null)
                return;

            var extension = Path.GetExtension(pathToSave);
            //Save in chosen format
            switch (extension)
            {
                case ".jpg":
                    bitmap.Save(pathToSave, ImageFormat.Jpeg);
                    break;
                case ".bmp":
                    bitmap.Save(pathToSave, ImageFormat.Bmp);
                    break;
                case ".png":
                    bitmap.Save(pathToSave, ImageFormat.Png);
                    break;
            }
        }
    }
}
