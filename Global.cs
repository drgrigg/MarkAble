using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;
using System.Runtime.InteropServices; //for calling Win32 API to eject and close CD


namespace MarkAble2
{
    public class Global
    {
        public const string Version = "2.4.6";

        public static uint SampleRate = 44100;

        private const string MyRegKey = @"Software\Rightword\MarkAble2\";

        public static uint StandardTimeScale = 2400;

        public static FileListCollection ListsToMerge = new FileListCollection();

        public enum FileTypes
        {
            AAC,
            MP2,
            MP3,
            WMA,
            WAV
        }

        public enum EncodeToTypes
        {
            AAC,
            MP3
        }

        public enum ChapterTypes
        {
            None,
            ByTime,
            ByDiscAndTrack,
            BySourceFile,
            ByTimeWithDisc
        }

        public enum ProcessModes
        {
            None,
            CDs,
            Other,
            AddedToBatch,
            ProcessingBatch
        }

        public static ProcessModes ProcessMode = ProcessModes.None;

        //public static double RegularMins;
        //public static int NumParts = 1;


        public static OptionClass Options = new OptionClass();

        public static TrackInfoList RippedTracks;
        public static CDFileList RippedCDFiles;
        public static SeparateFileList UnRippedFiles;
        public static SeparateFileList ConvertedSeparateFiles;
        public static BatchList CurrentBatch;

        public static int iTunesWaitSecs = 120;


        public static bool SkipAtStart;
        public static float SecsToSkip;

        public static StringBuilder DebugLog = new StringBuilder();

        public static int ResumeDisc;

        public static int CurrentDiscNum;
        public static bool ProcessingFirstCd = true;

        public enum OverwriteTypes
        {
            Overwrite, Rename, Cancel
        }
        public static OverwriteTypes OverwriteResult = OverwriteTypes.Overwrite;
        public static string RenameFile;



        public static FileTypes ConvertEncoderToFileType(EncodeToTypes encodeType)
        {
            switch(encodeType)
            {
                case EncodeToTypes.MP3:
                    return FileTypes.MP3;
                default:
                    return FileTypes.AAC;
            }
        }

        public static EncodeToTypes ConvertFileTypeToEncoder(FileTypes fileType)
        {
            switch(fileType)
            {
                case FileTypes.MP3:
                    return EncodeToTypes.MP3;
                default:
                    return EncodeToTypes.AAC;
            }
        }


//        public static iTunesApp myITunes;
        public static bool Disabled;

