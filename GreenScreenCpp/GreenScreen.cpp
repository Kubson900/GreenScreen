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

extern "C"
{
	/*
	*	Funkcja przyjmuje adres pierwszego elementu ciagu pikseli ARGB
	*	adres pierwszego elementu wartosci RGB wybranego koloru
	*	rozmiar tablicy pikseli
	*	Jezeli kazda z wartosci RGB piksela oraz koloru wskazanego przez uzytkownika bedzie taka sama
	*	kazda z wartosci ARGB piksela rownana jest z zerem
	*/
	_declspec(dllexport) void removeGreenScreenCpp(unsigned char* pixels, unsigned char* rgbValues, int size)
	{
		for (int i = 0; i < size; i += 4)
		{
			if (pixels[i + 1] == rgbValues[0] &&
				pixels[i + 2] == rgbValues[1] &&
				pixels[i + 3] == rgbValues[2])
			{
				pixels[i] = 0; //A
				pixels[i + 1] = 0; //R
				pixels[i + 2] = 0; //G
				pixels[i + 3] = 0; //B
			}
		}
	}
}
