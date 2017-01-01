namespace Zagorapps.Audio.Library.Managers
{
    using System;

    public interface IAudioManager : IDisposable
    {
        int Volume { get; set; }

        bool IsMuted { get; set; }
    }
}