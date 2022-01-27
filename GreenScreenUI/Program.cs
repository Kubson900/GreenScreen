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

using System;
using System.Windows.Forms;

namespace GreenScreenUI
{
    /*
     *  Punkt wejsciowy programu
     *  Uruchamia GUI
     */
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GreenScreenForm());
        }
    }
}
