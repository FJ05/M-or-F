using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing.Imaging;

namespace Male_Or_Female
{
    public partial class Form1 : Form
    {
        VideoCapture capture;
        Mat frame;
        Bitmap image;
        
        public Form1()
        {

            InitializeComponent();

            frame = new Mat();
            capture = new VideoCapture(0);
            capture.Open(0);
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            // Takes a photo
            // stops the camera
            timer2.Stop();

            ImageCodecInfo myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // save Image bitmap as a jpg in the image folder
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            myEncoder = Encoder.Quality;

            myEncoderParameters = new EncoderParameters(1);

            myEncoderParameter = new EncoderParameter(myEncoder, 25L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            image.Save("image.jpg", myImageCodecInfo, myEncoderParameters);

            // fetch image

            // path is the path to the image which is located in bin/debug/net6.0-windows
            var path = "image.jpg";


            // Make the Ai generate a guess based on the image
            // Load sample data
            var imageBytes = File.ReadAllBytes(path);
            MLModel1.ModelInput sampleData = new MLModel1.ModelInput()
            {
                ImageSource = imageBytes,
            };
            //Load model and predict output
            var result = MLModel1.Predict(sampleData);
            ResultLable.Text = $"result {result.PredictedLabel} certanty: {result.Score.Max() * 100}%";
            // Starts the Camera
            timer2.Start();
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (capture.IsOpened())
            {
                capture.Read(frame);
                // convert frame to bitmap
                image = BitmapConverter.ToBitmap(frame);
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                pictureBox1.Image = image;
            }
        }
    }
}