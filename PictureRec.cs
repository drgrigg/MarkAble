using System.Drawing;

namespace MarkAble2
{
    public class PictureRec
    {
        public enum PictureTypes
        {
            None = 0x00,
            FileIcon = 0x01,
            OtherFileIcon = 0x02,
            FrontCover = 0x03,
            BackCover = 0x04,
            Leaflet = 0x05,
            Media = 0x06,
            LeadArtist = 0x07,
            Performer = 0x08,
            Conductor = 0x09,
            Band = 0x0A,
            Composer = 0x0B,
            Lyricist = 0x0C,
            RecordingLocation = 0x0D,
            RecordingShot = 0x0E,
            PerformanceShot = 0x0F,
            VideoCapture = 0x10,
            Fish = 0x11,
            Illustration = 0x12,
            BandLogo = 0x13,
            StudioLogo = 0x14
        }

        private Image myImage = null;

        public PictureTypes PictureType;
        public string MIMEtype = "\\image";
        public string Description = "";

        public Image Picture
        {
            get { return myImage; }
            set { myImage = value; }
        }
    }
}
