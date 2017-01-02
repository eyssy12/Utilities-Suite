namespace Zagorapps.Graphics.Configuration.Library
{
    using Graphics.Library.ZXing;
    using SimpleInjector;
    using Zagorapps.Configuration.Library;

    public class GraphicsBindings : BindingsBase
    {
        protected override void RegisterBindings()
        {
            this.Register<IQRCodeServiceProvider, QRCodeServiceProvider>(lifestyle: Lifestyle.Transient);
        }
    }
}