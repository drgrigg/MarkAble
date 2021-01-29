using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MarkAble2
{
	/// <summary>
	/// Summary description for MCIClass.
	/// </summary>
	public class MCIClass
	{


		#region Interop Methods

		[DllImport( "winmm.dll", EntryPoint="mciSendStringA", CharSet=CharSet.Ansi )]
		protected static extern int MciSendString( string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback );

		#endregion


		private static StringBuilder ReturnCode = new StringBuilder();
		private static string CommandLine = "";

		public MCIClass()
		{
		}


		private static void OpenCD()
		{
			CommandLine = @"open cdaudio alias cd";
			try
			{
				MciSendString( CommandLine, ReturnCode, ReturnCode.Length, new IntPtr(0) );
			}
			catch
			{
				//do nowt
			}
		}

		public static void CloseCD()
		{
			CommandLine = "close cd";
			try
			{
				MciSendString( CommandLine, ReturnCode, ReturnCode.Length, new IntPtr(0) );
			}
			catch
			{
								//do nowt
			}
		}

		public static void OpenDrive(string DriveStr)
		{
			CommandLine = "open " + DriveStr + " type cdaudio alias cd";
			MciSendString( CommandLine, ReturnCode, ReturnCode.Length, new IntPtr(0) );
		}

		public static bool IsDoorClosed()
		{
		return false;
		}

		public static void OpenDoor( string DriveStr, bool OpenIt )
		{
			OpenDrive(DriveStr);

			CommandLine = "set cd door ";
			string ActionLine = "";

			if (OpenIt) 
			{
				ActionLine = "open";
			}
			else
			{
				ActionLine = "closed";
			}

			CommandLine = CommandLine + " " + ActionLine;

			try
			{
				MciSendString( CommandLine, ReturnCode, ReturnCode.Length, new IntPtr(0) );
			}
			catch
			{
				//do nowt
			}
			CloseCD();
		}

	}
}
