namespace Zagorapps.Audio.Library.Events
{
    using System;

    public class VolumeChangeEvent : EventArgs
    {
        private bool isMuted;
        private int volume;

        public VolumeChangeEvent(bool isMuted, int volume)
        {
            this.isMuted = isMuted;
            this.volume = volume;
        }

        public bool IsMuted
        {
            get { return this.isMuted; }
        }

        public int Volume
        {
            get { return this.volume; }
        }
    }
}