using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MarkAble2
{
    public class Prefs
    {


        private const string LibraryLocation = "<key>iTunes Library XML Location";
        private const string AACEncoderKey = "<key>iTunes.exe:AAC Encoder</key>";
        private const string MP3EncoderKey = "<key>iTunes.exe:MP3 Encoder</key>";
        private const string AACCustomKey = "<key>iTunes.exe:AAC Encoder:Custom</key>";
        private const string MP3CustomKey = "<key>iTunes.exe:MP3 Encoder:Custom</key>";
        private const string DataStart = "<data>";
        private const string DataEnd = "</data>";

        //private static bool LibraryPresent = false;
        private static bool EncodeAACPresent = false;
        private static bool EncodeMP3Present = false;
        private static bool CustomAACPresent = false;
        private static bool CustomMP3Present = false;
        private static readonly string PrefsFile = Global.AppData() + @"\Apple Computer\iTunes\iTunesPrefs.xml";
        private static readonly string PrefsBackup = Global.AppData() + @"\Apple Computer\iTunes\Backup_iTunesPrefs.xml";
        private static readonly string PrefsModified = Global.AppData() + @"\Apple Computer\iTunes\MarkAble_iTunesPrefs.xml";


        private const string Custom = @"		AAAAAAZDdXN0b20AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==
";

        private const string AAC64kbps = @"		AAAAAAB9AAAA+gAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAA==
";

        private const string AAC96kbps = @"		AAAAAIC7AAAAdwEAAgAAAESsAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAA==
";
        private const string AAC128kbps = @"		AAAAAAD6AAAA9AEAAgAAAESsAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAA==
";

        private const string MP364kbps = @"		QQAAACAAAABAAAAAAgAAAAEAAAAiVgAACgAyADIALgAwADUAMAAgAGsASAB6
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=
";

        private const string MP396kbps = @"		QQAAADAAAABgAAAAAgAAAAEAAAAiVgAACgAyADIALgAwADUAMAAgAGsASAB6
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=
";

        private const string MP3128kbps = @"		QQAAAEAAAACAAAAAAgAAAAEAAAAiVgAACgAyADIALgAwADUAMAAgAGsASAB6
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=
";

        private const string AAC256kbps = @"		AAAAAAD0AQAA6AMAAgAAAESsAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAA==
";

        private const string MP3256kbps = @"		QQAAAIAAAAAAAQAAAgAAAAEAAAAAAAAABABBAHUAdABvADUAMAAgAGsASAB6
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=
";


        private static bool OpenPrefs()
        {
            if (!File.Exists(PrefsFile))
                return false;

            CheckForKeys(PrefsFile);
            return true;
        }

        public static bool HaveChanged()
        {
            return File.Exists(PrefsBackup);
        }

        private static void CheckForKeys(string preffile)
        {
            using(TextReader tr = new StreamReader(preffile,Encoding.UTF8))
            {
                string line;
                while((line = tr.ReadLine()) != null)
                {
                    //if (line.IndexOf(LibraryLocation) >= 0)
                    //{
                    //    LibraryPresent = true;
                    //}
                    if (line.IndexOf(AACEncoderKey) >= 0)
                    {
                        EncodeAACPresent = true;
                    }
                    if (line.IndexOf(MP3EncoderKey) >= 0)
                    {
                        EncodeMP3Present = true;
                    }
                    if (line.IndexOf(AACCustomKey) >= 0)
                    {
                        CustomAACPresent = true;
                    }
                    if (line.IndexOf(MP3CustomKey) >= 0)
                    {
                        CustomMP3Present = true;
                    }
                }

                tr.Close();
            }
        }

        private enum States
        {
            None,
            FoundLibraryLocation,
            FoundAACEncoder,
            FoundAACCustom,
            FoundMP3Encoder,
            FoundMP3Custom,
            InsideData

        }

        private static States CurrentState = States.None;

        public static bool SetCustomRate()
        {
            string AACEncode = AAC96kbps;
            string MP3Encode = MP396kbps;

            switch(Global.Options.BitRate)
            {
                case 64:
                    AACEncode = AAC64kbps;
                    MP3Encode = MP364kbps;
                    break;
                case 96:
                    AACEncode = AAC96kbps;
                    MP3Encode = MP396kbps;
                    break;
                case 128:
                    AACEncode = AAC128kbps;
                    MP3Encode = MP3128kbps;
                    break;
                case 256:
                    AACEncode = AAC256kbps;
                    MP3Encode = MP3256kbps;
                    break;
            }

            if (OpenPrefs())
            {
                using (TextReader tr = new StreamReader(PrefsFile, Encoding.UTF8))
                {
                    using (TextWriter tw = new StreamWriter(PrefsModified, false, Encoding.UTF8))
                    {
                        tw.NewLine = "\n"; //Apple file!

                        string line;

                        while ((line = tr.ReadLine()) != null)
                        {
                            if (line.IndexOf(LibraryLocation) >= 0)
                            {
                                CurrentState = States.FoundLibraryLocation;
                            }
                            if (line.IndexOf(AACEncoderKey) >= 0)
                            {
                                CurrentState = States.FoundAACEncoder;
                            }
                            if (line.IndexOf(MP3EncoderKey) >= 0)
                            {
                                CurrentState = States.FoundMP3Encoder;
                            }
                            if (line.IndexOf(AACCustomKey) >= 0)
                            {
                                CurrentState = States.FoundAACCustom;
                            }
                            if (line.IndexOf(MP3CustomKey) >= 0)
                            {
                                CurrentState = States.FoundMP3Custom;
                            }

                            if (line.IndexOf(DataEnd) >= 0)
                            {
                                CurrentState = States.None;
                            }

                            switch (CurrentState)
                            {
                                case States.None:
                                    tw.WriteLine(line);
                                    break;

                                case States.FoundLibraryLocation:                                   
                                    //read and write the whole library block
                                    tw.WriteLine(line);
                                    while ((line != null) && (line.IndexOf(DataEnd) == -1))
                                    {
                                        line = tr.ReadLine();
                                        tw.WriteLine(line);
                                    }

                                    //if we didn't find encoder blocks, write them AFTER the library block
                                    if (!EncodeAACPresent)
                                    {
                                        tw.WriteLine("\t\t" + AACEncoderKey);
                                        tw.WriteLine("\t\t" + DataStart);
                                        tw.Write(Custom);
                                        tw.WriteLine("\t\t" + DataEnd);

                                        if (!CustomAACPresent)
                                        {
                                            tw.WriteLine("\t\t" + AACCustomKey);
//                                            tw.WriteLine(line);
                                            tw.WriteLine("\t\t" + DataStart);
                                            tw.Write(AACEncode);
                                            tw.WriteLine("\t\t" + DataEnd);
                                        }

                                        CurrentState = States.None;
                                    }
                                    if (!EncodeMP3Present)
                                    {
                                        tw.WriteLine("\t\t" + MP3EncoderKey);
                                        tw.WriteLine("\t\t" + DataStart);
                                        tw.Write(Custom);
                                        tw.WriteLine("\t\t" + DataEnd);

                                        if (!CustomMP3Present)
                                        {
                                            tw.WriteLine("\t\t" + MP3CustomKey);
                                            // tw.WriteLine(line);
                                            tw.WriteLine("\t\t" + DataStart);
                                            tw.Write(MP3Encode);
                                            tw.WriteLine("\t\t" + DataEnd);
                                        }

                                        CurrentState = States.None;                                        
                                    }

                                    CurrentState = States.None;
                                    break;

                                case States.FoundAACEncoder:
                                    tw.WriteLine(line);
                                    tw.WriteLine("\t\t" + DataStart);
                                    tw.Write(Custom);
                                    //don't write data end because we're skipping until we reach the existing one

                                    if (!CustomAACPresent)
                                    {
                                        tw.WriteLine("\t\t" + DataEnd); //terminate the encoder block

                                        tw.WriteLine("\t\t" + AACCustomKey);
//                                        tw.WriteLine(line);
                                        tw.WriteLine("\t\t" + DataStart);
                                        tw.Write(AACEncode);
                                        //don't write data end because we're skipping until we reach the existing one

                                    }

                                    CurrentState = States.InsideData;
                                    break;

                                case States.FoundMP3Encoder:
                                    tw.WriteLine(line);
                                    tw.WriteLine("\t\t" + DataStart);
                                    tw.Write(Custom);
                                    //don't write data end because we're skipping until we reach the existing one

                                    if (!CustomMP3Present)
                                    {
                                        tw.WriteLine("\t\t" + DataEnd); //terminate the encoder block

                                        tw.WriteLine("\t\t" + MP3CustomKey);
                                        //tw.WriteLine(line);
                                        tw.WriteLine("\t\t" + DataStart);
                                        tw.Write(MP3Encode);
                                        //don't write data end because we're skipping until we reach the existing one
                                    }

                                    CurrentState = States.InsideData;
                                    break;

                                case States.FoundAACCustom:
                                    tw.WriteLine(line);
                                    tw.WriteLine("\t\t" + DataStart);
                                    tw.Write(AACEncode);
                                    //don't write data end because we're skipping until we reach the existing one

                                    CurrentState = States.InsideData;
                                    break;

                                case States.FoundMP3Custom:
                                    tw.WriteLine(line);
                                    tw.WriteLine("\t\t" + DataStart);
                                    tw.Write(MP3Encode);
                                    //don't write data end because we're skipping until we reach the existing one

                                    CurrentState = States.InsideData;
                                    break;

                                case States.InsideData:
                                    //do nowt - skip the line (we're looking for data end)
                                    break;
                            }
                        }
                        tw.Close();
                    }
                    tr.Close();
                }

                try
                {
                    if (File.Exists(PrefsBackup))
                        File.Delete(PrefsBackup);

                    File.Move(PrefsFile,PrefsBackup);

                    File.Move(PrefsModified,PrefsFile);
                }
                catch (Exception)
                {
                    return false;
                }

            }
            return true;
        }

        public static bool RateIsAlreadySet(int bitrate)
        {
            string AACEncode = AAC96kbps;
            string MP3Encode = MP396kbps;

            switch (bitrate)
            {
                case 64:
                    AACEncode = AAC64kbps;
                    MP3Encode = MP364kbps;
                    break;
                case 96:
                    AACEncode = AAC96kbps;
                    MP3Encode = MP396kbps;
                    break;
                case 128:
                    AACEncode = AAC128kbps;
                    MP3Encode = MP3128kbps;
                    break;
                case 256:
                    AACEncode = AAC256kbps;
                    MP3Encode = MP3256kbps;
                    break;
            }


                    var sbAACEncoder = new StringBuilder();
                    var sbMP3Encoder = new StringBuilder();
                    var sbAACCustom = new StringBuilder();
                    var sbMP3Custom = new StringBuilder();

            using (TextReader tr = new StreamReader(PrefsFile, Encoding.UTF8))
            {
                    string line, templine;


                    while ((line = tr.ReadLine()) != null)
                    {
                        if (line.IndexOf(AACEncoderKey) >= 0)
                        {
                            CurrentState = States.FoundAACEncoder;
                        }
                        if (line.IndexOf(MP3EncoderKey) >= 0)
                        {
                            CurrentState = States.FoundMP3Encoder;
                        }
                        if (line.IndexOf(AACCustomKey) >= 0)
                        {
                            CurrentState = States.FoundAACCustom;
                        }
                        if (line.IndexOf(MP3CustomKey) >= 0)
                        {
                            CurrentState = States.FoundMP3Custom;
                        }

                        if (line.IndexOf(DataEnd) >= 0)
                        {
                            CurrentState = States.None;
                        }

                        switch (CurrentState)
                        {
                            case States.None:
                                //do nowt
                                break;

                            case States.FoundAACEncoder:
                                tr.ReadLine(); //should be DataStart
                                templine = "";
                                while (templine.IndexOf(DataEnd) == -1)
                                {
                                    if (templine != "")
                                        sbAACEncoder.Append(templine + "\r\n");
                                    templine = tr.ReadLine();
                                }
                                CurrentState = States.None;
                                break;

                            case States.FoundMP3Encoder:
                                tr.ReadLine(); //should be DataStart
                                templine = "";
                                while (templine.IndexOf(DataEnd) == -1)
                                {
                                    if (templine != "")
                                        sbMP3Encoder.Append(templine + "\r\n");
                                    templine = tr.ReadLine();
                                }
                                CurrentState = States.None;
                                break;


                            case States.FoundAACCustom:
                                tr.ReadLine(); //should be DataStart
                                templine = "";
                                while (templine.IndexOf(DataEnd) == -1)
                                {
                                    if (templine != "")
                                        sbAACCustom.Append(templine + "\r\n");
                                    templine = tr.ReadLine();
                                }
                                CurrentState = States.None;
                                break;

                            case States.FoundMP3Custom:
                                tr.ReadLine(); //should be DataStart
                                templine = "";
                                while (templine.IndexOf(DataEnd) == -1)
                                {
                                    if (templine != "")
                                        sbMP3Custom.Append(templine + "\r\n");
                                    templine = tr.ReadLine();
                                }
                                CurrentState = States.None;
                                break;

                            case States.InsideData:
                                //do nowt - skip the line (we're looking for data end)
                                break;
                        }
                    }

                tr.Close();
            }

            if (Global.Options.Encoder == Global.EncodeToTypes.AAC)
            {
                if (sbAACEncoder.ToString().Trim() != Custom.Trim())
                    return false;

                if (sbAACCustom.ToString().Trim() != AACEncode.Trim())
                    return false;
            }

            if (Global.Options.Encoder == Global.EncodeToTypes.MP3)
            {
                if (sbMP3Encoder.ToString().Trim() != Custom.Trim())
                    return false;

                if (sbMP3Custom.ToString().Trim() != MP3Encode.Trim())
                    return false;
            }

            return true;
        }

        public static bool CustomRateIsAlreadySet()
        {
            if (Global.Options.ChangePrefs == false)
                return true; //true because we don't want to take any action.

            return RateIsAlreadySet(Global.Options.BitRate);
        }

        public static bool RestorePrefs()
        {
            if (!HaveChanged())
                return true;

            try
            {
                if (File.Exists(PrefsBackup))
                {
                    if (File.Exists(PrefsFile))
                        File.Delete(PrefsFile);

                    File.Move(PrefsBackup, PrefsFile);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static void DeleteBackup()
        {
            try
            {
                if (File.Exists(PrefsBackup))
                {
                    File.Delete(PrefsBackup);
                }
            }
            catch (Exception)
            {
                //do nowt
            }
        }

        public static string GetLibraryLocation()
        {
            byte[] bytes =
                Convert.FromBase64String(
                    "QwA6AFwARABvAGMAdQBtAGUAbgB0AHMAIABhAG4AZAAgAFMAZQB0AHQAaQBuAGcAcwBcAEQAYQB2AGkAZABcAE0AeQAgAEQAbwBjAHUAbQBlAG4AdABzAFwATQB5ACAATQB1AHMAaQBjAFwAaQBUAHUAbgBlAHMAXABpAFQAdQBuAGUAcwAgAE0AdQBzAGkAYwAgAEwAaQBiAHIAYQByAHkALgB4AG0AbAA=");

            return Encoding.Unicode.GetString(bytes);
        }
    }
}
