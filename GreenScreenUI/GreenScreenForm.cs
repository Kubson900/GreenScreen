using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace GreenScreenUI
{
    /*
     *  Klasa obslugujaca GUI 
     */
    public partial class GreenScreenForm : Form
    {
        /*
         *  Dynamiczne zaimportowanie biblioteki DLL dla Assemblera
         */
        [DllImport(@"C:\\GreenScreen\\x64\\Release\\GreenScreenAsm.dll")]

        /*
         *  Metoda przyjmuje wskaznik na ciag pikseli ARGB, wskaznik na wartosci RGB wybranego koloru, rozmiar tablicy pikseli
         *  Jest odpowiedzialna za usuwanie tla obrazu za pomoca Assemblera
         */
        public static extern unsafe void removeGreenScreenAsm(byte* pixels, byte* rgbValues, int size);

        /*
         *  Dynamiczne zaimportowanie biblioteki DLL dla Cpp
         */
        [DllImport(@"C:\\GreenScreen\\x64\\Release\\GreenScreenCpp.dll", CallingConvention = CallingConvention.Cdecl)]

        /*
         *  Metoda przyjmuje wskaznik na ciag pikseli ARGB, wskaznik na wartosci RGB wybranego koloru, rozmiar tablicy pikseli
         *  Jest odpowiedzialna za usuwanie tla obrazu za pomoca Cpp
         */
        public static extern unsafe void removeGreenScreenCpp(byte* pixels, byte* rgbValues, int size);

        /*
         *  Ilosc watkow wybranych przez uzytkownika dla ktorych ma zostac wykonany program
         */
        int threadsNumberInUse = Environment.ProcessorCount;

        /*
         *  Sciezka do zdjecia
         */
        string imagePath;

        /*
         *  Wartosc boolowska czy program ma uzyc Assembler
         */
        bool useAssembler;

        /*
         *  Obiekt klasy ImageHolder, ktory obsluguje zdjecia na wejsciu i wyjsciu 
         */
        private ImageHolder imageHolder;

        /*
         *  Obiekt klasy Color, ktory reprezentuje kolor do usuniecia z tla wybrany przez uzytkownika 
         */
        Color colorPicked;

        /*
         *  Metoda, ktora przygotowywuje wszystkie komponenty do uzycia 
         */
        public GreenScreenForm()
        {
            InitializeComponent();
        }

        /*
         *  Metoda jest wywolywana po kliknieciu w przycisk Upload
         *  Otwiera okno dialogowe i pozwala wybrac zdjecie we wskazanym formacie do zaladowania przez program
         *  Wyswietla stosowny komunikat, jezeli zdjecie nie zostanie wybrane
         *  Umieszcza bitmape w lewym boxie na ekranie
         *  Zapisuje zdjecie skonwertowane do bitmapy w ImageHolderze
         *  Wyswietla sciezke do pliku
         *  Odblokowuje mozliwosc wyboru koloru
         */
        private void ButtonUploadPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\GreenScreen\\Examples";
                openFileDialog.Filter = "Image Files| *.bmp; *.jpg; *.png;| JPG | *.jpg | BMP | *.bmp | PNG | *.png";
                openFileDialog.RestoreDirectory = true;

                openFileDialog.ShowDialog();

                if (String.IsNullOrEmpty(openFileDialog.FileName))
                {
                    MessageBox.Show("File hasn't been selected!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    imagePath = openFileDialog.FileName;
                    leftPictureBefore.ImageLocation = imagePath;
                    imageHolder = new ImageHolder { InputImage = new Bitmap(imagePath) };
                    buttonPickColor.Enabled = true;
                    LabelFileLocation.Text = imagePath;
                }
            }
        }

        /*
         *  Metoda wywolywana jest po kliknieciu w przycisk Pick a Color
         *  Otwiera okno dialogowe i pozwala wybrac kolor do zaladowania przez program
         *  Zapisuje kolor, umieszcza kolor w boxie ponizej przycisku
         *  Odblokowuje mozliwosc wyboru odpalenia programu oraz wygenerowania raportu
         */
        private void ButtonPickColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                textBoxColorPicked.BackColor = colorDialog.Color;
                colorPicked = colorDialog.Color;
                buttonRunProgram.Enabled = true;
                buttonGenerateRaport.Enabled = true;
            }
        }

        /*
         *  Metoda wywolywana jest po zaznaczeniu boxa Use Assembler
         *  Zmienia wartosc boolowska useAssembler
         */
        private void CheckBoxUseAssembler_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUseAssembler.Checked)
            {
                useAssembler = true;
            }
            else
            {
                useAssembler = false;
            }
        }

        /*
         *  Metoda zapisuje ilosc watkow wybranych na scroll'u
         *  Umieszcza ta wartosc nad scroll'em
         */
        private void TrackBarThreadsNumber_Scroll(object sender, EventArgs e)
        {
            threadsNumberInUse = trackBarThreadsNumber.Value;
            labelThreadsNumberPicked.Text = threadsNumberInUse.ToString();
        }

        /*
         *  Metoda pelni role przejsciowki
         *  Tlumaczy podane zmienne na adresy pierwszych elementow zmiennych
         *  Wywoluje metode usuwajaca tlo z uzyciem assemblera
         */
        private void RunAsmDll(byte[] pixels, byte[] rgbValues, int size)
        {
            unsafe
            {
                fixed (byte* rgbValuesPtr = &rgbValues[0])
                {
                    fixed (byte* pixelsPtr = &pixels[0])
                    {
                        removeGreenScreenAsm(pixelsPtr, rgbValuesPtr, size);
                    }
                }
            }
        }

        /*
         *  Metoda pelni role przejsciowki
         *  Tlumaczy podane zmienne na adresy pierwszych elementow zmiennych
         *  Wywoluje metode usuwajaca tlo z uzyciem cpp
         */
        private void RunCppDll(byte[] pixels, byte[] rgbValues, int size)
        {
            unsafe
            {
                fixed (byte* rgbValuesPtr = &rgbValues[0])
                {
                    fixed (byte* pixelsPtr = &pixels[0])
                    {
                        removeGreenScreenCpp(pixelsPtr, rgbValuesPtr, size);
                    }
                }
            }
        }

        /*
         *  Metoda jest wywolywana po kliknieciu przycisku Run
         *  Konwertuje bitmape ze zdjecia wejsciowego na ciag pikseli ARGB
         *  Konwertuje wybrany kolor na ciag wartosci RGB
         *  Dla wielowatkowosci dzieli rowno zadania miedzy kazdy watek
         *  Mierzy czas wykonania algorytmu zarówno dla Assemblera jak i Cpp
         *  Czas zostaje umieszczony pod przyciskiem Run
         *  Konwertuje przetworzone piksele do bitmapy i umieszcza je na ekranie w prawym boxie
         *  Odblokowuje przycisk zapisu zdjecia
         *  Wyswietla komunikat o zakonczonym dzialaniu
         */
        private void ButtonRunProgram_Click(object sender, EventArgs e)
        {
            imageHolder.Pixels = ImageUtilities.ToPixels(imageHolder.InputImage);
            byte[] colorPickedInRGB = ImageUtilities.GetRGB(colorPicked);

            Stopwatch stopWatch = new Stopwatch();

            if (threadsNumberInUse == 1)
            {
                if (useAssembler)
                {
                    stopWatch.Start();
                    RunAsmDll(imageHolder.Pixels, colorPickedInRGB, imageHolder.GetPixelsSize());
                    stopWatch.Stop();
                }
                else
                {
                    stopWatch.Start();
                    RunCppDll(imageHolder.Pixels, colorPickedInRGB, imageHolder.GetPixelsSize());
                    stopWatch.Stop();
                }
            }
            else
            {
                List<byte[]> arrayList = ThreadsUtilities.SplitPixelsToArrays(imageHolder.Pixels, imageHolder.GetPixelsSize(), threadsNumberInUse);

                if (useAssembler)
                {
                    List<Thread> listOfThreads = ThreadsUtilities.AssignTasksToThreads(new Action<byte[], byte[], int>(this.RunAsmDll), arrayList, colorPickedInRGB);
                    stopWatch.Start();
                    ThreadsUtilities.RunThreads(listOfThreads);
                    stopWatch.Stop();
                }
                else
                {
                    List<Thread> listOfThreads = ThreadsUtilities.AssignTasksToThreads(new Action<byte[], byte[], int>(this.RunCppDll), arrayList, colorPickedInRGB);
                    stopWatch.Start();
                    ThreadsUtilities.RunThreads(listOfThreads);
                    stopWatch.Stop();
                }
                imageHolder.Pixels = ThreadsUtilities.MergeArray(arrayList);
            }

            labelTimeElapsed.Text = ConvertTimeToString(stopWatch.Elapsed);
            imageHolder.OutputImage = ImageUtilities.ToOutputBitmap(imageHolder.Pixels, imageHolder.GetInputWidth(), imageHolder.GetInputHeight());
            rightPictureAfter.Image = imageHolder.OutputImage;
            ButtonSave.Enabled = true;
            MessageBox.Show("Your image is ready!", "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /*
         *  Metoda przyjmuje czas otrzymany od stopwatch'a
         *  Konwertuje go do odpowiedniego formatu i zwraca
         */
        private string ConvertTimeToString(TimeSpan timeSpan)
        {
            string elapsedTime = String.Format("{0:00}:{1:000}", timeSpan.Seconds, timeSpan.Milliseconds);
            elapsedTime += " sec";

            return elapsedTime;
        }

        /*
         *  Metoda jest wywolywana po wcisnieciu przycisku Save
         *  Otwiera okno dialogowe, z ktorego mozna wybrac miejsce do zapisania przetworzonego zdjecia z wybranym rozszerzeniem
         *  Zapisuje zdjecie
         */
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Image Files|*.bmp;*.jpg;*.png;|JPG|*.jpg|BMP|*.bmp|PNG|*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            saveFileDialog.ShowDialog();

            ImageUtilities.SaveImageToFile(saveFileDialog.FileName, imageHolder.OutputImage);
        }

        /*
         *  Metoda jest wywolywana po wcisnieciu przycisku Generate a Raport
         *  Generuje plik tekstowy zawierajacy porownanie czasowe wykonania algorytmu
         *  dla implementacji w Asm oraz Cpp dla kazdego watku
         */
        private void buttonGenerateRaport_Click(object sender, EventArgs e)
        {
            const int maxAmountOfThreads = 16;
            byte[] colorPickedInRGB = ImageUtilities.GetRGB(colorPicked);
            imageHolder.Pixels = ImageUtilities.ToPixels(imageHolder.InputImage);
            Stopwatch stopWatch = new Stopwatch();

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            saveFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                MessageBox.Show("File hasn't been selected!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);

            streamWriter.WriteLine("Assembler");
            for (int i = 0; i < maxAmountOfThreads; i++)
            {
                if (i == 0)
                {
                    stopWatch.Start();
                    RunAsmDll(imageHolder.Pixels, colorPickedInRGB, imageHolder.GetPixelsSize());
                    stopWatch.Stop();
                }
                else
                {
                    List<byte[]> arrayList = ThreadsUtilities.SplitPixelsToArrays(imageHolder.Pixels, imageHolder.GetPixelsSize(), i + 1);

                    List<Thread> listOfThreads = ThreadsUtilities.AssignTasksToThreads(new Action<byte[], byte[], int>(this.RunAsmDll), arrayList, colorPickedInRGB);
                    stopWatch.Start();
                    ThreadsUtilities.RunThreads(listOfThreads);
                    stopWatch.Stop();
                }
                streamWriter.WriteLine($"Threads: {i + 1,2}, Time: {ConvertTimeToString(stopWatch.Elapsed)}");
            }

            streamWriter.WriteLine("\nCpp");
            for (int i = 0; i < maxAmountOfThreads; i++)
            {
                if (i == 0)
                {
                    stopWatch.Start();
                    RunCppDll(imageHolder.Pixels, colorPickedInRGB, imageHolder.GetPixelsSize());
                    stopWatch.Stop();
                }
                else
                {
                    List<byte[]> arrayList = ThreadsUtilities.SplitPixelsToArrays(imageHolder.Pixels, imageHolder.GetPixelsSize(), i + 1);

                    List<Thread> listOfThreads = ThreadsUtilities.AssignTasksToThreads(new Action<byte[], byte[], int>(this.RunCppDll), arrayList, colorPickedInRGB);
                    stopWatch.Start();
                    ThreadsUtilities.RunThreads(listOfThreads);
                    stopWatch.Stop();
                }
                streamWriter.WriteLine($"Threads: {i + 1,2}, Time: {ConvertTimeToString(stopWatch.Elapsed)}");
            }
            streamWriter.Flush();
            streamWriter.Close();
            MessageBox.Show("Your raport is ready!", "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
