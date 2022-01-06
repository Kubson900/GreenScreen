using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenScreenUI
{
    public partial class GreenScreenForm : Form
    {
        int threadsNumberInUse = 8;
        string imagePath;
        bool useAssembler;
        private ImageHolder imageHolder;
        public GreenScreenForm()
        {
            InitializeComponent();
        }

        private void trackBarThreadsNumber_Scroll(object sender, EventArgs e)
        {
            threadsNumberInUse = trackBarThreadsNumber.Value;
            labelThreadsNumberPicked.Text = threadsNumberInUse.ToString();

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

        private void buttonRunProgram_Click(object sender, EventArgs e)
        {

        }

        private void buttonPickColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                textBoxColorPicked.BackColor = colorDialog.Color;
                buttonRunProgram.Enabled = true;
            }
        }
    }
}
