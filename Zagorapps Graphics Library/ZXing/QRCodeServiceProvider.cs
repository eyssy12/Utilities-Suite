namespace Zagorapps.Graphics.Library.ZXing
{
    using System.Drawing;
    using com.google.zxing;
    using com.google.zxing.qrcode;

    public class QRCodeServiceProvider : IQRCodeServiceProvider
    {
        protected readonly QRCodeWriter Writer;
        protected readonly QRCodeReader Reader;

        public QRCodeServiceProvider()
        {
            this.Writer = new QRCodeWriter();
            this.Reader = new QRCodeReader();
        }

        public Bitmap GenerateImage(string contents, int width, int height)
        {
            try
            {
                return this.Writer.encode(contents, BarcodeFormat.QR_CODE, width, height).ToBitmap();
            }
            catch
            {
                throw;
            }
        }
    }
}