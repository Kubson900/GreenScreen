using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GreenScreenUI
{
    // klasa odpowiedzialna wielowatkowosc aplikacji
    class ThreadsUtilities
    {
        /*
         *  Metoda przyjmuje ciag pikseli ARGBARGB, wielkosc tej tablicy oraz wybrana ilosc watkow
         *  Rowno rozdziela czworki ARGB do tablic, aby kazdy watek mial tyle samo zadan do wykonania
         *  Jezeli nie jest mozliwe rowne rozdzielenie ostatni watek otrzymuje nadmiarowe piksele
         */
        public static List<byte[]> SplitPixelsToArrays(byte[] pixels, int size, int threadsAmount)
        {
            List<byte[]> splitPixelArray = new List<byte[]>();

            int pixelsIntoFours = size / 4;

            int moduloSizeThreadsAmount = pixelsIntoFours % threadsAmount;

            if (moduloSizeThreadsAmount == 0)
            {
                int elementsPerArray = size / threadsAmount;
                int startIndex = 0;
                for (int i = 0; i < threadsAmount; i++)
                {
                    byte[] splitArray = new byte[elementsPerArray];
                    Array.Copy(pixels, startIndex, splitArray, 0, elementsPerArray);
                    splitPixelArray.Add(splitArray);
                    startIndex += elementsPerArray;
                }
            }
            else
            {
                int elementsPerArray = (pixelsIntoFours - moduloSizeThreadsAmount) / threadsAmount * 4;
                int startIndex = 0;
                for (int i = 0; i < threadsAmount - 1; i++)
                {
                    byte[] splitArray = new byte[elementsPerArray];
                    Array.Copy(pixels, startIndex, splitArray, 0, elementsPerArray);
                    splitPixelArray.Add(splitArray);
                    startIndex += elementsPerArray;
                }
                byte[] lastArrayWithExtraPixels = new byte[elementsPerArray + moduloSizeThreadsAmount * 4];
                Array.Copy(pixels, startIndex, lastArrayWithExtraPixels, 0, elementsPerArray + moduloSizeThreadsAmount * 4);
                splitPixelArray.Add(lastArrayWithExtraPixels);
            }

            return splitPixelArray;
        }

        /*
         *  Metoda przyjmuje metode trojargumentowa, ktora nic nie zwraca, liste tablic wypelnionych rowno pikselami zgodnie z watkami
         *  oraz tablice wartosci rgb koloru wybranego przez uzytkownika
         *  Zwraca liste watkow z przypisanymi zadaniami gotowymi do wykonania
         */
        public static List<Thread> AssignTasksToThreads(Action<byte[], byte[], int> function, List<byte[]> splitPixelArray, byte[] arrayRgbColorBytes)
        {
            List<Thread> listOfThread = new List<Thread>();
            foreach (byte[] pixels in splitPixelArray)
            {
                Thread newThread = new Thread(() => { function(pixels, arrayRgbColorBytes, pixels.Length); });
                listOfThread.Add(newThread);
            }

            return listOfThread;

        }

        /*
         *  Metoda przyjmuje liste watkow z przypisanymi zadaniami do wykonania
         *  Dla kazdego watku wykonuje wskazanie zadanie
         */
        public static void RunThreads(List<Thread> listOfThreads)
        {
            foreach (Thread thread in listOfThreads)
            {
                thread.Start();
                thread.Join();
            }
        }

        /*
         *  Metoda przyjmuje liste tablic pikseli, dla ktorych wykonano algorytm usuwania tla
         *  Zwraca ciag pikseli ARGB
         */
        public static byte[] MergeArray(List<byte[]> splitPixelArray)
        {
            byte[] newPixelArray = new byte[splitPixelArray.Sum(array => array.Length)];
            int offset = 0;

            foreach (var elem in splitPixelArray)
            {
                elem.CopyTo(newPixelArray, offset);
                offset += elem.Length;
            }

            return newPixelArray;
        }
    }
}
