using System;

namespace GreenScreenUI
{
    partial class GreenScreenForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.leftPictureBefore = new System.Windows.Forms.PictureBox();
            this.rightPictureAfter = new System.Windows.Forms.PictureBox();
            this.checkBoxUseAssembler = new System.Windows.Forms.CheckBox();
            this.buttonUploadPicture = new System.Windows.Forms.Button();
            this.buttonRunProgram = new System.Windows.Forms.Button();
            this.trackBarThreadsNumber = new System.Windows.Forms.TrackBar();
            this.labelBefore = new System.Windows.Forms.Label();
            this.labelAfter = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.buttonPickColor = new System.Windows.Forms.Button();
            this.textBoxColorPicked = new System.Windows.Forms.TextBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelThreads = new System.Windows.Forms.Label();
            this.labelThreadsNumberPicked = new System.Windows.Forms.Label();
            this.labelTimeElapsed = new System.Windows.Forms.Label();
            this.ButtonSave = new System.Windows.Forms.Button();
            this.buttonGenerateRaport = new System.Windows.Forms.Button();
            this.LabelChooseFile = new System.Windows.Forms.Label();
            this.LabelFileLocation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.leftPictureBefore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightPictureAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreadsNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // leftPictureBefore
            // 
            this.leftPictureBefore.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.leftPictureBefore.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.leftPictureBefore.Location = new System.Drawing.Point(50, 50);
            this.leftPictureBefore.Name = "leftPictureBefore";
            this.leftPictureBefore.Size = new System.Drawing.Size(250, 160);
            this.leftPictureBefore.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.leftPictureBefore.TabIndex = 0;
            this.leftPictureBefore.TabStop = false;
            // 
            // rightPictureAfter
            // 
            this.rightPictureAfter.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rightPictureAfter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightPictureAfter.Location = new System.Drawing.Point(500, 50);
            this.rightPictureAfter.Name = "rightPictureAfter";
            this.rightPictureAfter.Size = new System.Drawing.Size(250, 160);
            this.rightPictureAfter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rightPictureAfter.TabIndex = 1;
            this.rightPictureAfter.TabStop = false;
            // 
            // checkBoxUseAssembler
            // 
            this.checkBoxUseAssembler.AutoSize = true;
            this.checkBoxUseAssembler.Location = new System.Drawing.Point(120, 380);
            this.checkBoxUseAssembler.Name = "checkBoxUseAssembler";
            this.checkBoxUseAssembler.Size = new System.Drawing.Size(96, 17);
            this.checkBoxUseAssembler.TabIndex = 2;
            this.checkBoxUseAssembler.Text = "Use Assembler";
            this.checkBoxUseAssembler.UseVisualStyleBackColor = true;
            this.checkBoxUseAssembler.CheckedChanged += new System.EventHandler(this.CheckBoxUseAssembler_CheckedChanged);
            // 
            // buttonUploadPicture
            // 
            this.buttonUploadPicture.Location = new System.Drawing.Point(120, 216);
            this.buttonUploadPicture.Name = "buttonUploadPicture";
            this.buttonUploadPicture.Size = new System.Drawing.Size(96, 38);
            this.buttonUploadPicture.TabIndex = 3;
            this.buttonUploadPicture.Text = "Upload";
            this.buttonUploadPicture.UseVisualStyleBackColor = true;
            this.buttonUploadPicture.Click += new System.EventHandler(this.ButtonUploadPicture_Click);
            // 
            // buttonRunProgram
            // 
            this.buttonRunProgram.Enabled = false;
            this.buttonRunProgram.Location = new System.Drawing.Point(352, 304);
            this.buttonRunProgram.Name = "buttonRunProgram";
            this.buttonRunProgram.Size = new System.Drawing.Size(96, 23);
            this.buttonRunProgram.TabIndex = 4;
            this.buttonRunProgram.Text = "Run";
            this.buttonRunProgram.UseVisualStyleBackColor = true;
            this.buttonRunProgram.Click += new System.EventHandler(this.ButtonRunProgram_Click);
            // 
            // trackBarThreadsNumber
            // 
            this.trackBarThreadsNumber.AllowDrop = true;
            this.trackBarThreadsNumber.LargeChange = 1;
            this.trackBarThreadsNumber.Location = new System.Drawing.Point(296, 395);
            this.trackBarThreadsNumber.Maximum = 16;
            this.trackBarThreadsNumber.Minimum = 1;
            this.trackBarThreadsNumber.Name = "trackBarThreadsNumber";
            this.trackBarThreadsNumber.Size = new System.Drawing.Size(208, 45);
            this.trackBarThreadsNumber.TabIndex = 5;
            this.trackBarThreadsNumber.Value = Environment.ProcessorCount;
            this.trackBarThreadsNumber.Scroll += new System.EventHandler(this.TrackBarThreadsNumber_Scroll);
            // 
            // labelBefore
            // 
            this.labelBefore.AutoSize = true;
            this.labelBefore.Location = new System.Drawing.Point(148, 22);
            this.labelBefore.Name = "labelBefore";
            this.labelBefore.Size = new System.Drawing.Size(38, 13);
            this.labelBefore.TabIndex = 6;
            this.labelBefore.Text = "Before";
            // 
            // labelAfter
            // 
            this.labelAfter.AutoSize = true;
            this.labelAfter.Location = new System.Drawing.Point(613, 22);
            this.labelAfter.Name = "labelAfter";
            this.labelAfter.Size = new System.Drawing.Size(29, 13);
            this.labelAfter.TabIndex = 7;
            this.labelAfter.Text = "After";
            // 
            // buttonPickColor
            // 
            this.buttonPickColor.Enabled = false;
            this.buttonPickColor.Location = new System.Drawing.Point(120, 304);
            this.buttonPickColor.Name = "buttonPickColor";
            this.buttonPickColor.Size = new System.Drawing.Size(96, 23);
            this.buttonPickColor.TabIndex = 8;
            this.buttonPickColor.Text = "Pick a color";
            this.buttonPickColor.UseVisualStyleBackColor = true;
            this.buttonPickColor.Click += new System.EventHandler(this.ButtonPickColor_Click);
            // 
            // textBoxColorPicked
            // 
            this.textBoxColorPicked.Enabled = false;
            this.textBoxColorPicked.Location = new System.Drawing.Point(120, 333);
            this.textBoxColorPicked.Name = "textBoxColorPicked";
            this.textBoxColorPicked.Size = new System.Drawing.Size(96, 20);
            this.textBoxColorPicked.TabIndex = 9;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(375, 340);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(33, 13);
            this.labelTime.TabIndex = 10;
            this.labelTime.Text = "Time:";
            // 
            // labelThreads
            // 
            this.labelThreads.AutoSize = true;
            this.labelThreads.Location = new System.Drawing.Point(365, 379);
            this.labelThreads.Name = "labelThreads";
            this.labelThreads.Size = new System.Drawing.Size(49, 13);
            this.labelThreads.TabIndex = 11;
            this.labelThreads.Text = "Threads:";
            // 
            // labelThreadsNumberPicked
            // 
            this.labelThreadsNumberPicked.AutoSize = true;
            this.labelThreadsNumberPicked.Location = new System.Drawing.Point(420, 379);
            this.labelThreadsNumberPicked.Name = "labelThreadsNumberPicked";
            this.labelThreadsNumberPicked.Size = new System.Drawing.Size(13, 13);
            this.labelThreadsNumberPicked.TabIndex = 12;
            this.labelThreadsNumberPicked.Text = Environment.ProcessorCount.ToString();
            // 
            // labelTimeElapsed
            // 
            this.labelTimeElapsed.AutoSize = true;
            this.labelTimeElapsed.Location = new System.Drawing.Point(414, 340);
            this.labelTimeElapsed.Name = "labelTimeElapsed";
            this.labelTimeElapsed.Size = new System.Drawing.Size(13, 13);
            this.labelTimeElapsed.TabIndex = 13;
            this.labelTimeElapsed.Text = "0";
            // 
            // ButtonSave
            // 
            this.ButtonSave.Enabled = false;
            this.ButtonSave.Location = new System.Drawing.Point(579, 216);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(96, 38);
            this.ButtonSave.TabIndex = 14;
            this.ButtonSave.Text = "Save";
            this.ButtonSave.UseVisualStyleBackColor = true;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // buttonGenerateRaport
            // 
            this.buttonGenerateRaport.Enabled = false;
            this.buttonGenerateRaport.Location = new System.Drawing.Point(692, 400);
            this.buttonGenerateRaport.Name = "buttonGenerateRaport";
            this.buttonGenerateRaport.Size = new System.Drawing.Size(96, 38);
            this.buttonGenerateRaport.TabIndex = 15;
            this.buttonGenerateRaport.Text = "Generate a raport";
            this.buttonGenerateRaport.UseVisualStyleBackColor = true;
            this.buttonGenerateRaport.Click += new System.EventHandler(this.buttonGenerateRaport_Click);
            // 
            // LabelChooseFile
            // 
            this.LabelChooseFile.AutoSize = true;
            this.LabelChooseFile.Location = new System.Drawing.Point(47, 256);
            this.LabelChooseFile.Name = "LabelChooseFile";
            this.LabelChooseFile.Size = new System.Drawing.Size(62, 13);
            this.LabelChooseFile.TabIndex = 16;
            this.LabelChooseFile.Text = "Chosen file:";
            // 
            // LabelFileLocation
            // 
            this.LabelFileLocation.AutoSize = true;
            this.LabelFileLocation.Location = new System.Drawing.Point(47, 279);
            this.LabelFileLocation.Name = "LabelFileLocation";
            this.LabelFileLocation.Size = new System.Drawing.Size(0, 13);
            this.LabelFileLocation.TabIndex = 17;
            // 
            // GreenScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LabelFileLocation);
            this.Controls.Add(this.LabelChooseFile);
            this.Controls.Add(this.buttonGenerateRaport);
            this.Controls.Add(this.ButtonSave);
            this.Controls.Add(this.labelTimeElapsed);
            this.Controls.Add(this.labelThreadsNumberPicked);
            this.Controls.Add(this.labelThreads);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.textBoxColorPicked);
            this.Controls.Add(this.buttonPickColor);
            this.Controls.Add(this.labelAfter);
            this.Controls.Add(this.labelBefore);
            this.Controls.Add(this.trackBarThreadsNumber);
            this.Controls.Add(this.buttonRunProgram);
            this.Controls.Add(this.buttonUploadPicture);
            this.Controls.Add(this.checkBoxUseAssembler);
            this.Controls.Add(this.rightPictureAfter);
            this.Controls.Add(this.leftPictureBefore);
            this.Name = "GreenScreenForm";
            this.Text = "GreenScreen";
            ((System.ComponentModel.ISupportInitialize)(this.leftPictureBefore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightPictureAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreadsNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox leftPictureBefore;
        private System.Windows.Forms.PictureBox rightPictureAfter;
        private System.Windows.Forms.CheckBox checkBoxUseAssembler;
        private System.Windows.Forms.Button buttonUploadPicture;
        private System.Windows.Forms.Button buttonRunProgram;
        private System.Windows.Forms.TrackBar trackBarThreadsNumber;
        private System.Windows.Forms.Label labelBefore;
        private System.Windows.Forms.Label labelAfter;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button buttonPickColor;
        private System.Windows.Forms.TextBox textBoxColorPicked;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label labelThreads;
        private System.Windows.Forms.Label labelThreadsNumberPicked;
        private System.Windows.Forms.Label labelTimeElapsed;
        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.Button buttonGenerateRaport;
        private System.Windows.Forms.Label LabelChooseFile;
        private System.Windows.Forms.Label LabelFileLocation;
    }
}
