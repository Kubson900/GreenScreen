﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace GreenScreenUI
{
    public partial class GreenScreenForm : Form
    {
        [DllImport(@"C:\\GreenScreen\\x64\\Release\\GreenScreenAsm.dll")]
        public static extern unsafe void removeGreenScreenAsm(byte* pixels, byte* rgbValues, int size);


        [DllImport(@"C:\\GreenScreen\\x64\\Release\\GreenScreenCpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void removeGreenScreenCpp(byte* pixels, byte* rgbValues, int size);


        int threadsNumberInUse = 8;
        string imagePath;
        bool useAssembler;
        private ImageHolder imageHolder;
        Color colorPicked;

        public GreenScreenForm()
        {
            InitializeComponent();
        }

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
                }
            }
        }

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

        private void TrackBarThreadsNumber_Scroll(object sender, EventArgs e)
        {
            threadsNumberInUse = trackBarThreadsNumber.Value;
            labelThreadsNumberPicked.Text = threadsNumberInUse.ToString();
        }

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
        }

        private string ConvertTimeToString(TimeSpan timeSpan)
        {
            string elapsedTime = String.Format("{0:00}:{1:000}", timeSpan.Seconds, timeSpan.Milliseconds);
            elapsedTime += " sec";

            return elapsedTime;
        }

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
