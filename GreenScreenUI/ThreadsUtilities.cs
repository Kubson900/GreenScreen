using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GreenScreenUI
{
    class ThreadsUtilities
    {
        public static List<byte[]> SplitPixelsToArrays(byte[] pixels, int size, int threadsAmount)
        {
            List<byte[]> splitPixelArray = new List<byte[]>();

            int moduloSizeThreadsAmount = size % threadsAmount;

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
                int elementsPerArray = (size - moduloSizeThreadsAmount) / threadsAmount;
                int startIndex = 0;
                for (int i = 0; i < threadsAmount - 1; i++)
                {
                    byte[] splitArray = new byte[elementsPerArray];
                    Array.Copy(pixels, startIndex, splitArray, 0, elementsPerArray);
                    splitPixelArray.Add(splitArray);
                    startIndex += elementsPerArray;
                }
                byte[] lastArrayWithExtraPixels = new byte[elementsPerArray + moduloSizeThreadsAmount];
                Array.Copy(pixels, startIndex, lastArrayWithExtraPixels, 0, elementsPerArray + moduloSizeThreadsAmount);
                splitPixelArray.Add(lastArrayWithExtraPixels);
            }

            return splitPixelArray;
        }
       
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

        public static void RunThreads(List<Thread> listOfThreads)
        {
            foreach (Thread thread in listOfThreads)
            {
                thread.Start();
                thread.Join();
            }
        }
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
