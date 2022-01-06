namespace GreenScreenUI
{
    class GreenScreenRemover
    {
        public void processPicture(byte[] pixels, byte[] colorRgbBytes, int size)
        {
            for (int i = 0; i < size; i += 4)
            {
                if (pixels[i + 1] == colorRgbBytes[0] && pixels[i + 2] == colorRgbBytes[1] && pixels[i + 3] == colorRgbBytes[2])
                {
                    pixels[i] = 0; //A
                    pixels[i + 1] = 0; //R
                    pixels[i + 2] = 0; //G
                    pixels[i + 3] = 0; //B
                }
            }
        }
    }
}
