using System;
using System.Collections.Generic;
using System.Text;

namespace DomoSolarInterface.Data
{
	class DeltaSolSample
	{
		public DeltaSolSample(ResolVBus.DeltaSolData data)
		{
			this.temp1 = data.TemperatureSensor1;
			this.temp2 = data.TemperatureSensor2;
			this.temp3 = data.TemperatureSensor3;
			this.pump = data.Pump;
		}

		public DeltaSolSample(IEnumerable<DeltaSolSample> samples)
		{
			int count = 0;

			foreach (DeltaSolSample sample in samples)
			{
				this.temp1 += sample.temp1;
				this.temp2 += sample.temp2;
				this.temp3 += sample.temp3;
				this.pump += sample.pump;

				count++;
			}

			if (count > 1)
			{
				this.temp1 /= count;
				this.temp2 /= count;
				this.temp3 /= count;
				this.pump /= count;
			}
		}

		public decimal TemperatureSensor1
		{
			get
			{
				return this.temp1;
			}
		}

		public decimal TemperatureSensor2
		{
			get
			{
				return this.temp2;
			}
		}

		public decimal TemperatureSensor3
		{
			get
			{
				return this.temp3;
			}
		}

		public decimal Pump
		{
			get
			{
				return this.pump;
			}
		}

		public override string ToString()
		{
			return string.Format ("{0,5:F1}\t{1,5:F1}\t{2,5:F1}\t{3,3}", this.temp1, this.temp2, this.temp3, (int) this.pump);
		}

		private decimal temp1;
		private decimal temp2;
		private decimal temp3;
		private decimal pump;
	}
}
