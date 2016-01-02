using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace ResolVBus
{
	public class Port
	{
		public Port(string name)
		{
			this.port = new SerialPort (name, 9600, Parity.None, 8, StopBits.One);
			this.port.ReadTimeout = 10*1000;
			this.port.WriteTimeout = 1000;
		}

		public Frame[] ReceiveFrames()
		{
			byte[] data = this.ReceiveRawData ();

			if (data == null)
			{
				data = this.ReceiveRawData ();
			}
			if (data == null)
			{
				throw new System.Exception ("Communication error, no data");
			}
			
			int numFrames = data[7];
			Frame[] frames = new Frame[numFrames];

			for (int i = 0; i < numFrames; i++)
			{
				frames[i] = new Frame (data, 9+i*6, 6);
			}
			
			return frames;
		}

		public byte[] ReceiveRawData()
		{
			List<byte> buffer = new List<byte> ();
			
			this.port.Open ();

			try
			{
				bool record = false;

				while (true)
				{
					try
					{
						byte c = (byte) this.port.ReadByte ();

						if (c == 0xAA)
						{
							if (record)
							{
								return null;
							}

							record = true;
						}
						else
						{
							buffer.Add (c);

							if (buffer.Count > 8)
							{
								int numFrames = buffer[7];

								if (buffer.Count == (9+numFrames*6))
								{
									break;
								}
							}
						}
					}
					catch (System.TimeoutException)
					{
						return null;
					}
				}
			}
			finally
			{
				this.port.Close ();
			}

			return buffer.ToArray ();
		}
		
		private SerialPort port;
	}
}
