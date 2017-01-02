namespace Zagorapps.Audio.Library.Managers
{
    using System;
    using Events;

    public interface IAudioManager : IDisposable
    {
        event EventHandler<VolumeChangeEvent> OnVolumeChanged;

        int Volume { get; set; }

        bool IsMuted { get; set; }
    }
}