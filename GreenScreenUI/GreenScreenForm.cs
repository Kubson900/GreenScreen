using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GreenScreenUI
{
    public partial class GreenScreenForm : Form
    {
        int threadsNumberInUse = 8;
        string imagePath;
        bool useAssembler;
        private ImageHolder imageHolder;
        Color colorPicked;

        public GreenScreenForm()
        {
            InitializeComponent();
        }

        private void buttonUploadPicture_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
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

        private void buttonPickColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                textBoxColorPicked.BackColor = colorDialog.Color;
                colorPicked = colorDialog.Color;
                buttonRunProgram.Enabled = true;
            }
        }

        private void checkBoxUseAssembler_CheckedChanged(object sender, EventArgs e)
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

        private void trackBarThreadsNumber_Scroll(object sender, EventArgs e)
        {
            threadsNumberInUse = trackBarThreadsNumber.Value;
            labelThreadsNumberPicked.Text = threadsNumberInUse.ToString();
        }

        private void buttonRunProgram_Click(object sender, EventArgs e)
        {
            imageHolder.Pixels = ImageUtilities.ToPixels(imageHolder.InputImage);
            byte[] colorPickedRGB = ImageUtilities.GetRGB(colorPicked);

            Stopwatch stopWatch = new Stopwatch();
            if (useAssembler)
            {
                // TODO
            }
            else
            {
                stopWatch.Start();
                GreenScreenRemover greenScreenRemover = new GreenScreenRemover();
                greenScreenRemover.processPicture(imageHolder.Pixels, colorPickedRGB, imageHolder.GetPixelsSize());
                stopWatch.Stop();
            }

            labelTimeElapsed.Text = convertTimeToString(stopWatch.Elapsed);

            imageHolder.OutputImage = ImageUtilities.ToOutputBitmap(imageHolder.Pixels, imageHolder.GetInputWidth(), imageHolder.GetInputHeight());
            rightPictureAfter.Image = imageHolder.OutputImage;
        }

        private string convertTimeToString(TimeSpan timeSpan)
        {
            string elapsedTime = String.Format("{0:00}:{1:000}", timeSpan.Seconds, timeSpan.Milliseconds);
            elapsedTime += " sec";

            return elapsedTime;
        }
    }
}
