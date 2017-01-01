namespace Zagorapps.Audio.Library.Managers
{
    using System;
    using Core.Library.Events;
    using Events;
    using NAudio.CoreAudioApi;

    public class AudioManager : IAudioManager
    {
        private const float MinimumVolume = 0.00f,
            MaximumVolume = 100.00f;

        private readonly MMDeviceEnumerator deviceEnumerator;
        private readonly MMDevice localAudioDevice;

        public AudioManager()
        {
            this.deviceEnumerator = new MMDeviceEnumerator();
            this.localAudioDevice = this.deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            this.localAudioDevice.AudioEndpointVolume.OnVolumeNotification += this.AudioEndpointVolume_OnVolumeNotification;
        }

        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            Invoker.Raise(ref this.OnVolumeChanged, this, new VolumeChangeEvent(data.Muted, (int)(data.MasterVolume * AudioManager.MaximumVolume)));
        }

        public event EventHandler<VolumeChangeEvent> OnVolumeChanged;

        public int Volume
        {
            get { return (int)(this.MasterVolumeScalar * AudioManager.MaximumVolume); }
            set { this.MasterVolumeScalar = this.EnsureVolumeIsInRange(value) / AudioManager.MaximumVolume; }
        }
        
        public bool IsMuted
        {
            get { return this.localAudioDevice.AudioEndpointVolume.Mute; }
            set { this.localAudioDevice.AudioEndpointVolume.Mute = value; }
        }

        private float MasterVolumeScalar
        {
            get { return this.localAudioDevice.AudioEndpointVolume.MasterVolumeLevelScalar; }
            set { this.localAudioDevice.AudioEndpointVolume.MasterVolumeLevelScalar = value; }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.deviceEnumerator.Dispose();
            }
        }

        protected int EnsureVolumeIsInRange(float proposed)
        {
            if (proposed < AudioManager.MinimumVolume)
            {
                return (int)AudioManager.MinimumVolume;
            }

            if (proposed > AudioManager.MaximumVolume)
            {
                return (int)AudioManager.MaximumVolume;
            }

            return (int)proposed;
        }
    }
}