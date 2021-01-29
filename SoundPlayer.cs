using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MarkAble2
{
    //this code taken from an article on CodeProject "Playing MP3s using MCI By krazymir"
    //http://www.codeproject.com/KB/audio-video/MP3Example.aspx

    public class OpenFileEventArgs : EventArgs
    {
        public OpenFileEventArgs(string filename)
        {
            this.FileName = filename;
        }
        public readonly string FileName;
    }

    public class PlayFileEventArgs : EventArgs
    {
    }

    public class PauseFileEventArgs : EventArgs
    {
    }

    public class StopFileEventArgs : EventArgs
    {
    }

    public class CloseFileEventArgs : EventArgs
    {
    }

    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(long Err)
        {
            this.ErrNum = Err;
        }

        public readonly long ErrNum;
    }


    public class SoundPlayer
    {
        private string Pcommand,FName;
        private bool Opened, Playing, Paused, Loop, 
                     MutedAll, MutedLeft, MutedRight;
        private int rVolume, lVolume, aVolume, 
                    tVolume, bVolume, VolBalance;
        private ulong Lng;
        private long Err;

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, 
                StringBuilder strReturn, int iReturnLength, 
                IntPtr hwndCallback);


        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        public SoundPlayer()
        {
            Opened = false;
            Pcommand = "";
            FName = "";
            Playing = false;
            Paused = false;
            Loop = false;
            MutedAll = MutedLeft = MutedRight = false;
            rVolume = lVolume = aVolume = 
                      tVolume = bVolume = 1000;
            Lng = 0;
            VolBalance = 0;
            Err = 0;
        }

        #region Volume
        public bool MuteAll
        {
            get
            {
                return MutedAll;
            }
            set
            {
                MutedAll = value;
                if (MutedAll)
                {
                    Pcommand = "setaudio MediaFile off";
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
                else
                {
                    Pcommand = "setaudio MediaFile on";
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
            }

        }

        public bool MuteLeft
        {
            get
            {
                return MutedLeft;
            }
            set
            {
                MutedLeft = value;
                if (MutedLeft)
                {
                    Pcommand = "setaudio MediaFile left off";
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
                else
                {
                    Pcommand = "setaudio MediaFile left on";
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
            }

        }

        public bool MuteRight
        {
            get
            {
                return MutedRight;
            }
            set
            {
                MutedRight = value;
                if (MutedRight)
                {
                    Pcommand = "setaudio MediaFile right off";
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
                else
                {
                    Pcommand = "setaudio MediaFile right on";
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
            }

        }

        public int VolumeAll
        {
            get
            {
                return aVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    aVolume = value;
                    Pcommand = String.Format("setaudio MediaFile" + 
                               " volume to {0}", aVolume);
                    if((Err=mciSendString(Pcommand, null, 0, 
                                          IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
            }
        }

        public int VolumeLeft
        {
            get
            {
                return lVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    lVolume = value;
                    Pcommand = String.Format("setaudio MediaFile" + 
                               " left volume to {0}", lVolume);
                    if((Err=mciSendString(Pcommand, null, 0, 
                                           IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
            }
        }

        public int VolumeRight
        {
            get
            {
                return rVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    rVolume = value;
                    Pcommand = String.Format("setaudio" + 
                               " MediaFile right volume to {0}", rVolume);
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
            }
        }

        public int VolumeTreble
        {
            get
            {
                return tVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    tVolume = value;
                    Pcommand = String.Format("setaudio MediaFile" + 
                                             " treble to {0}", tVolume);
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
            }
        }

        public int VolumeBass
        {
            get
            {
                return bVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    bVolume = value;
                    Pcommand = String.Format("setaudio MediaFile bass to {0}", 
                                             bVolume);
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                }
            }
        }

        public int Balance
        {
            get
            {
                return VolBalance;
            }
            set
            {
                if (Opened && (value >= -1000 && value <= 1000))
                {
                    VolBalance = value;
                    if (value < 0)
                    {
                        Pcommand = "setaudio MediaFile left volume to 1000";
                        if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                            OnError(new ErrorEventArgs(Err));
                        Pcommand = String.Format("setaudio MediaFile right" + 
                                                 " volume to {0}", 1000 + value);
                        if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                            OnError(new ErrorEventArgs(Err));
                    }
                    else
                    {
                        Pcommand = "setaudio MediaFile right volume to 1000";
                        if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                            OnError(new ErrorEventArgs(Err));
                        Pcommand = String.Format("setaudio MediaFile" + 
                                   " left volume to {0}", 1000 - value);
                        if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                            OnError(new ErrorEventArgs(Err));
                    }
                }
            }
        }
        #endregion


        #region MasterVolume
        public static int GetVolume()
        {
            uint currVol = 0;
            waveOutGetVolume(IntPtr.Zero, out currVol);
            // Calculate the volume
            return (int)(currVol & 0x0000ffff);
        }

        public static void SetVolume(int volume)
        {
            uint newVolumeAllChannels = (((uint)volume & 0x0000ffff) | ((uint)volume << 16));
            // Set the volume
            waveOutSetVolume(IntPtr.Zero, newVolumeAllChannels);
        }

        private static int mutedVolume = 0;

        public static void MuteVolume()
        {
            int currentVolume = GetVolume();
            if (currentVolume > 0)
            {
                mutedVolume = currentVolume;
            }
            SetVolume(0);
        }

        public static void RestoreVolume()
        {
            if (mutedVolume > 0)
            {
                SetVolume(mutedVolume);
            }
        }
        #endregion

        #region Main Functions

        public string FileName
        {
            get
            {
                return FName;
            }
        }

        public bool Looping
        {
            get
            {
                return Loop;
            }
            set
            {
                Loop = value;
            }
        }

        public void Seek(ulong Millisecs)
        {
            if (Lng == 0)
                CalculateLength();

            if (Opened && Millisecs <= Lng)
            {
                if (Playing)
                {
                    if (Paused)
                    {
                        Pcommand = String.Format("seek MediaFile to {0}", Millisecs);
                        if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                            OnError(new ErrorEventArgs(Err));
                    }
                    else
                    {
                        Pcommand = String.Format("seek MediaFile to {0}", Millisecs);
                        if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                            OnError(new ErrorEventArgs(Err));
                        Pcommand = "play MediaFile";
                        if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                            OnError(new ErrorEventArgs(Err));
                    }
                }
            }
        }

        private void CalculateLength()
        {
            if (Opened)
            {
                StringBuilder str = new StringBuilder(128);
                mciSendString("status MediaFile length", str, 128, IntPtr.Zero);
                Lng = Convert.ToUInt64(str.ToString());
            }
            else
            {
                Lng = 0;
            }
        }

        public ulong AudioLength
        {
            get
            {
              CalculateLength();
              return Lng;
            }
        }

        public void Close()
        {
            if (Opened)
            {
                Pcommand = "close MediaFile";
                if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                    OnError(new ErrorEventArgs(Err));
                Opened = false;
                Playing = false;
                Paused = false;
                OnCloseFile(new CloseFileEventArgs());
            }
        }

        public void Open(string sFileName)
        {
            if (!File.Exists(sFileName))
            {
                Opened = false;
                return;
            }

            if (!Opened)
            {
                Pcommand = "open \"" + sFileName + 
                           "\" type mpegvideo alias MediaFile";
                if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                    OnError(new ErrorEventArgs(Err));
                FName = sFileName;
                Opened = true;
                Playing = false;
                Paused = false;
                Pcommand = "set MediaFile time format milliseconds";
                if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                    OnError(new ErrorEventArgs(Err));
                Pcommand = "set MediaFile seek exactly on";
                if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                    OnError(new ErrorEventArgs(Err));
                //CalculateLength();
                OnOpenFile(new OpenFileEventArgs(sFileName));
            }
            else
            {
                this.Close();
                this.Open(sFileName);
            }
        }

        public void Play()
        {
            if (Opened)
            {
                if (!Playing)
                {
                    Playing = true;
                    Pcommand = "play MediaFile";
                    if (Loop) Pcommand += " REPEAT";
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                    OnPlayFile(new PlayFileEventArgs());
                }
                else
                {
                    if (!Paused)
                    {
                        Pcommand = "seek MediaFile to start";
                        if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                            OnError(new ErrorEventArgs(Err));
                        Pcommand = "play MediaFile";
                        if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                            OnError(new ErrorEventArgs(Err));
                        OnPlayFile(new PlayFileEventArgs());
                    }
                    else
                    {
                        Paused = false;
                        Pcommand = "play MediaFile";
                        if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                            OnError(new ErrorEventArgs(Err));
                        OnPlayFile(new PlayFileEventArgs());
                    }
                }
            }
        }

        public void Pause()
        {
            if (Opened)
            {
                if (!Paused)
                {
                    Paused = true;
                    Pcommand = "pause MediaFile";
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                    OnPauseFile(new PauseFileEventArgs());
                }
                else
                {
                    Paused = false;
                    Pcommand = "play MediaFile";
                    if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                    OnPlayFile(new PlayFileEventArgs());
                }
            }
        }

        public void Stop()
        {
            if (Opened && Playing)
            {
                Playing = false;
                Paused = false;
                Pcommand = "seek MediaFile to start";
                if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                    OnError(new ErrorEventArgs(Err));
                Pcommand = "stop MediaFile";
                if((Err=mciSendString(Pcommand, null, 0, IntPtr.Zero))!=0)
                    OnError(new ErrorEventArgs(Err));
                OnStopFile(new StopFileEventArgs());
            }
        }

        public ulong CurrentPosition
        {
            get
            {
                if (Opened && Playing)
                {
                    StringBuilder s = new StringBuilder(128);
                    Pcommand = "status MediaFile position";
                    if((Err=mciSendString(Pcommand, s, 128, IntPtr.Zero))!=0)
                        OnError(new ErrorEventArgs(Err));
                    return Convert.ToUInt64(s.ToString());
                }
                else return 0;
            }
        }

#endregion

        #region Event Handling

        public delegate void OpenFileEventHandler(Object sender, 
                             OpenFileEventArgs oea);

        public delegate void PlayFileEventHandler(Object sender, 
                             PlayFileEventArgs pea);

        public delegate void PauseFileEventHandler(Object sender, 
                             PauseFileEventArgs paea);

        public delegate void StopFileEventHandler(Object sender, 
                                         StopFileEventArgs sea);

        public delegate void CloseFileEventHandler(Object sender, 
                                         CloseFileEventArgs cea);

        public delegate void ErrorEventHandler(Object sender, 
                                         ErrorEventArgs eea);

        public event OpenFileEventHandler OpenFile;

        public event PlayFileEventHandler PlayFile;

        public event PauseFileEventHandler PauseFile;

        public event StopFileEventHandler StopFile;

        public event CloseFileEventHandler CloseFile;

        public event ErrorEventHandler Error;

        protected virtual void OnOpenFile(OpenFileEventArgs oea)
        {
            if (OpenFile != null) OpenFile(this, oea);
        }

        protected virtual void OnPlayFile(PlayFileEventArgs pea)
        {
            if (PlayFile != null) PlayFile(this, pea);
        }

        protected virtual void OnPauseFile(PauseFileEventArgs paea)
        {
            if (PauseFile != null) PauseFile(this, paea);
        }

        protected virtual void OnStopFile(StopFileEventArgs sea)
        {
            if (StopFile != null) StopFile(this, sea);
        }

        protected virtual void OnCloseFile(CloseFileEventArgs cea)
        {
            if (CloseFile != null) CloseFile(this, cea);
        }

        protected virtual void OnError(ErrorEventArgs eea)
        {
            if (Error != null) Error(this, eea);
        }

        #endregion
    }
}
