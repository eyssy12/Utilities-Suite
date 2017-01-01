namespace Zagorapps.Audio.Configuration.Library
{
    using Audio.Library.Managers;
    using SimpleInjector;
    using Zagorapps.Configuration.Library;

    public class AudioBindings : BindingsBase
    {
        protected override void RegisterBindings()
        {
            this.Register<IAudioManager, AudioManager>(lifestyle: Lifestyle.Singleton);
        }
    }
}