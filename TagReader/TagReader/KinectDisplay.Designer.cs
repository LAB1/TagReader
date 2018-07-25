using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace TagReader
{
    partial class KinectDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// 
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        bool StartSavingFrames = false;
        bool isShown = false;
        LinkedList<String> SaveColorTimestamps = new LinkedList<String>();
        LinkedList<BitmapSource> SaveColorFrames = new LinkedList<BitmapSource>();

        LinkedList<String> SaveDepthTimestamps = new LinkedList<String>();
        LinkedList<BitmapSource> SaveDepthFrames = new LinkedList<BitmapSource>();

        LinkedList<String> SaveCombinedTimestamps = new LinkedList<String>();
        LinkedList<BitmapSource> SaveCombinedFrames = new LinkedList<BitmapSource>();

        LinkedList<ushort[]> SaveDepthMatrix = new LinkedList<ushort[]>();
        LinkedList<String> SaveDepthMatrixTimestamp = new LinkedList<string>();

        LinkedList<String> SaveXYZTimestamps = new LinkedList<String>();
        LinkedList<CameraSpacePoint[]> SaveCameraSpacePoints= new LinkedList<CameraSpacePoint[]>();

        PixelFormat format = PixelFormats.Bgr32;

        const int width = 512;
        const int height = 424;
        

        public enum Mode
        {
            Color,
            Depth,
            Infrared,
            Record,
            Stop
        }

        Mode _mode = Mode.Color;
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
            this.RGBVideo = new System.Windows.Forms.PictureBox();
            this.DepthVideo = new System.Windows.Forms.PictureBox();
            this.RGBLabel = new System.Windows.Forms.Label();
            this.DepthLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RGBVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepthVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // RGBVideo
            // 
            this.RGBVideo.Location = new System.Drawing.Point(11, 81);
            this.RGBVideo.Name = "RGBVideo";
            this.RGBVideo.Size = new System.Drawing.Size(374, 252);
            this.RGBVideo.TabIndex = 0;
            this.RGBVideo.TabStop = false;
            this.RGBVideo.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // DepthVideo
            // 
            this.DepthVideo.Location = new System.Drawing.Point(425, 81);
            this.DepthVideo.Name = "DepthVideo";
            this.DepthVideo.Size = new System.Drawing.Size(360, 252);
            this.DepthVideo.TabIndex = 1;
            this.DepthVideo.TabStop = false;
            // 
            // RGBLabel
            // 
            this.RGBLabel.AutoSize = true;
            this.RGBLabel.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RGBLabel.Location = new System.Drawing.Point(159, 39);
            this.RGBLabel.Name = "RGBLabel";
            this.RGBLabel.Size = new System.Drawing.Size(71, 39);
            this.RGBLabel.TabIndex = 2;
            this.RGBLabel.Text = "RGB";
            // 
            // DepthLabel
            // 
            this.DepthLabel.AutoSize = true;
            this.DepthLabel.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DepthLabel.Location = new System.Drawing.Point(556, 39);
            this.DepthLabel.Name = "DepthLabel";
            this.DepthLabel.Size = new System.Drawing.Size(98, 39);
            this.DepthLabel.TabIndex = 3;
            this.DepthLabel.Text = "Depth";
            // 
            // KinectDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 382);
            this.Controls.Add(this.DepthLabel);
            this.Controls.Add(this.RGBLabel);
            this.Controls.Add(this.DepthVideo);
            this.Controls.Add(this.RGBVideo);
            this.Name = "KinectDisplay";
            this.Text = "KinectRGB";
            this.Load += new System.EventHandler(this.KinectRGB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RGBVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepthVideo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox RGBVideo;
        private System.Windows.Forms.PictureBox DepthVideo;
        private System.Windows.Forms.Label RGBLabel;
        private System.Windows.Forms.Label DepthLabel;
    }


}