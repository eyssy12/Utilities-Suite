namespace Zagorapps.Audio.Library.Managers
{
    using NAudio.CoreAudioApi;

    public class AudioManager : IAudioManager
    {
        private const float MinimumVolume = 0.00f,
            MaximumVolume = 100.00f;

        private readonly MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
        private readonly MMDevice localAudioDevice;

        private float lastMasterVolumeScalar;

        public AudioManager()
        {
            this.localAudioDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            this.lastMasterVolumeScalar = this.MasterVolumeScalar;
        }

        public int Volume
        {
            get { return (int)(this.MasterVolumeScalar * AudioManager.MaximumVolume); }
            set
            {
                if (value < AudioManager.MinimumVolume)
                {
                    value = (int)AudioManager.MinimumVolume;
                }
                else if (value > AudioManager.MaximumVolume)
                {
                    value = (int)AudioManager.MaximumVolume;
                }

                this.lastMasterVolumeScalar = this.MasterVolumeScalar;
                this.MasterVolumeScalar = value / AudioManager.MaximumVolume;
            }
        }

        private float MasterVolumeScalar
        {
            get { return this.localAudioDevice.AudioEndpointVolume.MasterVolumeLevelScalar; }
            set { this.localAudioDevice.AudioEndpointVolume.MasterVolumeLevelScalar = value; }
        }

        public bool IsMuted
        {
            get { return this.localAudioDevice.AudioEndpointVolume.Mute; }
            set { this.localAudioDevice.AudioEndpointVolume.Mute = value; }
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
    }
}