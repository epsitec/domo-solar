using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace MaxComm
{
	public class Port
	{
		public Port(string name)
		{
			this.port = new SerialPort (name, 19200, Parity.None, 8, StopBits.One);
			this.port.ReadTimeout = 100;
			this.port.WriteTimeout = 1000;
		}

		public string ExchangeData(int address, int port, string command)
		{
			System.Text.StringBuilder buffer = new StringBuilder ();

			byte[] data = Port.GenerateSequence (address, port, command);
			
			this.port.Open ();
			this.port.Write (data, 0, data.Length);

			bool record = false;
			
			while (true)
			{
				try
				{
					byte c = (byte) this.port.ReadByte ();

					if (c == '}')
					{
						break;
					}
					else if (c == ':')
					{
						record = true;
					}
					else if (record)
					{
						if (c == '|')
						{
							break;
						}
						
						buffer.Append ((char) c);
					}
				}
				catch (System.TimeoutException)
				{
					break;
				}
			}
			
			this.port.Close ();
			
			return buffer.ToString ();
		}

		public static byte[] GenerateSequence(int address, int port, string command)
		{
			byte[] data = new byte[255];
			int index = 0;
			int sizeIndex = 0;

			Port.Append (data, ref index, "{FB;");
			Port.Append (data, ref index, address.ToString ("X2"));
			Port.Append (data, ref index, ";");
			
			sizeIndex = index;
			
			Port.Append (data, ref index, "xx");
			Port.Append (data, ref index, "|");
			Port.Append (data, ref index, port.ToString ("X2"));
			Port.Append (data, ref index, ":");
			Port.Append (data, ref index, command);
			Port.Append (data, ref index, "|");

			int size = index+4+1;
			
			Port.Append (data, ref sizeIndex, size.ToString ("X2"));

			int crc = 0;

			for (int i = 1; i < index; i++)
			{
				crc += data[i];
			}

			crc &= 0xffff;

			Port.Append (data, ref index, crc.ToString ("X4"));
			Port.Append (data, ref index, "}");

			byte[] copy = new byte[size];
			System.Array.Copy (data, 0, copy, 0, size);
			
			return copy;
		}

		private static void Append(byte[] buffer, ref int index, string data)
		{
			foreach (char c in data)
			{
				buffer[index++] = (byte) c;
			}
		}

		private SerialPort port;
	}
}
