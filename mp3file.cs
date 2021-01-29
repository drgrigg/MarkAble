using System;
using System.IO;
using System.Text;
using System.Drawing;

namespace MarkAble2
{
    /// <summary>
    /// Summary description for mp3file.
    /// </summary>
    public class mp3file
    {
        const int ArraySize = 2048;
        private FileStream myFile = null;
        private string myArtist = "";
        private string myGenre = "";
        private string myTrack = "";
        private string myDiscNum = "";
        private string myTitle = "";
        private string myAlbum = "";
        private string myEncoder = "";
        private string myComment = "";
        private string myYear = "";
        private bool myPodcast = false;
        private long myStartData, myEndData, myStartTag, myEndTag;
        private long myID2Size = 0;
        private long myDataLength = 0;
        private long myNumFrames = 0;
        private double myDuration = 0; //in secs
        private double myDurationPerFrame = 0;
        private bool myIsVBR = false;
        private bool myHasXing = false;

        public PictureRecs Pictures = new PictureRecs();

        public enum StringEncodings
        {
            Ascii,
            Unicode16BigEndian,
            Unicode16LittleEndian,
            None
        }


        // this is the bitrate table
        int[] bitrate1 = { 0, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448, -1 };
        int[] bitrate2 = { 0, 32, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 384, -1 };
        int[] bitrate3 = { 0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, -1 };
        int[] bitrate4 = { 0, 32, 48, 56, 64, 80, 96, 112, 128, 144, 160, 176, 192, 224, 256, -1 };
        int[] bitrate5 = { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, -1 };

        public enum MPEGVersion
        {
            VersionBad = -1,
            MPEG25 = 0,
            VersionReserved = 1,
            MPEG2 = 2,
            MPEG1 = 3
        }

        public enum LayerTypes
        {
            LayerBad = -1,
            LayerReserved = 0,
            LayerIII = 1,
            LayerII = 2,
            LayerI = 3
        }

        public enum ChannelTypes
        {
            Stereo = 0,
            JointStereo = 1,
            DualChannel = 2,
            SingleChannel = 3
        }


        public struct frameHeader
        {
            public MPEGVersion Version;
            public LayerTypes Layer;
            public bool Protected;
            public int BitRate;
            public int SamplingRate;
            public int Padding;
            public int Length;
            public int Private;
            public ChannelTypes ChannelMode;
            public int StereoMode;
            public bool Copyrighted;
            public bool Original;
            public int Emphasis;
            public int Samples;
            public long StartFrame;
            public long NextFrame;
            public bool isVBR;
            public int byte1;
            public int byte2;
            public int byte3;
            public int byte4;
            public int Offset;
        }


        public frameHeader CurrentFrame;



        public enum AccessType
        {
            None,
            Reading,
            Writing,
            ReadWrite
        }

        private AccessType myAccess;

        public mp3file()
        {
        }

