using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			System.Console.Out.WriteLine ("{0,6:F1}", 12.34M);

			ResolVBus.Port resol = new ResolVBus.Port ("COM3");

			ResolVBus.DeltaSolData deltaSol = new ResolVBus.DeltaSolData (resol.ReceiveFrames ());

			System.Console.Out.WriteLine ("DeltaSol: {5} {0}°C, {1}°C, {2}°C, {3}%, {4}h", deltaSol.TemperatureSensor1, deltaSol.TemperatureSensor2, deltaSol.TemperatureSensor3, deltaSol.Pump, deltaSol.TotalPumpTime, System.DateTime.Now.ToShortTimeString ());

			MaxComm.Port port = new MaxComm.Port ("COM4");

			MaxComm.SolarMaxData solarMaxData1 = new MaxComm.SolarMaxData (1, port.ExchangeData (1, 100, "PAC;PRL;TKK;KHR;KYR;KMT;KDY;KT0;UDC;IDC;UL1;IL1;TNP"));
			MaxComm.SolarMaxData solarMaxData2 = new MaxComm.SolarMaxData (2, port.ExchangeData (2, 100, "PAC;PRL;TKK;KHR;KYR;KMT;KDY;KT0;UDC;IDC;UL1;IL1;TNP"));

			System.Console.Out.WriteLine ("SolarMax #1:");
			Program.DumpSolarMax (solarMaxData1);
			System.Console.Out.WriteLine ("SolarMax #2:");
			Program.DumpSolarMax (solarMaxData2);

#if false
			System.Console.Out.WriteLine ("SolarMax #1:");
			Program.DumpResults (port.ExchangeData (1, 100, "TYP;PAC;PRL;TKK;KHR;KYR;KMT;KDY;KT0;UDC;IDC;UL1;IL1;TNP"));
			System.Console.Out.WriteLine ();

			System.Console.Out.WriteLine ("SolarMax #2:");
			Program.DumpResults (port.ExchangeData (2, 100, "TYP;PAC;PRL;TKK;KHR;KYR;KMT;KDY;KT0;UDC;IDC;UL1;IL1;TNP"));
			System.Console.Out.WriteLine ();
#endif
			
			System.Console.In.ReadLine ();
		}

		private static void DumpSolarMax(MaxComm.SolarMaxData data)
		{
			System.Console.Out.WriteLine ("Power: {0} W, {1} %; {2}°C", data.PowerAC, data.PowerRelative, data.ConverterTemperature);
			System.Console.Out.WriteLine ("AC: {0} V, {1} A", data.VoltageAC, data.CurrentAC);
			System.Console.Out.WriteLine ("DC: {0} V, {1} A", data.VoltageDC, data.CurrentDC);
			System.Console.Out.WriteLine ("Energy: {0} kWh/day, {1} kWh/month, {2} kWh/year, {3} h, total {4} kWh", data.EnergyDay, data.EnergyMonth, data.EnergyYear, data.TotalTime, data.TotalEnergy);
		}

		private static void DumpResults(string reply)
		{
			if (string.IsNullOrEmpty (reply))
			{
				System.Console.Out.WriteLine ("no data");
				return;
			}
			
			string[] replies = reply.Split (';');

			foreach (string item in replies)
			{
				string[] elems = item.Split ('=');

				string tag = elems[0];
				int value = int.Parse (elems[1], System.Globalization.NumberStyles.HexNumber);
				decimal realValue = 0;
				string friendlyTag = null;
				string friendlyUnit = null;

				switch (tag)
				{
					case "PAC":
						friendlyTag = "Power";
						friendlyUnit = "W";
						realValue = value * 0.5M;
						break;

					case "KHR":
						friendlyTag = "Time";
						friendlyUnit = "h";
						realValue = value;
						break;

					case "KYR":
						friendlyTag = "Energy/year";
						friendlyUnit = "kWh";
						realValue = value;
						break;

					case "KMT":
						friendlyTag = "Energy/month";
						friendlyUnit = "kWh";
						realValue = value;
						break;

					case "KDY":
						friendlyTag = "Energy/day";
						friendlyUnit = "kWh";
						realValue = value * 0.1M;
						break;

					case "KT0":
						friendlyTag = "Energy/total";
						friendlyUnit = "kWh";
						realValue = value;
						break;

					case "TNP":
						friendlyTag = "Line period";
						friendlyUnit = "us";
						realValue = value;
						break;

					case "PRL":
						friendlyTag = "Relative power";
						friendlyUnit = "%";
						realValue = value;
						break;

					case "UDC":
						friendlyTag = "Voltage DC";
						friendlyUnit = "V";
						realValue = value * 0.1M;
						break;

					case "UL1":
						friendlyTag = "Voltage phase 1";
						friendlyUnit = "V";
						realValue = value * 0.1M;
						break;

					case "UL2":
						friendlyTag = "Voltage phase 2";
						friendlyUnit = "V";
						realValue = value * 0.1M;
						break;

					case "UL3":
						friendlyTag = "Voltage phase 3";
						friendlyUnit = "V";
						realValue = value * 0.1M;
						break;

					case "IDC":
						friendlyTag = "Current DC";
						friendlyUnit = "A";
						realValue = value * 0.01M;
						break;

					case "IL1":
						friendlyTag = "Current phase 1";
						friendlyUnit = "A";
						realValue = value * 0.01M;
						break;

					case "IL2":
						friendlyTag = "Current phase 2";
						friendlyUnit = "A";
						realValue = value * 0.01M;
						break;

					case "IL3":
						friendlyTag = "Current phase 3";
						friendlyUnit = "A";
						realValue = value * 0.01M;
						break;

					case "TYP":
						friendlyTag = "SolarMax type";
						friendlyUnit = "";
						realValue = value;
						break;
					
					case "TKK":
						friendlyTag = "Temperature";
						friendlyUnit = "°C";
						realValue = value;
						break;

					default:
						friendlyTag = "?? " + tag;
						friendlyUnit = "";
						realValue = value;
						break;
				}

				System.Console.Out.WriteLine ("{0} {1} {2}", friendlyTag, realValue, friendlyUnit);
			}
		}
	}
}
