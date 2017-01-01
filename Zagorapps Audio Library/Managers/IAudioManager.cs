namespace Zagorapps.Audio.Library.Managers
{
    using System;
    using Core.Library.Events;
    using Events;

    public interface IAudioManager : IDisposable
    {
        int Volume { get; set; }

        bool IsMuted { get; set; }

        event EventHandler<VolumeChangeEvent> OnVolumeChanged;
    }
}