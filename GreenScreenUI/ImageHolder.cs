using System.Drawing;

namespace GreenScreenUI
{
    /*
     *  Klasa zawiera miejsce do przechowywania zdjecia przed oraz po zastosowaniu algorytmu
     *  Pozwala to na wielokrotne wywolywanie operacji na danych wejsciowych
     */
    class ImageHolder
    {
        /*
         *  Metoda pobiera i ustawia bitmape z danych wejsciowych
         */
        public Bitmap InputImage { get; set; }

        /*
         *  Metoda pobiera i ustawia bitmape dla danych wyjsciowych 
         */
        public Bitmap OutputImage { get; set; }

        /*
         *  Metoda pobiera i ustawia ciag pikseli ARGB 
         */
        public byte[] Pixels { get; set; }

        /*
         *  Metoda zwraca rozmiar tablicy pikseli 
         */
        public int GetPixelsSize()
        {
            return Pixels.Length;
        }

        /*
         *  Metoda zwraca wysokosc zdjecia 
         */
        public int GetInputHeight()
        {
            return InputImage.Height;
        }

        /*
         *  Metoda zwraca szerokosc zdjecia 
         */
        public int GetInputWidth()
        {
            return InputImage.Width;
        }
    }
}
