using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace GreenScreenUI
{
    public partial class GreenScreenForm : Form
    {
        [DllImport(@"C:\\GreenScreen\\x64\\Debug\\GreenScreenAsm.dll")]
        public static extern unsafe void processPictureAssembler(byte* pixelArray, byte* colorRgbBytes, int size);

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
                //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

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

        private void RunAsmDll(byte[] pixelArray, byte[] colorToRemoveRgb, int size)
        {
            unsafe
            {
                fixed (byte* colorToRemoveRgbPtr = &colorToRemoveRgb[0])
                {
                    fixed (byte* pixelArrayPtr = &pixelArray[0])
                    {
                        processPictureAssembler(pixelArrayPtr, colorToRemoveRgbPtr, size);
                    }
                }
            }
        }

        private void ButtonRunProgram_Click(object sender, EventArgs e)
        {
            imageHolder.Pixels = ImageUtilities.ToPixels(imageHolder.InputImage);
            byte[] colorPickedRGB = ImageUtilities.GetRGB(colorPicked);

            Stopwatch stopWatch = new Stopwatch();

            if (threadsNumberInUse == 1)
            {
                if (useAssembler)
                {
                    stopWatch.Start();
                    RunAsmDll(imageHolder.Pixels, colorPickedRGB, imageHolder.GetPixelsSize());
                    stopWatch.Stop();
                }
                else
                {
                    stopWatch.Start();
                    GreenScreenRemover greenScreenRemover = new GreenScreenRemover();
                    greenScreenRemover.processPicture(imageHolder.Pixels, colorPickedRGB, imageHolder.GetPixelsSize());
                    stopWatch.Stop();
                }
            }
            else
            {
                List<byte[]> arrayList = ThreadsUtilities.SplitPixelsToArrays(imageHolder.Pixels, imageHolder.GetPixelsSize(), threadsNumberInUse);

                if (useAssembler)
                {
                    List<Thread> listOfThreads = ThreadsUtilities.AssignTasksToThreads(new Action<byte[], byte[], int>(this.RunAsmDll), arrayList, colorPickedRGB);
                    stopWatch.Start();
                    ThreadsUtilities.RunThreads(listOfThreads);
                    stopWatch.Stop();
                }
                else
                {
                    GreenScreenRemover greenScreenRemover = new GreenScreenRemover();
                    List<Thread> listOfThreads = ThreadsUtilities.AssignTasksToThreads(new Action<byte[], byte[], int>(greenScreenRemover.processPicture), arrayList, colorPickedRGB);
                    stopWatch.Start();
                    ThreadsUtilities.RunThreads(listOfThreads);
                    stopWatch.Stop();
                }
                imageHolder.Pixels = ThreadsUtilities.MergeArray(arrayList);
            }

            labelTimeElapsed.Text = ConvertTimeToString(stopWatch.Elapsed);
            imageHolder.OutputImage = ImageUtilities.ToOutputBitmap(imageHolder.Pixels, imageHolder.GetInputWidth(), imageHolder.GetInputHeight());
            rightPictureAfter.Image = imageHolder.OutputImage;
        }

        private string ConvertTimeToString(TimeSpan timeSpan)
        {
            string elapsedTime = String.Format("{0:00}:{1:000}", timeSpan.Seconds, timeSpan.Milliseconds);
            elapsedTime += " sec";

            return elapsedTime;
        }
    }
}
