using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace MarkAble2
{
	public class OptionClass
	{

		[System.Xml.Serialization.XmlElement]
		public string RegularPrefix = "Part";

		[System.Xml.Serialization.XmlElement]
		public string DefaultGenre = "Audiobook";

		[System.Xml.Serialization.XmlElement]
		public string SeparateTemplate = "%S";

		[System.Xml.Serialization.XmlElement]
		public string AudioCDTemplate = "Disc %D - %S";

		[System.Xml.Serialization.XmlElement]
		public string MergeFolder;

		[System.Xml.Serialization.XmlElement]
		public string RipFolder;

		[System.Xml.Serialization.XmlElement]
		public string AudiobooksFolder;

		[System.Xml.Serialization.XmlElement]
		public string CDDrive = @"D:\";

		[System.Xml.Serialization.XmlElement]
		public Global.EncodeToTypes Encoder = Global.EncodeToTypes.AAC;

		[System.Xml.Serialization.XmlElement]
		public string DefaultImage;

		[System.Xml.Serialization.XmlElement]
		public bool DeleteRippedFiles = true;

		[System.Xml.Serialization.XmlElement]
		public bool DeleteMergedFiles = true;

		[System.Xml.Serialization.XmlElement]
		public decimal DefaultInterval = 15M;

		[System.Xml.Serialization.XmlElement]
		public decimal MaxHours = 13.5M;

		[System.Xml.Serialization.XmlElement]
		public decimal MaxSize = 800M; //MB

		[System.Xml.Serialization.XmlElement]
		public bool ChangePrefs = false;

		[System.Xml.Serialization.XmlElement]
		public int BitRate = 96;

		[System.Xml.Serialization.XmlElement]
		public string LastSourceFolder;

		[System.Xml.Serialization.XmlElement]
		public Global.ChapterTypes ChapterType = Global.ChapterTypes.None;

		[System.Xml.Serialization.XmlElement]
		public int TrackSpacing = 1;

		[System.Xml.Serialization.XmlElement]
		public bool CheckForNewVersion = true;

		[System.Xml.Serialization.XmlElement]
		public bool DebugMode = false;

		[System.Xml.Serialization.XmlElement]
		public bool EjectDiscs = true;

		[System.Xml.Serialization.XmlElement]
		public bool CreateFoldersForAuthorAndBook = true;

		[System.Xml.Serialization.XmlElement]
		public bool MuteSoundWhileRipping = false;

		[System.Xml.Serialization.XmlElement]
		public bool PlaySoundOnDiscComplete = false;

		[System.Xml.Serialization.XmlElement]
		public string DiscCompleteSound = "";

		[System.Xml.Serialization.XmlElement]
		public bool AllowTrackSelection = false;

		[System.Xml.Serialization.XmlElement]
		public bool AddToITunesLibrary = true;

        [System.Xml.Serialization.XmlElement]
        public string BackgroundColorHTML = "#F5DEB3"; //Wheat

        [System.Xml.Serialization.XmlElement]
        public int MaxImageSize = 4096; 


        protected string myFileName = "";

		public OptionClass()
		{
		}

		[System.Xml.Serialization.XmlIgnore]
		public Color BackgroundColor
		{
			get { return ColorTranslator.FromHtml(BackgroundColorHTML); }
			set { BackgroundColorHTML = ColorTranslator.ToHtml(value); }
		}

		private bool Save(string filename)
		{
			if (!Directory.Exists(Global.GetPath(filename)))
			{
				try
				{
					Directory.CreateDirectory(Global.GetPath(filename));
				}
				catch
				{
					return false;
				}
			}
			return Global.ToXmlFile(filename, this, false);
		}

		public bool Save()
		{
			return Save(myFileName);
		}

		public static OptionClass Load(string filename)
		{
			OptionClass opts;

			if (File.Exists(filename))
			{
				opts = (OptionClass) Global.FromXmlFile(filename, typeof (OptionClass)) ?? new OptionClass();
			}
			else
			{
				opts = new OptionClass();
			}
			opts.myFileName = filename;
            if (opts.MaxImageSize > 4096) { opts.MaxImageSize = 4096; }
			return opts;
		}


		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("AudiobooksFolder: {0}\r\n", AudiobooksFolder);
			sb.AppendFormat("AudioCDTemplate: {0}\r\n", AudioCDTemplate);
			sb.AppendFormat("BitRate: {0}\r\n", BitRate);
			sb.AppendFormat("CDDrive: {0}\r\n", CDDrive);
			sb.AppendFormat("ChangePrefs: {0}\r\n", ChangePrefs);
			sb.AppendFormat("ChapterType: {0}\r\n", ChapterType);
			sb.AppendFormat("DefaultGenre: {0}\r\n", DefaultGenre);
			sb.AppendFormat("DefaultImage: {0}\r\n", DefaultImage);
			sb.AppendFormat("DefaultInterval: {0}\r\n", DefaultInterval);
			sb.AppendFormat("DeleteMergedFiles: {0}\r\n", DeleteMergedFiles);
			sb.AppendFormat("DeleteRippedFiles: {0}\r\n", DeleteRippedFiles);
			sb.AppendFormat("Encoder: {0}\r\n", Encoder);
			sb.AppendFormat("LastSourceFolder: {0}\r\n", LastSourceFolder);
			sb.AppendFormat("MaxHours: {0}\r\n", MaxHours);
			sb.AppendFormat("MaxSize: {0}\r\n", MaxSize);
			sb.AppendFormat("MergeFolder: {0}\r\n", MergeFolder);
			sb.AppendFormat("RegularPrefix: {0}\r\n", RegularPrefix);
			sb.AppendFormat("RipFolder: {0}\r\n", RipFolder);
			sb.AppendFormat("SeparateTemplate: {0}\r\n", SeparateTemplate);
			sb.AppendFormat("TrackSpacing: {0}\r\n", TrackSpacing);
			sb.AppendFormat("CheckForNewVersion: {0}\r\n", CheckForNewVersion);
			sb.AppendFormat("DebugMode: {0}\r\n", DebugMode);
			sb.AppendFormat("EjectDiscs: {0}\r\n", EjectDiscs);
			sb.AppendFormat("CreateFoldersForAuthorAndBook: {0}\r\n", CreateFoldersForAuthorAndBook);
			sb.AppendFormat("MuteSoundWhileRipping: {0}\r\n", MuteSoundWhileRipping);
			sb.AppendFormat("PlaySoundOnDiscComplete: {0}\r\n", PlaySoundOnDiscComplete);
			sb.AppendFormat("DiscCompleteSound: {0}\r\n", DiscCompleteSound);
			sb.AppendFormat("AllowTrackSelection: {0}\r\n", AllowTrackSelection);
			sb.AppendFormat("LoadIntoItunes: {0}\r\n", AddToITunesLibrary);
			sb.AppendFormat("BackgroundColor: {0}\r\n", BackgroundColorHTML);
			return sb.ToString();
		}


	}
}