using System.Drawing;

namespace GreenScreenUI
{
    class ImageHolder
    {
        public Bitmap InputImage { get; set; }

        public Bitmap OutputImage { get; set; }

        public byte[] Pixels { get; set; }

        public int GetPixelsSize()
        {
            return Pixels.Length;
        }
        public int GetInputHeight()
        {
            return InputImage.Height;
        }

        public int GetInputWidth()
        {
            return InputImage.Width;
        }
    }
}
