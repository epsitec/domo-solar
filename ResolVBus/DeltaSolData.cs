using System;
using System.Collections.Generic;
using System.Text;

namespace ResolVBus
{
	public class DeltaSolData
	{
		public DeltaSolData(Frame[] frames)
		{
			this.sensor1 = DeltaSolData.GetTemperature (frames, 0, 0);
			this.sensor2 = DeltaSolData.GetTemperature (frames, 0, 2);
			this.sensor3 = DeltaSolData.GetTemperature (frames, 1, 0);
			this.pump = frames[1][2];
			this.totalPumpTime = (ushort) (frames[3][0] | (frames[3][1] << 8));
		}

		public decimal TemperatureSensor1
		{
			get
			{
				return this.sensor1;
			}
		}

		public decimal TemperatureSensor2
		{
			get
			{
				return this.sensor2;
			}
		}

		public decimal TemperatureSensor3
		{
			get
			{
				return this.sensor3;
			}
		}

		public int Pump
		{
			get
			{
				return this.pump;
			}
		}

		public int TotalPumpTime
		{
			get
			{
				return this.totalPumpTime;
			}
		}

		private static decimal GetTemperature(Frame[] frames, int index, int offset)
		{
			byte low  = frames[index][offset+0];
			byte high = frames[index][offset+1];
			short num = (short) (low | (high<<8));
			return num * 0.1M;
		}


		private decimal sensor1;
		private decimal sensor2;
		private decimal sensor3;
		private int pump;
		private int totalPumpTime;
	}
}
