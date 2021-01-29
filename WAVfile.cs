using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MarkAble2
{
    public class WAVfile
    {
        private string myFileName = "";
        private long myLength = 0;

        public WAVfile(string filename)
        {
            myFileName = filename;
            if (File.Exists(myFileName))
            {
                try
                {
                    var fi = new FileInfo(myFileName);
                    myLength = fi.Length;
                }
                catch (Exception)
                {
                    myLength = 0;
                }
            }

        }

        public string FileName
        {
            get { return myFileName; }
            set { myFileName = value; }
        }

        public long Length
        {
            get
            {
                return myLength;
            }
            set { myLength = value; }
        }

        [DllImport("winmm.dll")]
        private static extern uint mciSendString(
            string command,
            StringBuilder returnValue,
            int returnLength,
            IntPtr winHandle);

        public double DurationInSecs()
        {
            if (!File.Exists(myFileName))
            {
                return 0;
            }

            try
            {
                StringBuilder lengthBuf = new StringBuilder(32);

                mciSendString(string.Format("open \"{0}\" type waveaudio alias wave", myFileName), null, 0, IntPtr.Zero);
                mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
                mciSendString("close wave", null, 0, IntPtr.Zero);

                int length = 0;
                int.TryParse(lengthBuf.ToString(), out length);  //this returns milliseconds

                return (double)length / 1000; //makes it seconds
            }
            catch (Exception)
            {
                return 0;
            }

        }
    }
}