        public bool Close()
        {
            myAccess = AccessType.None;
            try
            {
                if (myFile != null)
                {
                    try
                    {
                        myFile.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Global.LogIt("Close MP3 file", ex.Message);
                    }
                    myFile = null;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool OpenForRead(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    myFile = File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    myAccess = AccessType.Reading;
                    return true;
                }
                catch
                {
                    myFile = null;
                    myAccess = AccessType.None;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool OpenForWrite(string filename)
        {
            if (!File.Exists(filename))
            {
                try
                {
                    myFile = File.Open(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    myAccess = AccessType.Writing;
                    return true;
                }
                catch
                {
                    myFile = null;
                    myAccess = AccessType.None;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool OpenForReadWrite(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    myFile = File.Open(filename, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
                    myAccess = AccessType.ReadWrite;
                    return true;
                }
                catch
                {
                    myFile = null;
                    myAccess = AccessType.None;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



        private byte[] arrayID1 = new byte[128];

        public bool GetID1(out long Datapos)
        {
            Datapos = -1;
            if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite)) { return false; }
            if (myFile.Length < 128) { return false; }

            // try looking for ID3v1 tag at start
            myFile.Position = 0;
            myFile.Read(arrayID1, 0, 128);
            if ((arrayID1[0] == 84) && (arrayID1[1] == 65) && (arrayID1[2] == 71))
            {
                // found "TAG"
                Datapos = 0;
                return true;  // Datapos already set to 0
            }
            else
            {
                Datapos = myFile.Length - 128;
                myFile.Position = Datapos;
                myFile.Read(arrayID1, 0, 128);
                if ((arrayID1[0] == 84) && (arrayID1[1] == 65) && (arrayID1[2] == 71))
                {
                    // found "TAG"
                    Datapos = myFile.Length - 128;
                    return true;
                }
                else
                {
                    Datapos = -1;
                    return false;
                }
            }
        }

        private byte[] arrayID2 = new byte[ArraySize];

        private int ID2Pos = 0;
        private int versionID2;

        private void ID2_Clear()
        {
            for (int n = 0; n < ArraySize; n++)
            {
                arrayID2[n] = 0;
            }
        }

        public void ID2_SetStart()
        {
            ID2_Clear();
            arrayID2[0] = 73; //I
            arrayID2[1] = 68; //D
            arrayID2[2] = 51; //3
            arrayID2[3] = 03; // version 3
            arrayID2[4] = 0; // flags
            arrayID2[5] = 0; // size starts here
            arrayID2[6] = 0; // 
            arrayID2[7] = 0; // 
            arrayID2[8] = 7; // 
            arrayID2[9] = 118; // end of size (should be 400 hex = 1024) 
            ID2Pos = 10;
        }

        private void ID2_StartFrame(string TagName)
        {
            char[] TagArray = TagName.ToCharArray();
            arrayID2[ID2Pos] = (byte)TagArray[0];
            arrayID2[ID2Pos + 1] = (byte)TagArray[1];
            arrayID2[ID2Pos + 2] = (byte)TagArray[2];
            arrayID2[ID2Pos + 3] = (byte)TagArray[3];
            ID2Pos = ID2Pos + 4;
        }

        private void ID2_SetFrameSize(int aLength)
        {
            byte byte1, byte2, byte3, byte4;
            int int1, int2, int3, int4, remainder;
            int1 = aLength / (128 * 128 * 128);
            remainder = aLength - (int1 * 128 * 128 * 128);
            int2 = remainder / (128 * 128);
            remainder = remainder - (int2 * 128 * 128);
            int3 = remainder / 128;
            remainder = remainder - (int3 * 128);
            int4 = remainder;
            byte1 = (byte)int1;
            byte2 = (byte)int2;
            byte3 = (byte)int3;
            byte4 = (byte)int4;

            arrayID2[ID2Pos] = byte1;
            arrayID2[ID2Pos + 1] = byte2;
            arrayID2[ID2Pos + 2] = byte3;
            arrayID2[ID2Pos + 3] = byte4;
            arrayID2[ID2Pos + 4] = 0; //flag
            arrayID2[ID2Pos + 5] = 0; //flag
            arrayID2[ID2Pos + 6] = 0; //dunno
            ID2Pos = ID2Pos + 7;
        }

        private void ID2_SetString(string aString)
        {
            char[] TagArray = aString.ToCharArray();
            foreach (char C in TagArray)
            {
                arrayID2[ID2Pos] = (byte)C;
                ID2Pos++;
            }
            ID2Pos++; //this 'adds' a zero to end of string
        }

        private void ID2_SetArtist(string anArtist)
        {
            ID2_StartFrame("TPE1");
            ID2_SetFrameSize(anArtist.Length + 2);
            ID2_SetString(anArtist);
        }

        private void ID2_SetAlbum(string anAlbum)
        {
            ID2_StartFrame("TALB");
            ID2_SetFrameSize(anAlbum.Length + 2);
            ID2_SetString(anAlbum);
        }

        private void ID2_SetTitle(string aTitle)
        {
            ID2_StartFrame("TIT2");
            ID2_SetFrameSize(aTitle.Length + 2);
            ID2_SetString(aTitle);
        }

        private void ID2_SetTrack(string aTrack)
        {
            ID2_StartFrame("TRCK");
            ID2_SetFrameSize(aTrack.Length + 2);
            ID2_SetString(aTrack);
        }

        private void ID2_SetDiscNum(string aDiscNum)
        {
            ID2_StartFrame("TPOS");
            ID2_SetFrameSize(aDiscNum.Length + 2);
            ID2_SetString(aDiscNum);
        }

        private void ID2_SetGenre(string aGenre)
        {
            ID2_StartFrame("TCON");
            ID2_SetFrameSize(aGenre.Length + 2);
            ID2_SetString(aGenre);
        }

        private void ID2_SetYear(string aYear)
        {
            ID2_StartFrame("TYER");
            ID2_SetFrameSize(aYear.Length + 2);
            ID2_SetString(aYear);
        }

        private void ID2_SetComment(string aComment)
        {
            ID2_StartFrame("COMM");
            ID2_SetFrameSize(aComment.Length + 2);
            ID2_SetString(aComment);
        }

        private void ID2_SetEncoder(string anEncoder)
        {
            ID2_StartFrame("TENC");
            ID2_SetFrameSize(anEncoder.Length + 2);
            ID2_SetString(anEncoder);
        }


        private void ID2_SetPodcast()
        {
            ID2_StartFrame("PCST");
            ID2_SetFrameSize(4);
        }

        private bool WriteID2()
        {
            try
            {
                myFile.Position = 0;
                myFile.Write(arrayID2, 0, 1024);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool PrependID2()
        {
            if ((this.Access != AccessType.Writing) && (myAccess != AccessType.ReadWrite)) { return false; }
            ID2_SetStart();
            ID2_SetEncoder(myEncoder);
            ID2_SetYear(myYear);
            ID2_SetGenre(myGenre);
            ID2_SetTrack(myTrack);
            ID2_SetAlbum(myAlbum);
            ID2_SetDiscNum(myDiscNum);
            ID2_SetArtist(myArtist);
            ID2_SetTitle(myTitle);
            ID2_SetComment(myComment);
            if (myPodcast)
                ID2_SetPodcast();
            return WriteID2();
        }

        public bool GetID2(out long EndTag)
        {
            EndTag = -1;
            myID2Size = 0;
            if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite)) { return false; }
            if (myFile.Length < 10) { return false; }

            myFile.Position = 0;
            //			myFile.Read(arrayID2,0,ArraySize);
            myFile.Read(arrayID2, 0, 31);
            if ((arrayID2[0] == 73) && (arrayID2[1] == 68) && (arrayID2[2] == 51))
            {
                // found "ID3"
                //calculate length of ID tag block - NOT necessarily the end
                //of the last actual tag.
                myID2Size = (long)arrayID2[6] * 128 * 128 * 128 + (long)arrayID2[7] * 128 * 128 + (long)arrayID2[8] * 128 + (long)arrayID2[9];
                EndTag = myID2Size + 10;
                versionID2 = (int)arrayID2[3];
                return true;
            }
            else
            {
                return false;
            }
        }

        public long ID2_FrameStart()
        {
            byte flags = arrayID2[5];
            if ((flags & 64) == 64) //extended header bit set
            {
                return 20;
            }
            else
            {
                return 10;
            }
        }

        public void ReadID2Frame(long startPos, out long newPos, out string frameType, out string frameText)
        {
            if (versionID2 == 2)
            {
                ReadID2Frame_Short(startPos, out newPos, out frameType, out frameText);
                return;
            }

            newPos = startPos;
            int frameSize;
            frameType = "";
            frameText = "";
            if ((startPos < 0) || (startPos >= myFile.Length)) //invalid array index?
            {
                newPos = -1;
                return;
            }

            //read header
            const int HEADER_SIZE = 10;

            var header = new byte[HEADER_SIZE];

            try
            {
                myFile.Position = startPos;
                myFile.Read(header, 0, HEADER_SIZE);

                if (header[0] == 255) //hit the audio data?
                {
                    newPos = -1;
                    return;
                }

                if (header[0] == 0) //no frame at this location
                {
                    newPos = -1;
                    return;
                }


                var myCode = new StringBuilder();
                var myText = new StringBuilder();

                for (long n = 0; n < 4; n++)
                {
                    if (header[n] != 0)
                        myCode.Append((char)header[n]);
                }

                frameType = myCode.ToString().Trim();


                frameSize = header[4] * 256 * 256 * 256 + header[5] * 256 * 256 + header[6] * 256 + header[7];

                //frameSize = ((header[4] * 256 * 256 * 256) >>3) |
                //    ((header[5] * 256 * 256) >> 2) |
                //    (header[6] * 256) >> 1 |
                //    header[7];


                if (frameSize == 0)
                {
 //                   newPos = -1;
                    newPos += HEADER_SIZE;
                    return;
                }

                int encoding = 0;

                //now we need to process differently based on the code we read.
                if (frameType[0] == 'T') //it's a text frame
                {
                    encoding = GetEncoding();
                    frameSize--;

                    frameText = ReadNullTerminatedString(ref frameSize, encoding);

                    newPos = myFile.Position;
                    return;
                }

                //if we get here, it's not a text frame
                switch (frameType)
                {
                    case "COMM":
                        encoding = GetEncoding();
                        frameSize--;
                        var language = new byte[3];
                        myFile.Read(language, 0, 3);
                        frameSize -= 3;

                        //read through content descriptor
                        var tempText = ReadNullTerminatedString(ref frameSize, encoding);
                        var textBuff = new byte[frameSize];
                        myFile.Read(textBuff, 0, (frameSize));
                        frameText = encoding == 0 ? Encoding.ASCII.GetString(textBuff) : Encoding.Unicode.GetString(textBuff);
                        if (String.IsNullOrEmpty(frameText))
                            frameText = tempText;

                        newPos = myFile.Position;
                        return;

                    case "PIC":
                    case "APIC":
                        encoding = GetEncoding();
                        frameSize--;

                        var pic = new PictureRec();

                        pic.MIMEtype = ReadNullTerminatedString(ref frameSize, 0);
                        var picType = myFile.ReadByte();
                        pic.PictureType = (PictureRec.PictureTypes)picType;

                        frameSize--;
                        frameText = ReadNullTerminatedString(ref frameSize, encoding);
                        var picBuff = new byte[frameSize];
                        myFile.Read(picBuff, 0, frameSize); //read in the image
                        using (var memstream = new MemoryStream(picBuff))
                        {
                            pic.Picture = Image.FromStream(memstream);
                            Pictures.Add(pic);
                            memstream.Close();
                        }
                        newPos = myFile.Position;
                        return;
                }
            }
            catch
            {
                newPos = -1;
                return;
            }
            newPos = myFile.Position;
        }

        private string ReadNullTerminatedString(ref int dataSize, int encoding)
        {
            if (dataSize < 1)
                return String.Empty;

            var sb = new StringBuilder();

            try
            {
                switch (encoding)
                {
                    case 0: //probably ASCII
                            //but have to check first for UTF8
                        var bom = new byte[3];
                        var curpos = myFile.Position; //save this for later restore

                        myFile.Read(bom, 0, 3);
                        if (bom[0] == 0xEF && bom[1] == 0xBB & bom[2] == 0xBF)
                        {
                            //it's UTF8
                            dataSize -= 3;
                            var utfBuff = new byte[dataSize];
                            myFile.Read(utfBuff, 0, dataSize);
                            var tempstr = Encoding.UTF8.GetString(utfBuff);
                            return tempstr.Replace('\0', ' ').Trim();
                        }

                        //must be ASCII, reset file pointer
                        //myFile.Seek(curpos, SeekOrigin.Begin);
                        myFile.Position = curpos;

                        int temp;
                        do
                        {
                            temp = myFile.ReadByte();

                            if (temp > 0)
                            {
                                sb.Append((char)temp);
                            }
                            dataSize--;

                        } while (temp > 0 && dataSize > 0);
                        return sb.ToString();
                    case 1:
                    case 2:
                        //need to read BOM first to determine byte order

                        int temp1 = myFile.ReadByte();
                        dataSize--;
                        int temp2 = myFile.ReadByte();
                        dataSize--;

                        StringEncodings myUnicode = StringEncodings.Unicode16LittleEndian;

                        if (temp1 == 0xFF && temp2 == 0xFE)
                        {
                            myUnicode = StringEncodings.Unicode16BigEndian;
                        }

                        do
                        {
                            temp1 = myFile.ReadByte();
                            dataSize--;
                            temp2 = myFile.ReadByte();
                            dataSize--;
                            if (temp1 > 0 || temp2 > 0)
                            {
                                if (myUnicode == StringEncodings.Unicode16BigEndian)
                                {
                                    sb.Append((char)(temp2 * 256 + temp1));
                                }
                                else
                                {
                                    sb.Append((char)(temp2 + temp1 * 256));
                                }
                            }

                        } while ((temp1 > 0 || temp2 > 0) && dataSize > 0);
                        return sb.ToString();
                    case 3: //UTF8
                        var utfBuff2 = new byte[dataSize];
                        myFile.Read(utfBuff2, 0, dataSize);
                        var tempstr2 = Encoding.UTF8.GetString(utfBuff2);
                        return tempstr2.Replace('\0', ' ').Trim();
                    default:
                        return "";
                }                
            }
            catch
            {
                return "";
            }
        }


        private int GetEncoding()
        {
            var oneByte = myFile.ReadByte();
            return oneByte;
        }

        private void ReadID2Frame_Short(long startPos, out long newPos, out string frameType, out string frameText)
        {
            int frameSize;
            frameType = "";
            frameText = "";
            if ((startPos < 0) || (startPos >= myFile.Length)) //invalid array index?
            {
                newPos = -1;
                return;
            }

            var thisFrame = new byte[256]; // set maximum limit to how big frame data can be

            try
            {
                myFile.Position = startPos;
                myFile.Read(thisFrame, 0, 256);

                if (thisFrame[0] == 255) //hit the audio data?
                {
                    newPos = -1;
                    return;
                }

                if (thisFrame[0] == 0) //no frame at this location
                {
                    newPos = -1;
                    return;
                }


                var myCode = new StringBuilder();

                for (long n = 0; n < 3; n++)
                {
                    if (thisFrame[n] != 0)
                        myCode.Append((char)thisFrame[n]);
                }


                frameType = myCode.ToString().Trim();


                frameSize = (int)thisFrame[3] * 256 * 256 + (int)thisFrame[4] * 256 + (int)thisFrame[5];

                if (frameSize <= 0)
                {
                    newPos = -1;
                    return;
                }

                //?????? reset read position
                myFile.Position = startPos + 7;  //temporary 'fix'
                //????????????

                newPos = myFile.Position + frameSize - 1;

                frameText = ReadNullTerminatedString(ref frameSize, 0);

            }
            catch
            {
                newPos = -1;
                return;
            }
        }


        private string GetUnistring(byte[] tempbuff)
        {
            int zeropos;

            if ((tempbuff[0] == 0xEF) && (tempbuff[1] == 0xBB) && (tempbuff[2] == 0xBF))
            {
                zeropos = GetSingleZero(tempbuff);
                return Encoding.UTF8.GetString(tempbuff, 3, zeropos - 3);
            }
            if ((tempbuff[0] == 0xFE) && (tempbuff[1] == 0xFF))
            {
                zeropos = GetDoubleZero(tempbuff);
                return Encoding.BigEndianUnicode.GetString(tempbuff, 2, zeropos - 2);
            }
            if ((tempbuff[0] == 0xFF) && (tempbuff[1] == 0xFE))
            {
                zeropos = GetDoubleZero(tempbuff);
                return Encoding.Unicode.GetString(tempbuff, 2, zeropos - 2);
            }
            return Encoding.Default.GetString(tempbuff);
        }

        //returns position of double zero
        private int GetDoubleZero(byte[] tempbuff)
        {
            int n = 0;
            bool found = false;
            while ((!found) && (n < (tempbuff.Length - 1)))
            {
                found = (tempbuff[n] == 0 && tempbuff[n + 1] == 0);
                if (!found)
                    n += 2;
            }
            if (!found)
            {
                if (tempbuff.Length % 2 == 1) //length is odd
                {
                    n = tempbuff.Length - 1; //this is even
                }
                else
                {
                    n = tempbuff.Length - 2; //this is even
                }
            }
            return n;
        }

        //returns position of single zero
        private int GetSingleZero(byte[] tempbuff)
        {
            int n = 0;
            bool found = false;
            while ((!found) && (n < (tempbuff.Length)))
            {
                found = (tempbuff[n] == 0);
                if (!found)
                    n++;
            }
            return found ? n : tempbuff.Length - 1;

        }

        private void ProcessID3V2Tags()
        {
            long startPos = ID2_FrameStart();
            if (startPos == -1) { return; }
            long newPos = startPos + 1;
            string frameText;

            while (newPos > 0)
            {
                string frameType;
                ReadID2Frame(startPos, out newPos, out frameType, out frameText);
                switch (frameType)
                {
                    case "TEN":
                    case "TENC":
                        myEncoder = frameText.Trim();
                        break;
                    case "TT2":
                    case "TIT2":
                        myTitle = frameText.Trim();
                        break;
                    case "TAL":
                    case "TALB":
                        myAlbum = frameText.Trim();
                        break;
                    case "TP1":
                    case "TPE1":
                        myArtist = frameText.Trim();
                        break;
                    case "TCO":
                    case "TCON":
                        myGenre = frameText.Trim();
                        break;
                    case "TPO":
                    case "TPOS":
                        myDiscNum = frameText.Trim();
                        break;
                    case "TRC":
                    case "TRK":
                    case "TRCK":
                        myTrack = frameText.Trim();
                        break;
                    case "COM":
                    case "COMM":
                        myComment = frameText.Trim();
                        break;
                    case "TYE":
                    case "TYER":
                        myYear = frameText.Trim();
                        break;
                    case "PCS":
                    case "PCST":
                        myPodcast = true;
                        break;
                    case "PIC":
                    case "APIC":
                        break;

                    default:
                        break;
                }
                startPos = newPos;
                if (startPos == -1) { return; }

            }
        }

        public bool CheckSyncBytes(long StartPos)
        {
            if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite))
            {
                return false;
            }
            return ValidSyncBytes(StartPos);
        }

        public bool FindSyncBytes(long StartPos, out long NewStartPos)
        {
            NewStartPos = -1;
            if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite)) { return false; }
            try
            {
                bool FoundEm = false;
                while ((!FoundEm) && (StartPos < (myFile.Length - 3)))
                {
                    FoundEm = ValidSyncBytes(StartPos);
                    if (!FoundEm)
                        StartPos++;
                }
                if (FoundEm)
                {
                    NewStartPos = StartPos; //the found location of the sync bytes
                    return true;
                }

                NewStartPos = -1; //give up - read through whole file and no sync bytes
                return false;
            }
            catch
            {
                NewStartPos = -1;
                return false;
            }
        }

        private bool ValidSyncBytes(long TestPos)
        {
            if (TestPos >= (myFile.Length - 3))
                return false;

            long restorePos = myFile.Position;

            bool retVal = true;

            try
            {
                myFile.Position = TestPos;

                int byte1 = myFile.ReadByte();
                int byte2 = myFile.ReadByte();
                int byte3 = myFile.ReadByte();

                if (byte1 != 255)
                {
                    retVal = false;
                }
                else
                {
                    int testsync2 = (byte2 >> 5);
                    if (testsync2 != 7)
                        retVal = false;

                    int layer = (byte2 & 0x07) >> 1;
                    if (layer == 0)
                        retVal = false;

                    int bitrate = (byte3 >> 4);
                    if (bitrate == 15)
                        retVal = false;

                    int samprate = (byte3 & 0x0F) >> 2;
                    if (samprate == 3)
                        retVal = false;
                }

            }
            finally
            {
                myFile.Position = restorePos;
            }

            return retVal;
        }


        private string ID1_GetTitle()
        {
            StringBuilder myString = new StringBuilder();
            for (int n = 3; n <= 32; n++)
            {
                if ((arrayID1[n] >= 32) && (arrayID1[n] <= 126))
                    myString.Append((char)arrayID1[n]);
            }
            return myString.ToString().Trim();
        }

        private void ID1_SetStart()
        {
            arrayID1[0] = 84;
            arrayID1[1] = 65;
            arrayID1[2] = 71;
        }

        private void ID1_SetTitle(string aTitle)
        {
            char[] titleArray = new char[30];
            if (aTitle.Length < 30)
                aTitle = aTitle.PadRight(30, (char)32);
            titleArray = aTitle.ToCharArray(0, 30);
            for (int n = 0; n <= 29; n++)
            {
                arrayID1[n + 3] = (byte)titleArray[n];
            }
        }

        private string ID1_GetArtist()
        {
            StringBuilder myString = new StringBuilder();
            for (int n = 33; n <= 62; n++)
            {
                if ((arrayID1[n] >= 32) && (arrayID1[n] <= 126))
                    myString.Append((char)arrayID1[n]);
            }
            return myString.ToString().Trim();
        }

        private void ID1_SetArtist(string anArtist)
        {
            char[] artistArray = new Char[30];
            if (anArtist.Length < 30)
                anArtist = anArtist.PadRight(30, (char)32);
            artistArray = anArtist.ToCharArray(0, 30);
            for (int n = 0; n <= 29; n++)
            {
                arrayID1[n + 33] = (byte)artistArray[n];
            }
        }

        private string ID1_GetAlbum()
        {
            StringBuilder myString = new StringBuilder();
            for (int n = 63; n <= 92; n++)
            {
                if ((arrayID1[n] >= 32) && (arrayID1[n] <= 126))
                    myString.Append((char)arrayID1[n]);
            }
            return myString.ToString().Trim();
        }

        private void ID1_SetAlbum(string anAlbum)
        {
            char[] albumArray = new char[30];
            if (anAlbum.Length < 30)
                anAlbum = anAlbum.PadRight(30, (char)32);
            albumArray = anAlbum.ToCharArray(0, 30);
            for (int n = 0; n <= 29; n++)
            {
                arrayID1[n + 63] = (byte)albumArray[n];
            }
        }

        private string ID1_GetYear()
        {
            StringBuilder myString = new StringBuilder();
            for (int n = 93; n <= 96; n++)
            {
                if ((arrayID1[n] >= 32) && (arrayID1[n] <= 126))
                    myString.Append((char)arrayID1[n]);
            }
            return myString.ToString().Trim();
        }

        private void ID1_SetYear(string aYear)
        {
            char[] YearArray = new char[4];
            if (aYear.Length < 4)
                aYear = aYear.PadRight(4, (char)32);
            YearArray = aYear.ToCharArray(0, 4);
            for (int n = 0; n < 4; n++)
            {
                arrayID1[n + 93] = (byte)YearArray[n];
            }
        }


        private string ID1_GetComment()
        {
            StringBuilder myString = new StringBuilder();
            for (int n = 97; n <= 125; n++)
            {
                if ((arrayID1[n] >= 32) && (arrayID1[n] <= 126))
                    myString.Append((char)arrayID1[n]);
            }
            return myString.ToString().Trim();
        }

        private void ID1_SetComment(string aComment)
        {
            char[] CommentArray = new char[30];
            if (aComment.Length < 30)
                aComment = aComment.PadRight(30, (char)32);
            CommentArray = aComment.ToCharArray(0, 30);
            for (int n = 0; n <= 28; n++)
            {
                arrayID1[n + 97] = (byte)CommentArray[n];
            }
        }

        private void ID1_SetGenre()
        {
            //forces it to "speech"
            arrayID1[126] = 1;
            arrayID1[127] = 101;
        }


        public bool AppendID1()
        {
            if ((this.Access != AccessType.Writing) && (myAccess != AccessType.ReadWrite)) { return false; }
            ID1_SetStart();
            ID1_SetTitle(myTitle);
            ID1_SetArtist(myArtist);
            ID1_SetAlbum(myAlbum);
            ID1_SetComment(myComment);
            ID1_SetYear(myYear);
            ID1_SetGenre();
            try
            {
                myFile.Position = myFile.Length;
                myFile.Write(arrayID1, 0, 128);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public AccessType Access
        {
            get
            {
                return myAccess;
            }
        }

        public bool FindAudioData(out long StartData, out long EndData)
        {
            StartData = 0;
            EndData = myFile.Length;
            if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite)) { return false; }

            long StartTag, EndTag;
            long NewStartPos;

            // first, see if we can find an IDv1 tag
            if (GetID1(out StartTag))
            {
                if (StartTag == 0)  // tag at start of file, so...
                {
                    StartData = 128;
                    EndData = myFile.Length;

                    if (CheckSyncBytes(StartData))
                    {
                        return true; //...can afford to return because mustn't be an IDv2 tag.
                    }

                    if (FindSyncBytes(StartData, out NewStartPos))
                    {
                        StartData = NewStartPos;
                        return true;
                    }
                    return false;
                }
                StartData = 0;
                EndData = myFile.Length - 128;
            }

            if (GetID2(out EndTag))
            {
                StartData = EndTag;
            }

            if (CheckSyncBytes(StartData))
            {
                return true;
            }

            if (FindSyncBytes(StartData, out NewStartPos))
            {
                StartData = NewStartPos;
                return true;
            }

            return false;
        }

        public bool GetAudioData()
        {
            try
            {
                myStartData = -1;
                myEndData = -1;
                if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite)) { return false; }

                // first, see if we can find an IDv1 tag
                if (this.GetID1(out myStartTag))
                {
                    if (myStartTag == 0)  // tag at start of file, so...
                    {
                        myStartData = 128;
                        myEndData = myFile.Length;
                        if (this.CheckSyncBytes(myStartData))
                        {
                            return true; //...can afford to return because mustn't be an IDv2 tag.
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        myStartData = 0;
                        myEndData = myFile.Length - 128;
                    }
                    myArtist = this.ID1_GetArtist();
                    myAlbum = this.ID1_GetAlbum();
                    myTitle = this.ID1_GetTitle();
                    myYear = this.ID1_GetYear();
                    myComment = this.ID1_GetComment();
                }
                if (this.GetID2(out myEndTag))
                {
                    myStartData = myEndTag;
                    //now process ID3v2 tags... tricky!
                    ProcessID3V2Tags();
                    if (myEndData == -1) //wasn't reset by finding ID3v1 tag
                    {
                        myEndData = myFile.Length;
                    }

                    if (myAlbum == "")
                    {
                        myAlbum = myTitle;
                    }

                }
                else
                {
                    myStartData = 0; //no tags, raw data
                    if (myEndData == -1) //wasn't reset by finding ID3v1 tag
                    {
                        myEndData = myFile.Length;
                    }
                }

                myDataLength = myEndData - myStartData;
                if (myDataLength < 0)
                    myDataLength = 0;

                //			if (this.CheckSyncBytes(myStartData))
                if (this.ReadFirstFrame(myStartData))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Global.LogIt("mp3file:GetAudioData", ex.Message);
                return false;
            }
        }


        static byte[] buffer = new byte[ArraySize];  //has to be static to be shared

        public bool FillBuffer(int toFill)
        {
            if (toFill <= 0)
                return false;
            if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite)) { return false; }
            myFile.Read(buffer, 0, toFill);
            return true;
        }

        public bool WriteBuffer(int toWrite)
        {
            if (toWrite <= 0)
                return false;
            if ((this.Access != AccessType.Writing) && (myAccess != AccessType.ReadWrite)) { return false; }
            myFile.Write(buffer, 0, toWrite);
            return true;
        }

        public bool SetPosition(long NewPosition)
        {
            if (myFile != null)
            {
                try
                {
                    myFile.Position = NewPosition;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public bool AppendOneFile(string otherFileName)
        {
            if ((this.Access != AccessType.Writing) && (myAccess != AccessType.ReadWrite)) { return false; }

            mp3file otherFile = new mp3file();
            if (!otherFile.OpenForRead(otherFileName)) { return false; }

            long StartData, EndData, toCopy, Copied;
            otherFile.FindAudioData(out StartData, out EndData);
            if (!otherFile.CheckSyncBytes(StartData)) { return false; }
            toCopy = EndData - StartData;
            Copied = 0;
            //	this.SetPosition(StartPos);
            this.SetPosition(myFile.Length);
            otherFile.SetPosition(StartData);
            int transferred = 0;

            while (Copied < toCopy)
            {
                if ((toCopy - Copied) >= ArraySize)
                {
                    transferred = ArraySize;
                }
                else
                {
                    transferred = (int)(toCopy - Copied);
                }
                otherFile.FillBuffer(transferred);
                this.WriteBuffer(transferred);
                Copied = Copied + Math.Abs(transferred);
            }
            otherFile.Close();
            return true;
        }



        public bool InsertPodcast(string otherFileName)
        {
            byte[] newheader = new byte[24];


            if ((this.Access != AccessType.Writing) && (myAccess != AccessType.ReadWrite)) { return false; }

            mp3file otherFile = new mp3file();
            if (!otherFile.OpenForRead(otherFileName)) { return false; }
            try
            {
                long StartData, EndData, toCopy, Copied;
                otherFile.GetID2(out EndData);
                long theID2Size = otherFile.ID2Size;

                if (otherFile.versionID2 == 2)
                {
                    newheader[0] = 73; // I
                    newheader[1] = 68; // D
                    newheader[2] = 51; // 3
                    newheader[3] = 02; // version 2.2
                    newheader[4] = 00; // flags
                    newheader[5] = 00; // not extended
                    newheader[6] = 00; // size of tag, encoded by 128
                    newheader[7] = 00; // size of tag, encoded by 128
                    newheader[8] = 00; // size of tag, encoded by 128
                    newheader[9] = 10; // size of tag, encoded by 128
                    newheader[10] = 80; // P
                    newheader[11] = 67; // C
                    newheader[12] = 83; // S
                    newheader[13] = 00; // size of frame (10 + 4)
                    newheader[14] = 00; // 
                    newheader[15] = 04; // 
                    newheader[16] = 00; // padding
                    newheader[17] = 00; // 
                    newheader[18] = 00; // 
                    newheader[19] = 00; // 
                }
                else
                {
                    newheader[0] = 73; // I
                    newheader[1] = 68; // D
                    newheader[2] = 51; // 3
                    newheader[3] = 03; // version 2.3
                    newheader[4] = 00; // flags
                    newheader[5] = 00; // not extended
                    newheader[6] = 00; // size of tag, encoded by 128
                    newheader[7] = 00; // size of tag, encoded by 128
                    newheader[8] = 00; // size of tag, encoded by 128
                    newheader[9] = 14; // size of tag, encoded by 128
                    newheader[10] = 80; // P
                    newheader[11] = 67; // C
                    newheader[12] = 83; // S
                    newheader[13] = 84; // T
                    newheader[14] = 00; // size of frame (10 + 4)
                    newheader[15] = 00; // 
                    newheader[16] = 00; // 
                    newheader[17] = 04; // 
                    newheader[18] = 00; // padding
                    newheader[19] = 00; // 
                    newheader[20] = 00; // 
                    newheader[21] = 00; // 
                    newheader[22] = 00; // 
                    newheader[23] = 00; // 
                }


                if (theID2Size == 0) //then no ID2 tag in other file
                    StartData = 0;
                else
                {
                    StartData = otherFile.ID2_FrameStart();

                    long b1, b2, b3, b4;
                    if (otherFile.versionID2 == 2)
                        theID2Size += 10;
                    else
                        theID2Size += 14; //increment it to adjust for new PDCST frame

                    b1 = theID2Size / (128 * 128 * 128);
                    b2 = (theID2Size - (b1 * 128 * 128 * 128)) / (128 * 128);
                    b3 = (theID2Size - (b1 * 128 * 128 * 128) - (b2 * 128 * 128)) / 128;
                    b4 = (theID2Size % 128);
                    newheader[6] = Convert.ToByte(b1); // size of tag, encoded by 128
                    newheader[7] = Convert.ToByte(b2); // size of tag, encoded by 128
                    newheader[8] = Convert.ToByte(b3); // size of tag, encoded by 128
                    newheader[9] = Convert.ToByte(b4); // size of tag, encoded by 128
                }

                if (otherFile.versionID2 == 2)
                    this.myFile.Write(newheader, 0, 20);
                else
                    this.myFile.Write(newheader, 0, 24);

                EndData = otherFile.Length;
                toCopy = EndData - StartData;
                Copied = 0;

                otherFile.SetPosition(StartData);
                int transferred = 0;

                while (Copied < toCopy)
                {
                    if ((toCopy - Copied) >= ArraySize)
                    {
                        transferred = ArraySize;
                    }
                    else
                    {
                        transferred = (int)(toCopy - Copied);
                    }
                    otherFile.FillBuffer(transferred);
                    this.WriteBuffer(transferred);

                    Copied = Copied + Math.Abs(transferred);
                }
            }
            finally
            {
                otherFile.Close();
            }
            return true;
        }




        public bool WriteXing(int VBROffset, int byte1, int byte2, int byte3, int byte4, int FirstFrameLength, long NumFrames)
        {
            if ((this.Access != AccessType.Writing) && (myAccess != AccessType.ReadWrite)) { return false; }
            if (VBROffset > (FirstFrameLength - 12))
                return false;

            byte[] arrayHeader = new byte[FirstFrameLength];
            for (int n = 0; n < FirstFrameLength; n++)
                arrayHeader[n] = 0;

            arrayHeader[0] = Convert.ToByte(byte1);
            arrayHeader[1] = Convert.ToByte(byte2);
            arrayHeader[2] = Convert.ToByte(byte3);
            arrayHeader[3] = Convert.ToByte(byte4);

            arrayHeader[VBROffset] = 88; // X
            arrayHeader[VBROffset + 1] = 105; // i
            arrayHeader[VBROffset + 2] = 110; // n
            arrayHeader[VBROffset + 3] = 103; // g

            arrayHeader[VBROffset + 7] = 1; // Only Frames field is valid

            long b1, b2, b3, b4;

            b1 = NumFrames >> 24;
            b2 = (NumFrames - (b1 * 256 * 256 * 256)) >> 16;
            b3 = (NumFrames - (b1 * 256 * 256 * 256) - (b2 * 256 * 256)) >> 8;
            b4 = (NumFrames % 256);
            arrayHeader[VBROffset + 8] = Convert.ToByte(b1);
            arrayHeader[VBROffset + 9] = Convert.ToByte(b2);
            arrayHeader[VBROffset + 10] = Convert.ToByte(b3);
            arrayHeader[VBROffset + 11] = Convert.ToByte(b4);

            try
            {
                myFile.Write(arrayHeader, 0, FirstFrameLength);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool NewAppendFile(string otherFileName, out long FramesCopied)
        {
            FramesCopied = 0;
            if ((this.Access != AccessType.Writing) && (myAccess != AccessType.ReadWrite)) { return false; }

            mp3file otherFile = new mp3file();
            if (!otherFile.OpenForRead(otherFileName)) { return false; }

            try
            {
                long StartData, EndData;
                int toCopy;
                otherFile.FindAudioData(out StartData, out EndData);
                if (!otherFile.GetAudioData())
                {
                    otherFile.Close();
                    return false;
                }

                if (otherFile.CurrentFrame.isVBR) //then skip it
                    otherFile.ReadNextFrame(otherFile.CurrentFrame.NextFrame);


                if (Global.SkipAtStart)
                {
                    if (Global.SecsToSkip > 0)
                    {
                        if (otherFile.FramesPerSecond > 0)
                        {
                            int toSkip = (int)((double)otherFile.FramesPerSecond * Global.SecsToSkip);

                            for (int n = 0; n < toSkip; n++)
                                otherFile.ReadNextFrame(otherFile.CurrentFrame.NextFrame);
                        }
                    }
                }


                this.SetPosition(myFile.Length);
                otherFile.SetPosition(otherFile.CurrentFrame.StartFrame);

                long NewStart = -1;
                long SearchPos = 0;

                bool ReadOK = true;

                while ((ReadOK) && (otherFile.CurrentFrame.Version != MPEGVersion.VersionBad))
                {
                    toCopy = otherFile.CurrentFrame.Length;
                    otherFile.FillBuffer(toCopy);
                    this.WriteBuffer(toCopy);
                    FramesCopied++;
                    // save it
                    SearchPos = otherFile.CurrentFrame.NextFrame;
                    if (otherFile.ReadNextFrame(otherFile.CurrentFrame.NextFrame))
                        otherFile.SetPosition(otherFile.CurrentFrame.StartFrame);
                    else
                    {
                        otherFile.FindSyncBytes(SearchPos, out NewStart);
                        if (NewStart > -1)
                        {
                            ReadOK = otherFile.ReadNextFrame(NewStart);
                        }
                        else
                        {
                            ReadOK = false;
                        }
                    }
                }
            }
            finally
            {
                otherFile.Close();
            }
            return true;
        }


        public long Length
        {
            get
            {
                if (myFile != null)
                {
                    return myFile.Length;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool GetData()
        {
            if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite))
                return false;
            if (!this.GetAudioData())
                return false;
            //do other stuff
            return true;
        }

        public string Artist
        {
            get
            {
                return myArtist;
            }
            set
            {
                myArtist = value;
            }
        }

        public string Album
        {
            get
            {
                return myAlbum;
            }
            set
            {
                myAlbum = value;
            }
        }

        public string Title
        {
            get
            {
                return myTitle;
            }
            set
            {
                myTitle = value;
            }
        }

        public string Genre
        {
            get
            {
                return myGenre;
            }
            set
            {
                myGenre = value;
            }
        }

        public string Track
        {
            get
            {
                return myTrack;
            }
            set
            {
                myTrack = value;
            }
        }

        public string DiscNum
        {
            get
            {
                return myDiscNum;
            }
            set
            {
                myDiscNum = value;
            }
        }

        public bool Podcast
        {
            get
            {
                return myPodcast;
            }
            set
            {
                myPodcast = value;
            }
        }

        public bool HasXing
        {
            get
            {
                return myHasXing;
            }
        }

        public frameHeader GetHeaderInfo(long StartData) //must be pointing at start of frame
        {
            int byte1;
            int byte2;
            int byte3;
            int byte4;

            var retHeader = new frameHeader { Version = MPEGVersion.VersionBad };

            if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite)) { return retHeader; }

            if (StartData >= (myFile.Length - 4))
                return retHeader;

            if (StartData < 0)
                return retHeader;

            //finding a header is REALLY hard

            bool foundGoodHeader = false;

            while ((!foundGoodHeader) && (StartData < myFile.Length - 4))
            {
                foundGoodHeader = true; //until proved otherwise

                if (ValidSyncBytes(StartData))
                {
                    myFile.Position = StartData;
                    try
                    {
                        byte1 = myFile.ReadByte(); //grumble: why this doesn't return a BYTE ?
                        byte2 = myFile.ReadByte();
                        byte3 = myFile.ReadByte();
                        byte4 = myFile.ReadByte();
                    }
                    catch
                    {
                        return retHeader; // BAD
                    }
                }
                else //not at the start of sync bytes
                {
                    long NewStartPos = -1;
                    if (FindSyncBytes(StartData, out NewStartPos))
                    {
                        if (NewStartPos == -1)
                            return retHeader; //couldn't find any sync bytes in whole file, so get out

                        StartData = NewStartPos;
                        myFile.Position = StartData;
                        try
                        {
                            byte1 = myFile.ReadByte(); //grumble: why this doesn't return a BYTE ?
                            byte2 = myFile.ReadByte();
                            byte3 = myFile.ReadByte();
                            byte4 = myFile.ReadByte();
                        }
                        catch
                        {
                            return retHeader; //couldn't read the bytes, so get out
                        }
                    }
                    else
                        return retHeader; //couldn't find any sync bytes in whole file, so get out
                }
                // If we get here, bytes 1-4 MAY have real sync header, so save it for convenience
                retHeader.byte1 = byte1;
                retHeader.byte2 = byte2;
                retHeader.byte3 = byte3;
                retHeader.byte4 = byte4;

                retHeader.StartFrame = StartData;
                //--------------B------
                int tempbyte = ((byte2 & 24) >> 3);
                retHeader.Version = (MPEGVersion)tempbyte;
                //--------------C-----
                tempbyte = ((byte2 & 6) >> 1);
                retHeader.Layer = (LayerTypes)tempbyte;


                if (retHeader.Layer == LayerTypes.LayerI)
                {
                    retHeader.Samples = 384;
                }
                if (retHeader.Layer == LayerTypes.LayerII)
                {
                    retHeader.Samples = 1152;
                }
                if (retHeader.Layer == LayerTypes.LayerIII)
                {
                    retHeader.Samples = retHeader.Version == MPEGVersion.MPEG1 ? 1152 : 576;
                }

                //--------------D-----
                retHeader.Protected = ((byte2 & 1) == 0);
                //-------------E-------
                tempbyte = (byte3 >> 4);
                if (tempbyte == 15)
                {
                    retHeader.BitRate = -1;
                }
                if ((retHeader.Version == MPEGVersion.MPEG1) && (retHeader.Layer == LayerTypes.LayerI))
                {
                    retHeader.BitRate = bitrate1[tempbyte];
                }
                if ((retHeader.Version == MPEGVersion.MPEG1) && (retHeader.Layer == LayerTypes.LayerII))
                {
                    retHeader.BitRate = bitrate2[tempbyte];
                }
                if ((retHeader.Version == MPEGVersion.MPEG1) && (retHeader.Layer == LayerTypes.LayerIII))
                {
                    retHeader.BitRate = bitrate3[tempbyte];
                }
                if (
                    (
                        (retHeader.Version == MPEGVersion.MPEG2)
                        ||
                        (retHeader.Version == MPEGVersion.MPEG25)
                    )
                    &&
                    (
                        retHeader.Layer == LayerTypes.LayerI
                    )
                    )
                {
                    retHeader.BitRate = bitrate4[tempbyte];
                }
                if (
                    (
                        (retHeader.Version == MPEGVersion.MPEG2)
                        ||
                        (retHeader.Version == MPEGVersion.MPEG25)
                    )
                    &&
                    (
                        (retHeader.Layer == LayerTypes.LayerII)
                        ||
                        (retHeader.Layer == LayerTypes.LayerIII)
                    )
                    )
                {
                    retHeader.BitRate = bitrate5[tempbyte];
                }
                //-----------------F-------------
                tempbyte = ((byte3 & 12) >> 2);
                retHeader.SamplingRate = -1;
                if ((retHeader.Version == MPEGVersion.MPEG1) && (tempbyte == 0))
                    retHeader.SamplingRate = 44100;
                if ((retHeader.Version == MPEGVersion.MPEG1) && (tempbyte == 1))
                    retHeader.SamplingRate = 48000;
                if ((retHeader.Version == MPEGVersion.MPEG1) && (tempbyte == 2))
                    retHeader.SamplingRate = 32000;

                if ((retHeader.Version == MPEGVersion.MPEG2) && (tempbyte == 0))
                    retHeader.SamplingRate = 22050;
                if ((retHeader.Version == MPEGVersion.MPEG2) && (tempbyte == 1))
                    retHeader.SamplingRate = 24000;
                if ((retHeader.Version == MPEGVersion.MPEG2) && (tempbyte == 2))
                    retHeader.SamplingRate = 16000;

                if ((retHeader.Version == MPEGVersion.MPEG25) && (tempbyte == 0))
                    retHeader.SamplingRate = 11025;
                if ((retHeader.Version == MPEGVersion.MPEG25) && (tempbyte == 1))
                    retHeader.SamplingRate = 12000;
                if ((retHeader.Version == MPEGVersion.MPEG25) && (tempbyte == 2))
                    retHeader.SamplingRate = 8000;
                //--------------- G ---------------
                tempbyte = ((byte3 & 2) >> 1);
                retHeader.Padding = tempbyte;
                int PaddingSize = 1 * tempbyte; // tempbyte may be zero
                if (retHeader.Layer == LayerTypes.LayerI)
                    PaddingSize = 4 * tempbyte; // tempbyte may be zero

                //------------------------------

                int FrameSize = 0;
                if ((retHeader.SamplingRate <= 0) || (retHeader.BitRate <= 0))
                {
                    foundGoodHeader = false;
                }

                FrameSize = ((retHeader.Samples / 8 * (retHeader.BitRate * 1000)) / retHeader.SamplingRate) + PaddingSize;

                retHeader.Length = FrameSize;

                bool nextFrameIsValid = false;

                if (retHeader.Length <= 0)
                {
                    retHeader.NextFrame = -1;
                }
                else
                {
                    retHeader.NextFrame = StartData + retHeader.Length;
                    //fail the header if doesn't point to a valid next frame or beyond the end of file
                    if (retHeader.NextFrame < myFile.Length)
                    {
                        nextFrameIsValid = ValidSyncBytes(retHeader.NextFrame);
                        if (!nextFrameIsValid)
                        {
                            foundGoodHeader = false;
                        }
                    }
                }

                //----------------H--------------
                tempbyte = byte3 & 1;
                retHeader.Private = tempbyte;
                //----------------I--------------
                tempbyte = (byte4 >> 6);
                retHeader.ChannelMode = (ChannelTypes)tempbyte;
                //----------------J--------------
                tempbyte = ((byte4 & 48) >> 4);
                retHeader.StereoMode = tempbyte;
                //----------------K--------------
                tempbyte = ((byte4 & 16) >> 3);
                retHeader.Copyrighted = (tempbyte == 1);
                //----------------L--------------
                tempbyte = ((byte4 & 8) >> 2);
                retHeader.Original = (tempbyte == 1);
                //----------------M--------------
                tempbyte = byte4 & 3;
                retHeader.Emphasis = tempbyte;
                //------------------------------
                if (!foundGoodHeader)
                    StartData++;
            }
            return retHeader;
        }


        public bool ReadFirstFrame(long StartPos)
        {
            // This reads the first frame header after StartPos and checks for VBR header
            // and sets this.CurrentFrame.isVBR accordingly
            // myNumFrames has the number of frames
            // Also sets duration in secs.

            if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite)) { return false; }

            try
            {
                this.CurrentFrame = this.GetHeaderInfo(StartPos);
                if (this.CurrentFrame.Version == MPEGVersion.VersionBad)
                    return false;

                // defaults for CBR
                myDuration = (double)myDataLength / (double)(this.CurrentFrame.BitRate * 1000) * 8; // in secs - for CBR, overwritten for VBR
                myNumFrames = Convert.ToInt32(Math.Ceiling((double)myDataLength / (double)this.CurrentFrame.Length)); // for CBR, overwriten for VBR
                if (myNumFrames > 0)
                    myDurationPerFrame = myDuration / myNumFrames;

                long VBROffset;
                if (this.CurrentFrame.ChannelMode == ChannelTypes.SingleChannel)
                {
                    if (this.CurrentFrame.Version == MPEGVersion.MPEG1)
                    {
                        VBROffset = 17 + 4;
                    }
                    else
                    {
                        VBROffset = 9 + 4;
                    }
                }
                else
                {
                    if (this.CurrentFrame.Version == MPEGVersion.MPEG1)
                    {
                        VBROffset = 32 + 4;
                    }
                    else
                    {
                        VBROffset = 17 + 4;
                    }
                }
                this.CurrentFrame.Offset = Convert.ToInt32(VBROffset);
                int byte1, byte2, byte3, byte4;
                try
                {
                    myFile.Position = StartPos + VBROffset;
                    byte1 = myFile.ReadByte();
                    byte2 = myFile.ReadByte();
                    byte3 = myFile.ReadByte();
                    byte4 = myFile.ReadByte();
                }
                catch
                {
                    return false;
                }

                this.CurrentFrame.isVBR = false;

                if (
                    ((byte1 == 88) && (byte2 == 105) && (byte3 == 110) && (byte4 == 103)) // "Xing"
                    ||
                    ((byte1 == 73) && (byte2 == 110) && (byte3 == 102) && (byte4 == 111)) // "Info"
                    )
                {
                    this.CurrentFrame.isVBR = true;
                    myIsVBR = true;
                    myHasXing = true;
                    //now read number of frames and bytes
                    int byte0;
                    try
                    {
                        myFile.Position = StartPos + VBROffset + 7;
                        byte0 = myFile.ReadByte();
                    }
                    catch
                    {
                        byte0 = 0;
                    }

                    if ((byte0 & 1) == 1) //frames field is valid
                    {
                        try
                        {
                            byte1 = myFile.ReadByte();
                            byte2 = myFile.ReadByte();
                            byte3 = myFile.ReadByte();
                            byte4 = myFile.ReadByte();
                            myNumFrames = byte1 * 255 * 255 * 255 + byte2 * 255 * 255 + byte3 * 255 + byte4;
                            myDuration = (double)myNumFrames * (double)this.CurrentFrame.Samples / (double)this.CurrentFrame.SamplingRate;
                        }
                        catch
                        {
                            // if frames field is invalid, then can't determine duration.
                        }
                    }
                    else
                    {
                        // if frames field is invalid, then can't determine duration.
                    }
                    return true;
                }
                else //test for VBRI
                {
                    VBROffset = 32 + 4;
                    try
                    {
                        myFile.Position = StartPos + VBROffset;
                        byte1 = myFile.ReadByte();
                        byte2 = myFile.ReadByte();
                        byte3 = myFile.ReadByte();
                        byte4 = myFile.ReadByte();
                    }
                    catch
                    {
                        // do nowt, should sort itself out on following IF
                    }

                    if ((byte1 == 86) && (byte2 == 66) && (byte3 == 82) && (byte4 == 73)) // "VBRI"
                    {
                        this.CurrentFrame.isVBR = true;
                        myIsVBR = true;
                        myHasXing = false;

                        try
                        {
                            myFile.Position = StartPos + VBROffset + 14;
                            byte1 = myFile.ReadByte();
                            byte2 = myFile.ReadByte();
                            byte3 = myFile.ReadByte();
                            byte4 = myFile.ReadByte();
                            myNumFrames = byte1 * 255 * 255 * 255 + byte2 * 255 * 255 + byte3 * 255 + byte4;
                            myDuration = (double)myNumFrames * (double)this.CurrentFrame.Samples / (double)this.CurrentFrame.SamplingRate;
                            return true;
                        }
                        catch
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }

            }
            catch
            {
                return false; //failed to read
            }
        }


        public bool ReadNextFrame(long StartPos)
        {
            if ((myAccess != AccessType.Reading) && (myAccess != AccessType.ReadWrite)) { return false; }

            try
            {
                this.CurrentFrame = this.GetHeaderInfo(StartPos);
                if (this.CurrentFrame.Version == MPEGVersion.VersionBad)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }


        public long NumFrames
        {
            get
            {
                return myNumFrames;
            }
        }

        public double DurationInSecs //in seconds!
        {
            get
            {
                return myDuration;
            }
        }

        public double DurationPerFrame
        {
            get
            {
                return myDurationPerFrame;
            }
        }

        public int FramesPerSecond
        {
            get
            {
                if (myDuration > 0)
                {
                    double temp = (double)myNumFrames / myDuration;
                    return (int)temp;
                }
                else
                    return 0;
            }
        }

        public bool IsVBR
        {
            get
            {
                return myIsVBR;
            }
        }

        public long DataLength
        {
            get
            {
                return myDataLength;
            }
        }

        public string Encoder
        {
            get
            {
                return myEncoder;
            }
            set
            {
                myEncoder = value;
            }
        }

        public string Comment
        {
            get
            {
                return myComment;
            }
            set
            {
                myComment = value;
            }
        }

        public string Year
        {
            get
            {
                return myYear;
            }
            set
            {
                myYear = value;
            }
        }

        public long ID2Size
        {
            get
            {
                return myID2Size;
            }
        }

        private static int SynchsafeToInt(int synchsafe)
        {
            return (synchsafe & 0x7f) |
            (synchsafe & 0x7f00) >> 1 |
            (synchsafe & 0x7f0000) >> 2 |
            (synchsafe & 0x7f000000) >> 3;
        }

    }
}
