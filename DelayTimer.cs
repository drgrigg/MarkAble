using System;
using System.Threading;
using System.Timers;

namespace MarkAble2
{
	/// <summary>
	/// Summary description for DelayTimer.
	/// </summary>
	public class DelayTimer
	{
		private System.Timers.Timer aTimer;

		public DelayTimer()
		{
			aTimer = new System.Timers.Timer();
			aTimer.Elapsed += new ElapsedEventHandler(OnTimer);
		}

		~DelayTimer()
		{
			aTimer.Enabled = false;
			aTimer = null;
		}

		public DelayTimer(int Secs)
		{
			aTimer = new System.Timers.Timer();
			aTimer.Elapsed += new ElapsedEventHandler(OnTimer);
			aTimer.Interval = Secs * 1000;
			aTimer.Enabled = true;
		}

		public void Start(int Secs)
		{
			aTimer.Interval = Secs * 100;
			aTimer.Enabled = true;
		}

		public void OnTimer(Object source, ElapsedEventArgs e)
		{
			aTimer.Enabled = false;			
		}

		public void Stop()
		{
			aTimer.Enabled = false;
		}

		public bool Ticking()
		{
			return aTimer.Enabled;
		}
	}



}
