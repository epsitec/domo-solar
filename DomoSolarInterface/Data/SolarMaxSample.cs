using System;
using System.Collections.Generic;
using System.Text;

namespace DomoSolarInterface.Data
{
	class SolarMaxSample
	{
		public SolarMaxSample(MaxComm.SolarMaxData data)
		{
			this.dailyProduction = data.EnergyDay;
			this.voltageDC = data.VoltageDC;
			this.currentDC = data.CurrentDC;
			this.powerAC = data.PowerAC;
			this.voltageAC = data.VoltageAC;
			this.currentAC = data.CurrentAC;
			this.temperature = data.ConverterTemperature;
			this.period = data.LinePeriod;
		}

		public SolarMaxSample(IEnumerable<SolarMaxSample> samples)
		{
			int count = 0;

			foreach (SolarMaxSample sample in samples)
			{
				this.dailyProduction = sample.dailyProduction;

				this.voltageDC += sample.voltageDC;
				this.currentDC += sample.currentDC;
				this.powerAC += sample.powerAC;
				this.voltageAC += sample.voltageAC;
				this.currentAC += sample.currentAC;
				this.temperature += sample.temperature;
				this.period += sample.period;
				
				count++;
			}

			if (count > 1)
			{
				this.voltageDC /= count;
				this.currentDC /= count;
				this.powerAC /= count;
				this.voltageAC /= count;
				this.currentAC /= count;
				this.temperature /= count;
				this.period /= count;
			}
		}

		public decimal DailyProduction
		{
			get
			{
				return this.dailyProduction;
			}
		}

		public decimal VoltageDC
		{
			get
			{
				return this.voltageDC;
			}
		}

		public decimal CurrentDC
		{
			get
			{
				return this.currentDC;
			}
		}

		public decimal PowerAC
		{
			get
			{
				return this.powerAC;
			}
		}

		public decimal VoltageAC
		{
			get
			{
				return this.voltageAC;
			}
		}

		public decimal CurrentAC
		{
			get
			{
				return this.currentAC;
			}
		}

		public decimal Temperature
		{
			get
			{
				return this.temperature;
			}
		}

		public decimal Period
		{
			get
			{
				return this.period;
			}
		}

		public override string ToString()
		{
			return string.Format ("{0,4:F1}\t{1,5:F1}\t{2,4:F1}\t{3,6:F1}\t{4,5:F1}\t{5,4:F1}\t{6,5:F1}\t{7,6:F3}", this.dailyProduction, this.voltageDC, this.currentDC, this.powerAC, this.voltageAC, this.currentAC, this.temperature, this.period);
		}

		decimal dailyProduction;
		decimal voltageDC;
		decimal currentDC;
		decimal powerAC;
		decimal voltageAC;
		decimal currentAC;
		decimal temperature;
		decimal period;
	}
}
