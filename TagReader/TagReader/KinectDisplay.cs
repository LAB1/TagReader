using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using System.IO;

namespace TagReader
{
    public partial class KinectDisplay : Form
    {
        public KinectDisplay(bool _isKinectDisplay)
        {
            if (_isKinectDisplay == false)
                isShown = false;
            else
            {
                isShown = true;
                
            }
            InitializeComponent();

            // Obtain the sensor and start it up
            _sensor = KinectSensor.GetDefault(); // Different than article

            if (_sensor != null)
            {
                _sensor.Open();
            }


            // Specify the requires streams
            _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color |
                                             FrameSourceTypes.Depth |
                                             FrameSourceTypes.Infrared);
            StartSavingFrames = true;
            _mode = Mode.Record;
            // Add an event handler
            _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;


        }
        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {

            // Get a reference to the multi-frame
            var reference = e.FrameReference.AcquireFrame();

            //stores color and depth frame by frame to outside source
            using (var colorframe = reference.ColorFrameReference.AcquireFrame())
            {
                using (var depthframe = reference.DepthFrameReference.AcquireFrame())
                {
                    if (colorframe != null && depthframe != null)
                    {
                        // Do something with the frame...
                        if (_mode == Mode.Record && StartSavingFrames)
                        {
                            
                            //BitmapSource colorbitmap = ToBitmap1(colorframe);
                            BitmapSource depthbitmap = ToBitmap1(depthframe);


                            //SaveColorTimestamps.AddLast(DateTime.Now.ToString("hhmmssfff"));
                            //SaveColorFrames.AddLast(colorbitmap);
                            SaveDepthTimestamps.AddLast((DateTime.Now - DateTime.Parse("1/1/1970 0:0:0")).TotalMilliseconds.ToString());
                            SaveDepthFrames.AddLast(depthbitmap);
                            
                            BitmapSource combinedbitmap = ToCombinedData(colorframe, depthframe);
                            SaveCombinedTimestamps.AddLast((DateTime.Now - DateTime.Parse("1/1/1970 0:0:0")).TotalMilliseconds.ToString());
                            SaveCombinedFrames.AddLast(combinedbitmap);                                                                                  
                            
                        }
                    }
                }
            }
            if (isShown == true)
            {
                // Open color frame
                using (var frame = reference.ColorFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        // Do something with the frame...
                        if (_mode == Mode.Color)
                        {
                            BitmapSource bitmap = ToBitmap1(frame);
                            RGBVideo.Image = BitmapFromSource(bitmap);


                        }
                    }
                }

                // Open depth frame
                using (var frame = reference.DepthFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        // Do something with the frame...
                        if (_mode == Mode.Depth)
                        {
                            BitmapSource bitmap = ToBitmap1(frame);
                            DepthVideo.Image = BitmapFromSource(bitmap);

                        }
                    }
                }

