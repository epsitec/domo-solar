using System;
using System.Collections.Generic;
using System.Text;

namespace DomoSolarInterface
{
	class DataJournal
	{
		public DataJournal(DataLogger logger)
		{
			this.logger = logger;
		}

		public void RunThread()
		{
			int oldDayOfYear = -1;
			int oldTimeOfDay = -1;
			int counter = 0;

			System.IO.FileStream streamSolarMax1 = null;
			System.IO.FileStream streamSolarMax2 = null;
			System.IO.FileStream streamDeltaSol  = null;

			try
			{
				System.Threading.Thread.Sleep (Settings.Default.LogInterval * 1000);

				while (true)
				{
					System.Threading.Thread.Sleep (2*60*1000);
					
					try
					{
						System.DateTime now = System.DateTime.Now;

						int timeOfDay = (int) now.TimeOfDay.TotalMinutes / 10;
						int dayOfYear = now.DayOfYear;

						if (timeOfDay == oldTimeOfDay)
						{
							continue;
						}

						oldTimeOfDay = timeOfDay;

						if (dayOfYear != oldDayOfYear)
						{
							string path = Settings.Default.LogPath;
							string day  = string.Format ("{0:D4}-{1:D3}", now.Year, dayOfYear);
							
							if (streamSolarMax1 != null)
							{
								streamSolarMax1.Close ();
							}
							if (streamSolarMax2 != null)
							{
								streamSolarMax2.Close ();
							}
							if (streamDeltaSol != null)
							{
								streamDeltaSol.Close ();
							}

							streamSolarMax1 = new System.IO.FileStream (System.IO.Path.Combine (path, "sm1-" + day + ".log"), System.IO.FileMode.Append, System.IO.FileAccess.Write);
							streamSolarMax2 = new System.IO.FileStream (System.IO.Path.Combine (path, "sm2-" + day + ".log"), System.IO.FileMode.Append, System.IO.FileAccess.Write);
							streamDeltaSol  = new System.IO.FileStream (System.IO.Path.Combine (path, "dso-" + day + ".log"), System.IO.FileMode.Append, System.IO.FileAccess.Write);

							oldDayOfYear = dayOfYear;
						}

						byte[] data;
						
						data = System.Text.Encoding.ASCII.GetBytes (string.Format ("{0,4}\t{1}\r\n", timeOfDay*10, this.logger.SolarMax1Mean.ToString ()));
						streamSolarMax1.Write (data, 0, data.Length);

						data = System.Text.Encoding.ASCII.GetBytes (string.Format ("{0,4}\t{1}\r\n", timeOfDay*10, this.logger.SolarMax2Mean.ToString ()));
						streamSolarMax2.Write (data, 0, data.Length);

						data = System.Text.Encoding.ASCII.GetBytes (string.Format ("{0,4}\t{1}\r\n", timeOfDay*10, this.logger.DeltaSolMean.ToString ()));
						streamDeltaSol.Write (data, 0, data.Length);

						counter++;

						if (counter > 4)
						{
							counter = 0;
							
							streamSolarMax1.Flush ();
							streamSolarMax2.Flush ();
							streamDeltaSol.Flush ();
						}
					}
					catch
					{
					}

					System.Threading.Thread.Sleep (System.Math.Max (Settings.Default.LogInterval, 10) * 1000);
				}
			}
			catch (System.Threading.ThreadInterruptedException)
			{
			}
			finally
			{
				if (streamSolarMax1 != null)
				{
					streamSolarMax1.Close ();
				}
				if (streamSolarMax2 != null)
				{
					streamSolarMax2.Close ();
				}
				if (streamDeltaSol != null)
				{
					streamDeltaSol.Close ();
				}
			}
		}

		private DataLogger logger;
	}
}