        public static bool SaveSetting(string Key, string Value)
        {
            RegistryKey sk;
            RegistryKey regkey = Registry.CurrentUser;
            try
            {
                sk = regkey.CreateSubKey(MyRegKey);
                if (sk != null)
                {
                    sk.SetValue(Key, Value);
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetSetting(string Key)
        {
            RegistryKey sk;
            RegistryKey regkey = Registry.CurrentUser;
            try
            {
                sk = regkey.OpenSubKey(MyRegKey, false);
                if (sk != null) return sk.GetValue(Key).ToString();
            }
            catch
            {
                return "";
            }
            return "";
        }

        public static string GetSetting(string Key, string Default)
        {
            RegistryKey sk;
            RegistryKey regkey = Registry.CurrentUser;
            try
            {
                sk = regkey.OpenSubKey(MyRegKey, false);
                if (sk != null)
                {
                    return sk.GetValue(Key).ToString();
                }
                return Default;
            }
            catch
            {
                return Default;
            }
        }


        public static string GetDrive(string aPath)
        {
            if (aPath.Length < 3)
                return "C";

            string temp = aPath.Substring(0, 3);
            if (temp.IndexOf(":") > 0)
            {
                return aPath.Substring(0, 1).ToUpper(); //eg "C"
            }
            return "";
        }

        public static string GetFilename(string aPath)
        {
            int lastSlash = aPath.LastIndexOf(@"\");
            return aPath.Substring(lastSlash + 1);
        }

        public static string GetFilename(string aPath, bool withExtent)
        {
            int lastSlash = aPath.LastIndexOf(@"\");
            if (withExtent)
                return aPath.Substring(lastSlash + 1);
            
            int lastDot;

            string temp = aPath.Substring(lastSlash + 1);
            lastDot = temp.LastIndexOf(@".");
            if (lastDot < 1)
                lastDot = temp.Length;
            return temp.Substring(0, lastDot);
        }

        //note this returns extension as lowercase, without the dot.
        public static string GetExtension(string aPath)
        {
            int lastDot = aPath.LastIndexOf(@".");
            return aPath.Substring(lastDot + 1).ToLower();
        }

        public static string GetPath(string aPath)
        {
            int lastSlash = aPath.LastIndexOf(@"\");
            if (lastSlash < 0)
                return aPath;
            return aPath.Substring(0, lastSlash);
        }

        public static string GetParentPath(string aPath)
        {
            return GetPath(GetPath(aPath));
        }

        public static string GetParentFolderName(string filename)
        {
            return GetFilename(GetPath(filename));
        }

        public static string LeadingZero(decimal aNum, int width)
        {
            return aNum.ToString().PadLeft(2, (char)48);
        }

        public static string LeadingZero(int aNum, int width)
        {
            return aNum.ToString().PadLeft(2, (char)48);
        }

        public static string LeadingZero(long aNum, int width)
        {
            return aNum.ToString().PadLeft(2, (char)48);
        }

        public static int Ceiling(float aNum)
        {
            int anInteger = (int)aNum;
            if ((aNum - anInteger) > 0) anInteger++;
            return anInteger;
        }

        public static bool IsNumeric(string aString)
        {
            if (aString.Length == 0)
                return false;
            char[] tempChars = aString.ToCharArray();
            bool isOK = true;
            foreach (char C in tempChars)
            {
                if ((!Char.IsNumber(C)) && (C != '.'))
                    isOK = false;
            }
            return isOK;
        }

        public static string MakePattern(string aString)
        {
            if (aString.Length == 0)
                return "";

            var retString = new StringBuilder();
            char[] tempChars = aString.ToCharArray();
            foreach (char C in tempChars)
            {
                if (Char.IsNumber(C))
                    retString.Append("?"); //question mark
                else
                    retString.Append(C.ToString());
            }
            return retString.ToString();

        }

		

        public static string CleanUp(string aString)
        {
            var tempStr = new StringBuilder();
            tempStr.Append(aString);
            tempStr.Replace("*", "_");
            tempStr.Replace("?", "_");
            tempStr.Replace("/", "_");
            tempStr.Replace(@"\", "_");
            tempStr.Replace("/", "_");
            tempStr.Replace(":", "-");
            tempStr.Replace("<", "(");
            tempStr.Replace(">", ")");
            tempStr.Replace((char)34, (char)39); //quotes
            if (tempStr.Length > 25) //too long for iTunes
                tempStr.Length = 25;
            return tempStr.ToString();
        }


        public static void LogIt(string aString)
        {
            DebugLog.Append(DateTime.Now.ToLongTimeString() + " " + aString + "\r\n");
        }

        public static void LogIt(string aString, string extra)
        {
            DebugLog.Append(DateTime.Now.ToLongTimeString() + " " + aString + ", " + extra + "\r\n");
        }

        public static void LogLine()
        {
            DebugLog.Append("--------------------------------------------------------------------------------------\r\n");
        }

        public static void LogImmediate(string aString)
        {
            LogIt(aString);
            SaveLog();
        }

        public static void LogImmediate(string aString, string extra)
        {
            LogIt(aString, extra);
            SaveLog();
        }

        public static void SaveLog(string LogFile)
        {
            if (DebugLog.Length == 0)
                return; //nothing to do!

            if (!Options.DebugMode)
                return;

            try
            {
                StreamWriter sr = File.AppendText(LogFile);
                sr.Write(DebugLog.ToString());
                sr.Flush();
                sr.Close();
                ClearLog();
            }
            catch
            {
                //do nowt
            }
        }

        public static void SaveLog()
        {
            if (DebugLog.Length == 0)
                return; //nothing to do!

            if (!Options.DebugMode)
                return;

            DateTime today = DateTime.Today;
            string filename = today.Year.ToString("0000") + today.Month.ToString("00") + today.Day.ToString("00") + ".log";
            string LogFolder = Options.RipFolder;
            if (LogFolder == "")
                LogFolder = @"C:\temp\logs";

            try
            {
                Directory.CreateDirectory(LogFolder);
            }
            catch
            {
                //do nowt
            }

            SaveLog(LogFolder + @"\" + filename);
        }

        public static void ClearLog()
        {
            DebugLog.Remove(0, DebugLog.Length);
        }

        public static string MyDocuments()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public static string LocalAppData()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        public static string AppData()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }


        public static bool WriteLocalAppData(string AppFolder, string Filename, string ToWrite)
        {
            StreamWriter SW;
            string myAppData = LocalAppData();
            if (Directory.Exists(myAppData))
            {
                if (!Directory.Exists(myAppData + @"\" + AppFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(myAppData + @"\" + AppFolder);
                    }
                    catch
                    {
                        //do nowt
                    }
                }
                try
                {
                    SW = File.CreateText(myAppData + @"\" + AppFolder + @"\" + Filename + ".ini");
                    SW.WriteLine(ToWrite);
                    SW.Flush();
                    SW.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    string Temp = ex.Message;
                    LogIt("WriteLocalAppData",Temp);
                    return false;
                }
            }
            LogIt("WriteLocalAppData","Couldn't locate local application folder");
            return false;
        }

        public static string ReadAppData(string AppFolder, string Filename)
        {
            StreamReader SR;
            string myAppData = LocalAppData();
            try
            {
                SR = File.OpenText(myAppData + @"\" + AppFolder + @"\" + Filename + ".ini");
                string tempStr = SR.ReadLine();
                SR.Close();
                return tempStr;
            }
            catch
            {
                return "";
            }
        }


        //This is necessary to enable Win32 API calls to eject or close the CD tray

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA")]
        public static extern void mciSendStringA(string lpstrCommand, string lpstrReturnString, Int32 uReturnLength, Int32 hwndCallback);
        private static string emptyString = "";



        public static void EjectCD(string driveletter)
        {
            //some weird users don't want discs ejected!
            if (Options.EjectDiscs == false)
                return;

            try
            {
                mciSendStringA("Open " + driveletter + " type CDAudio alias CDDrawer1", emptyString, 0, 0);
                mciSendStringA("set CDDrawer1 door open", emptyString, 127, 0);
                //this lets go of the device
                mciSendStringA("Close CDDrawer1", emptyString, 127, 0);
            }
            catch (Exception ex)
            {     
                LogImmediate("EjectCD",ex.Message);
            }
        }

        //public static void CloseCD(string driveletter)
        //{
        //    try
        //    {
        //        mciSendStringA("Open " + driveletter + " type CDAudio alias CDDrawer1", emptyString, 0, 0);
        //        mciSendStringA("set CDDrawer1 door closed", emptyString, 127, 0);
        //        mciSendStringA("Close CDDrawer1", emptyString, 127, 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogImmediate("CloseCD", ex.Message);
        //    }
        //}


        // -------------- XML Serialization Routines -------------------------//

        /* following two methods are taken from the Source Project web site:
         * ".NET XML and SOAP Serialization Samples, Tips"
         * By goxman
         * http://www.codeproject.com/soap/Serialization_Samples.asp  
         * 
         * slightly modified by David Grigg */

        /// <summary>
        /// Serializes an object to an XML string
        /// </summary>
        public static string ToXml(object objToXml,
            bool includeNameSpace)
        {
            StreamWriter stWriter = null;
            XmlSerializer xmlSerializer;
            string buffer;
            try
            {
                xmlSerializer =
                    new XmlSerializer(objToXml.GetType());
                MemoryStream memStream = new MemoryStream();
                stWriter = new StreamWriter(memStream);

                if (!includeNameSpace)
                {
                    XmlSerializerNamespaces xs = new XmlSerializerNamespaces();
                    //To remove namespace and any other inline 
                    //information tag                      
                    xs.Add("", "");
                    xmlSerializer.Serialize(stWriter, objToXml, xs);
                }
                else
                {
                    xmlSerializer.Serialize(stWriter, objToXml);
                }
                buffer = Encoding.UTF8.GetString(memStream.GetBuffer());
            }
            catch (Exception ex)
            {
                LogIt("ToXML", ex.Message);
                return null;
            }
            finally
            {
                if (stWriter != null) stWriter.Close();
            }
            return buffer;
        }

        /// <summary>
        /// Loads a class from an XML string.
        /// </summary>
        public static object FromXml(string xmlString, Type ExpectedType)
        {
            XmlSerializer xmlSerializer;
            MemoryStream memStream = null;
            try
            {
                xmlSerializer = new XmlSerializer(ExpectedType);
                var bytes = new byte[xmlString.Length];

                Encoding.UTF8.GetBytes(xmlString, 0, xmlString.Length, bytes, 0);
                memStream = new MemoryStream(bytes);
                object objectFromXml = xmlSerializer.Deserialize(memStream);
                return objectFromXml;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (memStream != null) memStream.Close();
            }
        }

        /// <summary>
        /// Loads a class from an XML file.
        /// </summary>
        public static object FromXmlFile(string filename, Type ExpectedType)
        {
            XmlSerializer xmlSerializer;

            if (!File.Exists(filename))
                return null;

            FileStream fStream = null;
            try
            {
                fStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                xmlSerializer = new XmlSerializer(ExpectedType);
                object objectFromXml = xmlSerializer.Deserialize(fStream);
                return objectFromXml;
            }
            catch (Exception ex)
            {
                LogIt("FromXmlFile:" + filename + ":" + ExpectedType.ToString(), ex.Message);
                return null;
            }
            finally
            {
                if (fStream != null) fStream.Close();
            }
        }


        /// <summary>
        /// Serializes an object to an XML file.
        /// </summary>
        public static bool ToXmlFile(string filename, object objToXml, bool includeNameSpace)
        {
            StreamWriter stWriter = null;
            XmlSerializer xmlSerializer;
            bool retVal;
            try
            {
                xmlSerializer =
                    new XmlSerializer(objToXml.GetType());
                var fStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                stWriter = new StreamWriter(fStream);

                if (!includeNameSpace)
                {
                    var xs = new XmlSerializerNamespaces();
                    //To remove namespace and any other inline 
                    //information tag                      
                    xs.Add("", "");
                    xmlSerializer.Serialize(stWriter, objToXml, xs);
                }
                else
                {
                    xmlSerializer.Serialize(stWriter, objToXml);
                }

                retVal = true;
            }
            catch (Exception ex)
            {
                LogIt("MsgGlobals.ToXMLFile", ex.Message);
                SaveLog();
                retVal = false;
            }
            finally
            {
                if (stWriter != null) stWriter.Close();
            }
            return retVal;
        }

        //returns a string safe for use in a file path
        public static string SafeFile(string title)
        {
            var sb = new StringBuilder();
            sb.Append(title);
            sb.Replace(':', '_');
            sb.Replace('/', '_');
            sb.Replace('\\', '_');
            sb.Replace('*', '_');
            sb.Replace('?', '_');
//            sb.Replace('.', '_');
            return sb.ToString();
        }

        public static string SimpleDuration(TimeSpan span)
        {
            return span.Days > 0
                       ? span.Days.ToString("00") + "." + span.Hours.ToString("00") + ":" + span.Minutes.ToString("00") +
                         ":" + span.Seconds.ToString("00.0")
                       : span.Hours.ToString("00") + ":" + span.Minutes.ToString("00") +
                         ":" + span.Seconds.ToString("00.0");
        }



        private enum States
        {
            Alpha,
            Numeric
        }

        public static string CreateSortable(string aString)
        {
            if (String.IsNullOrEmpty(aString))
                return "";

            char[] chars = aString.ToCharArray();

            var sb = new StringBuilder();

            States CurrentState;

            if ((chars[0] >= '0') && (chars[0] <= '9'))
                CurrentState = States.Numeric;
            else
                CurrentState = States.Alpha;

            foreach (char c in chars)
            {
                if ((c >= '0') && (c <= '9'))
                {
                    if (CurrentState == States.Alpha)
                    {
                        sb.Append(':');
                        CurrentState = States.Numeric;
                    }
                }
                else
                {
                    if (CurrentState == States.Numeric)
                    {
                        sb.Append(':');
                        CurrentState = States.Alpha;
                    }
                }
                sb.Append(c);
            }

            string[] parts = sb.ToString().Split(':');
            if (parts.Length <= 1)
            {
                return aString;
            }

            sb = new StringBuilder();
            foreach (string part in parts)
            {
                if (!String.IsNullOrEmpty(part))
                {
                    if ((part[0] >= '0') && (part[0] <= '9'))
                    {
                        double value = Convert.ToDouble(part);
                        sb.Append(value.ToString("00000"));
                    }
                    else
                    {
                        //handle roman numerals!!! (at least up to 20) Must be in caps to qualify, eg iii, iv won't work.
                        bool isRoman;
                        double romanValue = 0;
                        switch (part)
                        {
                            case "I":
                                isRoman = true;
                                romanValue = 1;
                                break;
                            case "II":
                                isRoman = true;
                                romanValue = 2;
                                break;
                            case "III":
                                isRoman = true;
                                romanValue = 3;
                                break;
                            case "IV":
                                isRoman = true;
                                romanValue = 4;
                                break;
                            case "V":
                                isRoman = true;
                                romanValue = 5;
                                break;
                            case "VI":
                                isRoman = true;
                                romanValue = 6;
                                break;
                            case "VII":
                                isRoman = true;
                                romanValue = 7;
                                break;
                            case "VIII":
                                isRoman = true;
                                romanValue = 8;
                                break;
                            case "IX":
                                isRoman = true;
                                romanValue = 9;
                                break;
                            case "X":
                                isRoman = true;
                                romanValue = 10;
                                break;
                            case "XI":
                                isRoman = true;
                                romanValue = 11;
                                break;
                            case "XII":
                                isRoman = true;
                                romanValue = 12;
                                break;
                            case "XIII":
                                isRoman = true;
                                romanValue = 13;
                                break;
                            case "XIV":
                                isRoman = true;
                                romanValue = 14;
                                break;
                            case "XV":
                                isRoman = true;
                                romanValue = 15;
                                break;
                            case "XVI":
                                isRoman = true;
                                romanValue = 16;
                                break;
                            case "XVII":
                                isRoman = true;
                                romanValue = 17;
                                break;
                            case "XVIII":
                                isRoman = true;
                                romanValue = 18;
                                break;
                            case "XIX":
                                isRoman = true;
                                romanValue = 19;
                                break;
                            case "XX":
                                isRoman = true;
                                romanValue = 20;
                                break;
                            default:
                                isRoman = false;
                                break;
                        }
                        if (isRoman)
                        {
                            sb.Append(romanValue.ToString("00000"));
                        }
                        else
                            sb.Append(part);
                    }
                }
            }
            return sb.ToString();
        }


        public static bool WriteAppData(string AppFolder, string Filename, string ToWrite)
        {
            StreamWriter SW;
            string myAppData = LocalAppData();
            if (Directory.Exists(myAppData))
            {
                if (!Directory.Exists(myAppData + @"\" + AppFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(myAppData + @"\" + AppFolder);
                    }
                    catch
                    {
                        //do nowt
                    }
                }
                try
                {
                    SW = File.CreateText(myAppData + @"\" + AppFolder + @"\" + Filename + ".ini");
                    SW.WriteLine(ToWrite);
                    SW.Flush();
                    SW.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public static string MyMusic()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        }


        public static void BuildListsToMerge(FileListBase fileList)
        {
            ListsToMerge.Clear();

            if (fileList.NumParts == 1)
            {
                ListsToMerge.Add(fileList);
                return;
            }

            if (fileList.NumParts >= fileList.Count)
            {
                //each part gets its own file
                for (int i = 0; i < fileList.Count; i++)
                {
                    var frec = fileList[i];
                    
                    var ripList = new FileListBase();
                    ripList.Clone(fileList);

                    ripList.Add(frec);
                    ListsToMerge.Add(ripList);
                }

                return;
            }


            //we'll 'deal' files into a series of file lists and store them in Global.ListsToMerge.

            //divide the size - do it by size rather than duration because we always have the size.
            long totalSize = fileList.TotalSize();
            //this is the max size for each part
            long partSize = Convert.ToInt64(totalSize / fileList.NumParts);

            bool happy;

            int attempts = 0;

            do
            {
                ListsToMerge.Clear();
                var CopyConvertedFiles = new FileListBase();
                for (int i = 0; i < fileList.Count;i++)
                {
                    var frec = fileList[i];
                    CopyConvertedFiles.Add(frec);
                }

                //deal them out until we're done.
                while (CopyConvertedFiles.Count > 0)
                {
                    if (ListsToMerge.Count < fileList.NumParts)
                    {
                        var ripList = new FileListBase();
                        ripList.Clone(fileList);

                        while ((CopyConvertedFiles.Count > 0) && ((ripList.TotalSize() + CopyConvertedFiles[0].Size) < partSize))
                        {
                            ripList.Add(CopyConvertedFiles[0]);
                            CopyConvertedFiles.RemoveAt(0);
                        }
                        ListsToMerge.Add(ripList);
                    }
                    else //we've run out of parts, add remaining files to last list.
                    {
                        FileListBase lastList = ListsToMerge[ListsToMerge.Count - 1];
                        lastList.Clone(fileList);

                        while ((CopyConvertedFiles.Count > 0))
                        {
                            lastList.Add(CopyConvertedFiles[0]);
                            CopyConvertedFiles.RemoveAt(0);
                        }
                    }
                }

                //are we happy? - if not, increase part size and try again
                happy = true;
                //if any list is much bigger than part size, we're not happy
                long partSizeMax = Convert.ToInt64(partSize * 1.2);

                foreach(var list in ListsToMerge)
                {
                    if (list.TotalSize() > partSizeMax)
                        happy = false;

                    if (list.Count == 0)
                        happy = false;
                }

                if (!happy)
                {
                    //let's try increasing the part size
                    partSize = partSizeMax;
                    attempts++;
                }

                Application.DoEvents();

            } while ((!happy) && (attempts < 20));
        }


        //---------------------------------------

        public static string QualifyTemplate(string text)
        {
            return text;
        }

        public static string PadZero(int num, int ofnum)
        {
            if (ofnum < 10)
            {
                return num.ToString("0");
            }
            if (ofnum < 100)
            {
                return num.ToString("00");
            }
            return num.ToString("000");
        }

        public static void ResizeForDPI(Form frm)
        {
            //System.Drawing.Graphics graph = Graphics.FromHwnd(frm.Handle);
            //double scalex = graph.DpiX / 96;
            //double scaley = graph.DpiY / 96;
            //frm.Width = (int)(scalex * frm.Width);
            //frm.Height = (int)(scaley * frm.Height);
        }

        public static Image MakeSmallImage(string filepath)
        {
            if (!File.Exists(filepath))
                return null;

            Image imgbig = Image.FromFile(filepath);
		   
		// get image height, check if more than maximum allowed.
			
			if (imgbig.Height > Options.MaxImageSize)
			{
                try
                {
                    Image imgsmall = ImageTools.ConstrainProportions(imgbig, Options.MaxImageSize, ImageTools.Dimensions.Height);
                    if (imgsmall == null) { throw new InvalidOperationException(); }

                    SafeImageSave(imgsmall, LocalAppData() + @"\MarkAble" + @"\" + "bookimage.png", ImageFormat.Png);
                    return imgsmall;
                }
                catch
                {
                    return imgbig;
                }
			}
			else
			{
				SafeImageSave(imgbig, LocalAppData() + @"\MarkAble" + @"\" + "bookimage.png", ImageFormat.Png);
				return imgbig;
			}		
        }

        public static void SafeImageSave(Image image, string filepath, ImageFormat format)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                image.Save(filepath,format);
            }
            catch (Exception ex)
            {      
                LogImmediate("SafeImageSave",ex.Message);
            }
            
        }

        public static string BookImageFile()
        {
            string filepath = LocalAppData() + @"\MarkAble" + @"\" + "bookimage.png";
            if (File.Exists(filepath))
                return LocalAppData() + @"\MarkAble" + @"\" + "bookimage.png";
            return "";
        }


        public static string GetBatchFolder()
        {
            string BatchFolder = GetPath(Options.RipFolder) + @"\Batchfiles";
            if (!Directory.Exists(BatchFolder))
            {
                try
                {
                    Directory.CreateDirectory(BatchFolder);
                }
                catch (Exception)
                {
                    BatchFolder = Options.RipFolder;
                }
            }
            return BatchFolder;
        }

        public static void KillEmptyDirectories(string foldername)
        {
            string[] folders = Directory.GetDirectories(foldername);
            if (folders.Length > 0)
            {
                //recursive
                foreach(var folder in folders)
                    KillEmptyDirectories(folder);
            }

            folders = Directory.GetDirectories(foldername);
            string[] files = Directory.GetFiles(foldername);
            if ((files.Length == 0) && (folders.Length == 0))
            {
                try
                {
                    Directory.Delete(foldername);
                }
                catch (Exception)
                {    
                    //do nowt
                }
            }
        }


        public static void WriteRippedFilelist()
        {
            try
            {
                ToXmlFile(Options.RipFolder + @"\" + SafeFile(RippedCDFiles.BookTitle) + ".filelist",
                                 RippedCDFiles, false);
                LogImmediate("TransferRippedFiles", "Wrote .filelist : " + Options.RipFolder + @"\" + SafeFile(RippedCDFiles.BookTitle) + ".filelist");

            }
            catch (Exception ex)
            {
                LogImmediate("TransferRippedFiles - Unable to write .filelist : " + Options.RipFolder + @"\" + SafeFile(RippedCDFiles.BookTitle) + ".filelist", ex.Message);
            }
        }


        public static void WriteSeparateList(SeparateFileList.ProcessStages processStage)
        {
            SeparateFileList theList = UnRippedFiles;
            switch(processStage)
            {
                case SeparateFileList.ProcessStages.None:
                case SeparateFileList.ProcessStages.Selected:
                case SeparateFileList.ProcessStages.Sorted:
                case SeparateFileList.ProcessStages.MetaDataAdded:
                case SeparateFileList.ProcessStages.ParametersSet:
                case SeparateFileList.ProcessStages.Converting:
                    theList = UnRippedFiles;
                    break;
                case SeparateFileList.ProcessStages.UnMerged:
                    theList = ConvertedSeparateFiles;
                    break;
            }

            theList.ProcessStage = processStage;

            try
            {
                ToXmlFile(Options.RipFolder + @"\" + SafeFile(theList.BookTitle) + ".separatelist",
                                 theList, false);
            }
            catch (Exception ex)
            {
                LogImmediate("WriteSeparateFilelist - Unable to write: " + Options.RipFolder + @"\" + SafeFile(theList.BookTitle) + ".separatelist", ex.Message);
            }
        }

        public static string GetFirstImage(string path)
        {
            var retVal = GetFirstFileToMatchPattern(path, new string[] {"*.jpg", ".png", "*.bmp", "*.gif"});

            //if (String.IsNullOrEmpty(retVal))
            //{
            //    retVal = GetFirstFileToMatchPattern(GetParentPath(path), new string[] { "*.jpg", ".png", "*.bmp", "*.gif" });
            //}

            return retVal;
        }

        private static string GetFirstFileToMatchPattern(string path, string[] patterns)
        {
            foreach (var pattern in patterns)
            {
                if (path.IndexOf('\\') == -1)
                    path = path + @"\";

                string[] files = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);

                string validimage = GetValidimage(files);
                if (!String.IsNullOrEmpty(validimage))
                {
                    return validimage;
                }

                //keep searching in subdirectories
                files = Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
                validimage = GetValidimage(files);
                if (!String.IsNullOrEmpty(validimage))
                {
                    return validimage;
                }
            }
            return "";
        }

        private static string GetValidimage(string[] files)
        {
            var validimage = "";

            var nonhidden = new List<string>();

            foreach(var file in files)
            {
                var fi = new FileInfo(file);
                if ((fi.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    nonhidden.Add(file);
                }
            }

            if (nonhidden.Count > 0)
            {
                validimage = nonhidden[0];
            }
            return validimage;
        }

        public static List<string> GetFilesMatchingPattern(string path, string[] patterns)
        {
            var allfiles = new List<string>();

            foreach (var pattern in patterns)
            {
                if (path.IndexOf('\\') == -1)
                    path = path + @"\";

                string[] files = Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    foreach(var file in files)
                    {
                      allfiles.Add(file);  
                    }
                }
            }
            return allfiles;
        }

        public static void WaitSecs(double secs)
        {
            DateTime nowTime = DateTime.Now;
            DateTime waitUntil = nowTime.AddMilliseconds(secs*1000);

            do
            {
                nowTime = DateTime.Now;
                Application.DoEvents();

            } while (nowTime < waitUntil);

        }

        public static void DeleteUnknownList()
        {
            if (File.Exists(Options.RipFolder + @"\Unknown.separatelist"))
            {
                try
                {
                    File.Delete(Options.RipFolder + @"\Unknown.separatelist");

                }
                catch
                {         
                    //do nowt
                }
            }
        }

        public static void DeleteSeparateList()
        {
            var sepfile = Options.RipFolder + @"\" + SafeFile(UnRippedFiles.BookTitle) + ".separatelist";

            if (File.Exists(sepfile))
            {
                try
                {
                    File.Delete(sepfile);
                }
                catch
                {
                    //do nowt
                }
            }
        }

        public static Color GetDarkerColor(Color origcolor, double proportion)
        {
            if (proportion < 0) proportion = 0.5;
            if (proportion > 1.0) proportion = 1.0;

            var backcolor = origcolor;
            var newR = Convert.ToInt32(backcolor.R * proportion);
            var newG = Convert.ToInt32(backcolor.G * proportion);
            var newB = Convert.ToInt32(backcolor.B * proportion);

            return Color.FromArgb(255, newR, newG, newB);
        }

    }
}