                // Open infrared frame
                using (var frame = reference.InfraredFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        // Do something with the frame...
                        if (_mode == Mode.Infrared)
                        {

                        }
                    }
                }
            }
            
            
        }

        public void kill()
        {
            StartSavingFrames = false;
            _mode = Mode.Stop;
            /*
            IEnumerator<String> a = SaveColorTimestamps.GetEnumerator();
            foreach (BitmapSource node in SaveColorFrames)
            {
                a.MoveNext();
                PngBitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(node));
                string temppath = System.IO.Path.Combine(@"../output/kinect/color/", a.Current + ".png");
                using (System.IO.FileStream fs = new System.IO.FileStream(temppath, System.IO.FileMode.Create))
                {
                    enc.Save(fs);
                    fs.Close();
                }
            }
            SaveColorTimestamps.Clear();
            SaveColorFrames.Clear();

            a.Dispose();
            a = SaveDepthTimestamps.GetEnumerator();
            foreach (BitmapSource node in SaveDepthFrames)
            {
                a.MoveNext();
                PngBitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(node));
                string temppath = System.IO.Path.Combine(@"../output/kinect/depth/", a.Current + ".png");
                using (System.IO.FileStream fs = new System.IO.FileStream(temppath, System.IO.FileMode.Create))
                {
                    enc.Save(fs);
                    fs.Close();

                }

            }
            SaveDepthTimestamps.Clear();
            SaveDepthFrames.Clear();
            this.Close();
            */
            IEnumerator<String> a = SaveDepthTimestamps.GetEnumerator();
            foreach (BitmapSource node in SaveDepthFrames)
            {
                a.MoveNext();
                PngBitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(node));
                string temppath = System.IO.Path.Combine(@"../output/kinect/depth/", a.Current + ".png");
                using (System.IO.FileStream fs = new System.IO.FileStream(temppath, System.IO.FileMode.Create))
                {
                    enc.Save(fs);
                    fs.Close();

                }

            }

            SaveDepthTimestamps.Clear();
            SaveDepthFrames.Clear();

            a = SaveCombinedTimestamps.GetEnumerator();
            foreach (BitmapSource node in SaveCombinedFrames)
            {
                a.MoveNext();
                PngBitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(node));
                string temppath = System.IO.Path.Combine(@"../output/kinect/color/", a.Current + ".png");
                using (System.IO.FileStream fs = new System.IO.FileStream(temppath, System.IO.FileMode.Create))
                {
                    enc.Save(fs);
                    fs.Close();
                }
            }

            SaveCombinedTimestamps.Clear();
            SaveCombinedFrames.Clear();

            a.Dispose();
            this.Close();
        }
        
        private Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        // Convert a ColorFrame to an ImageSource
        private ImageSource ToBitmap(ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }
        private BitmapSource ToBitmap1(ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }
        private BitmapSource ToBitmap1(DepthFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            ushort minDepth = frame.DepthMinReliableDistance;
            ushort maxDepth = frame.DepthMaxReliableDistance;

            ushort[] depthData = new ushort[width * height];
            byte[] pixelData = new byte[width * height * (format.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(depthData);

            int colorIndex = 0;
            for (int depthIndex = 0; depthIndex < depthData.Length; ++depthIndex)
            {
                ushort depth = depthData[depthIndex];
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                pixelData[colorIndex++] = intensity; // Blue
                pixelData[colorIndex++] = intensity; // Green
                pixelData[colorIndex++] = intensity; // Red

                ++colorIndex;
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixelData, stride);
        }

        // Convert a DepthFrame to an ImageSource
        private ImageSource ToBitmap(DepthFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            ushort minDepth = frame.DepthMinReliableDistance;
            ushort maxDepth = frame.DepthMaxReliableDistance;

            ushort[] depthData = new ushort[width * height];
            byte[] pixelData = new byte[width * height * (format.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(depthData);

            int colorIndex = 0;
            for (int depthIndex = 0; depthIndex < depthData.Length; ++depthIndex)
            {
                ushort depth = depthData[depthIndex];
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                pixelData[colorIndex++] = intensity; // Blue
                pixelData[colorIndex++] = intensity; // Green
                pixelData[colorIndex++] = intensity; // Red

                ++colorIndex;
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixelData, stride);
        }

        // Convert an InfraredFrame to an ImageSource
        private ImageSource ToBitmap(InfraredFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            ushort[] infraredData = new ushort[width * height];
            byte[] pixelData = new byte[width * height * (format.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(infraredData);

            int colorIndex = 0;
            for (int infraredIndex = 0; infraredIndex < infraredData.Length; ++infraredIndex)
            {
                ushort ir = infraredData[infraredIndex];
                byte intensity = (byte)(ir >> 8);

                pixelData[colorIndex++] = intensity; // Blue
                pixelData[colorIndex++] = intensity; // Green   
                pixelData[colorIndex++] = intensity; // Red

                ++colorIndex;
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixelData, stride);
        }

        private BitmapSource ToCombinedData(ColorFrame colorFrame, DepthFrame depthFrame)
        {
            int colorwidth = colorFrame.FrameDescription.Width;
            int colorheight = colorFrame.FrameDescription.Height;

            var depthWidth = depthFrame.FrameDescription.Width;
            var depthHeight = depthFrame.FrameDescription.Height;

            PixelFormat format = PixelFormats.Bgr32;
            int bytesPerPixel = ((format.BitsPerPixel + 7) / 8);

            byte[] _colorFrameData = new byte[colorwidth * colorheight * bytesPerPixel];
            byte[] pixels = new byte[depthWidth * depthHeight * bytesPerPixel];


            ushort[] _depthData = new ushort[depthWidth * depthHeight];
            ColorSpacePoint[] _colorSpacePoints = new ColorSpacePoint[depthWidth * depthHeight];

            colorFrame.CopyConvertedFrameDataToArray(_colorFrameData, ColorImageFormat.Bgra);

            depthFrame.CopyFrameDataToArray(_depthData);

            _sensor.CoordinateMapper.MapDepthFrameToColorSpace(_depthData, _colorSpacePoints);

            for (int depthY = 0; depthY < depthHeight; depthY++)
            {
                for (int depthX = 0; depthX < depthWidth; depthX++)
                {
                    int depthIndex = depthY * depthWidth + depthX;
                    int colorX = (int)(_colorSpacePoints[depthIndex].X + 0.5);
                    int colorY = (int)(_colorSpacePoints[depthIndex].Y + 0.5);
                    if ((0 <= colorX) && (colorX < colorwidth) && (0 <= colorY) && (colorY < colorheight))
                    {
                        int colorIndex = colorY * colorwidth + colorX;
                        pixels[depthIndex * bytesPerPixel + 0] = _colorFrameData[colorIndex * bytesPerPixel + 0];
                        pixels[depthIndex * bytesPerPixel + 1] = _colorFrameData[colorIndex * bytesPerPixel + 1];
                        pixels[depthIndex * bytesPerPixel + 2] = _colorFrameData[colorIndex * bytesPerPixel + 2];
                        pixels[depthIndex * bytesPerPixel + 3] = _colorFrameData[colorIndex * bytesPerPixel + 3];
                    }
                }
            }



            int stride = depthWidth * bytesPerPixel;
            return BitmapSource.Create(depthWidth, depthHeight, 96, 96, format, null, pixels, stride);
        }
        private void KinectRGB_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
