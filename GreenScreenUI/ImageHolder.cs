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
