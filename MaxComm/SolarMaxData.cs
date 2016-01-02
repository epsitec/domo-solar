using System;
using System.Collections.Generic;
using System.Text;

namespace MaxComm
{
	public class SolarMaxData
	{
		public SolarMaxData(int id, string reply)
		{
			this.id = id;
			this.Initialize (reply);
		}

		public int Id
		{
			get
			{
				return this.id;
			}
		}
		
		public decimal PowerAC
		{
			get
			{
				return this.powerAC;
			}
		}

		public int PowerRelative
		{
			get
			{
				return this.powerRelative;
			}
		}

		public int TotalTime
		{
			get
			{
				return this.totalTime;
			}
		}

		public int TotalEnergy
		{
			get
			{
				return this.totalEnergy;
			}
		}

		public decimal EnergyDay
		{
			get
			{
				return this.energyDay;
			}
		}

		public int EnergyMonth
		{
			get
			{
				return this.energyMonth;
			}
		}

		public int EnergyYear
		{
			get
			{
				return this.energyYear;
			}
		}

		public decimal LinePeriod
		{
			get
			{
				return this.linePeriod;
			}
		}

		public decimal VoltageAC
		{
			get
			{
				return this.voltageAC;
			}
		}

		public decimal VoltageDC
		{
			get
			{
				return this.voltageDC;
			}
		}

		public decimal CurrentAC
		{
			get
			{
				return this.currentAC;
			}
		}

		public decimal CurrentDC
		{
			get
			{
				return this.currentDC;
			}
		}

		public int ConverterTemperature
		{
			get
			{
				return this.converterTemperature;
			}
		}
		
		private void Initialize(string reply)
		{
			if (string.IsNullOrEmpty (reply))
			{
				return;
			}
			
			string[] replies = reply.Split (';');

			foreach (string item in replies)
			{
				string[] elems = item.Split ('=');

				string tag = elems[0];
				int value = int.Parse (elems[1], System.Globalization.NumberStyles.HexNumber);
				
				switch (tag)
				{
					case "PAC":
						this.powerAC = value * 0.5M;
						break;

					case "KHR":
						this.totalTime = value;
						break;

					case "KYR":
						this.energyYear = value;
						break;

					case "KMT":
						this.energyMonth = value;
						break;

					case "KDY":
						this.energyDay = value * 0.1M;
						break;

					case "KT0":
						this.totalEnergy = value;
						break;

					case "TNP":
						this.linePeriod = value / 1000M;
						break;

					case "PRL":
						this.powerRelative = value;
						break;

					case "UDC":
						this.voltageDC = value * 0.1M;
						break;

					case "UL1":
						this.voltageAC = value * 0.1M;
						break;

					case "IDC":
						this.currentDC = value * 0.01M;
						break;

					case "IL1":
						this.currentAC = value * 0.01M;
						break;

					case "TKK":
						this.converterTemperature = value;
						break;
				}
			}
		}

		private int id;
		private decimal powerAC;
		private int totalTime;
		private int totalEnergy;
		private int energyYear;
		private int energyMonth;
		private decimal energyDay;
		private decimal linePeriod;
		private int powerRelative;
		private decimal voltageDC;
		private decimal voltageAC;
		private decimal currentDC;
		private decimal currentAC;
		private int converterTemperature;
	}
}
