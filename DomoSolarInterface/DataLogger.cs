using System;
using System.Collections.Generic;
using System.Text;

namespace DomoSolarInterface
{
	class DataLogger
	{
		public DataLogger(AcquireMaxComm maxComm, AcquireResol resol)
		{
			this.maxComm = maxComm;
			this.resol = resol;
		}

		
		public Data.SolarMaxSample SolarMax1Mean
		{
			get
			{
				lock (this.exclusion)
				{
					return new Data.SolarMaxSample (this.solarMaxSamples1);
				}
			}
		}

		public Data.SolarMaxSample SolarMax2Mean
		{
			get
			{
				lock (this.exclusion)
				{
					return new Data.SolarMaxSample (this.solarMaxSamples2);
				}
			}
		}

		public Data.DeltaSolSample DeltaSolMean
		{
			get
			{
				lock (this.exclusion)
				{
					return new Data.DeltaSolSample (this.deltaSolSamples);
				}
			}
		}


		public void RunThread()
		{
			try
			{
				System.Threading.Thread.Sleep (Settings.Default.PollInterval * 1000);
				
				while (true)
				{
					try
					{
						MaxComm.SolarMaxData solarMaxData1 = this.maxComm.SolarMaxData1;
						MaxComm.SolarMaxData solarMaxData2 = this.maxComm.SolarMaxData2;
						ResolVBus.DeltaSolData deltaSolData = this.resol.DeltaSolData;

						//	TODO: store resulting data...

						System.Console.WriteLine (System.DateTime.Now.ToShortTimeString ());

						if (deltaSolData != null)
						{
							lock (this.exclusion)
							{
								this.deltaSolSamples.Add (new Data.DeltaSolSample (deltaSolData));

								while (this.deltaSolSamples.Count > Settings.Default.LogSampleCount)
								{
									this.deltaSolSamples.RemoveAt (0);
								}
							}

							System.Console.WriteLine ("DeltaSol: {0}°C, {1}°C, {2}°C, {3}%, {4}h", deltaSolData.TemperatureSensor1, deltaSolData.TemperatureSensor2, deltaSolData.TemperatureSensor3, deltaSolData.Pump, deltaSolData.TotalPumpTime);
						}
						else
						{
							lock (this.exclusion)
							{
								if (this.deltaSolSamples.Count > 0)
								{
									this.deltaSolSamples.RemoveAt (0);
								}
							}
						}

						if (solarMaxData1 != null)
						{
							lock (this.exclusion)
							{
								this.solarMaxSamples1.Add (new Data.SolarMaxSample (solarMaxData1));

								while (this.solarMaxSamples1.Count > Settings.Default.LogSampleCount)
								{
									this.solarMaxSamples1.RemoveAt (0);
								}
							}

							System.Console.WriteLine ("SolarMax{0}: {1}W, DC: {2}V {3}A, AC: {4}V {5}A", solarMaxData1.Id, solarMaxData1.PowerAC, solarMaxData1.VoltageDC, solarMaxData1.CurrentDC, solarMaxData1.VoltageAC, solarMaxData1.CurrentAC);
						}
						else
						{
							lock (this.exclusion)
							{
								if (this.solarMaxSamples1.Count > 0)
								{
									this.solarMaxSamples1.RemoveAt (0);
								}
							}
						}
						
						if (solarMaxData2 != null)
						{
							lock (this.exclusion)
							{
								this.solarMaxSamples2.Add (new Data.SolarMaxSample (solarMaxData2));

								while (this.solarMaxSamples2.Count > Settings.Default.LogSampleCount)
								{
									this.solarMaxSamples2.RemoveAt (0);
								}
							}

                            System.Console.WriteLine ("SolarMax{0}: {1}W, DC: {2}V {3}A, AC: {4}V {5}A", solarMaxData2.Id, solarMaxData2.PowerAC, solarMaxData2.VoltageDC, solarMaxData2.CurrentDC, solarMaxData2.VoltageAC, solarMaxData2.CurrentAC);
						}
						else
						{
							lock (this.exclusion)
							{
								if (this.solarMaxSamples2.Count > 0)
								{
									this.solarMaxSamples2.RemoveAt (0);
								}
							}
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
		}

		AcquireMaxComm maxComm;
		AcquireResol resol;

		object exclusion = new object ();

		List<Data.DeltaSolSample> deltaSolSamples = new List<DomoSolarInterface.Data.DeltaSolSample> ();
		List<Data.SolarMaxSample> solarMaxSamples1 = new List<DomoSolarInterface.Data.SolarMaxSample> ();
		List<Data.SolarMaxSample> solarMaxSamples2 = new List<DomoSolarInterface.Data.SolarMaxSample> ();
	}
}
