using System;
using System.Collections.Generic;
using System.Text;

namespace DomoSolarInterface
{
	class AcquireMaxComm
	{
		public AcquireMaxComm(string portName)
		{
			this.port = new MaxComm.Port (portName);
		}

		public MaxComm.SolarMaxData SolarMaxData1
		{
			get
			{
				return this.solarMaxData1;
			}
		}

		public MaxComm.SolarMaxData SolarMaxData2
		{
			get
			{
				return this.solarMaxData2;
			}
		}

		public void RunThread()
		{
			try
			{
				while (true)
				{
					try
					{
						MaxComm.SolarMaxData solarMaxData1 = new MaxComm.SolarMaxData (1, this.port.ExchangeData (1, 100, "PAC;PRL;TKK;KHR;KYR;KMT;KDY;KT0;UDC;IDC;UL1;IL1;TNP"));
						MaxComm.SolarMaxData solarMaxData2 = new MaxComm.SolarMaxData (2, this.port.ExchangeData (2, 100, "PAC;PRL;TKK;KHR;KYR;KMT;KDY;KT0;UDC;IDC;UL1;IL1;TNP"));

						this.Publish (solarMaxData1);
						this.Publish (solarMaxData2);
					}
					catch
					{
						this.solarMaxData1 = null;
						this.solarMaxData2 = null;
					}

					System.Threading.Thread.Sleep (System.Math.Max (Settings.Default.PollInterval, 1) * 1000);
				}
			}
			catch (System.Threading.ThreadInterruptedException)
			{
			}
		}

		private void Publish(MaxComm.SolarMaxData data)
		{
			switch (data.Id)
			{
				case 1:
					this.solarMaxData1 = data.TotalEnergy <= 0 ? null : data;
					break;
				case 2:
					this.solarMaxData2 = data.TotalEnergy <= 0 ? null : data;
					break;
			}
		}

		private MaxComm.Port port;
		private volatile MaxComm.SolarMaxData solarMaxData1;
		private volatile MaxComm.SolarMaxData solarMaxData2;
	}
}
