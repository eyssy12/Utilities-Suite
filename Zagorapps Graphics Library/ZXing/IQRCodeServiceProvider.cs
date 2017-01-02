namespace Zagorapps.Graphics.Library.ZXing
{
    using System.Drawing;
    using com.google.zxing;

    public interface IQRCodeServiceProvider
    {
        Bitmap GenerateImage(string contents, int width, int height);
    }
}