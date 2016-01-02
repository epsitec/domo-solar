using System;
using System.Collections.Generic;
using System.Text;

namespace ResolVBus
{
	public class Frame
	{
		public Frame(byte[] raw, int offset, int length)
		{
			byte[] temp = new byte[length];
			System.Array.Copy (raw, offset, temp, 0, length);
			this.Initialize (temp);
		}

		public Frame(byte[] raw)
		{
			this.Initialize (raw);
		}

		private void Initialize(byte[] raw)
		{
			if (raw.Length != 6)
			{
				throw new System.Exception ("Corrupted frame data, wrong length");
			}
			
			this.data = new byte[4];

			this.data[0] = raw[0];
			this.data[1] = raw[1];
			this.data[2] = raw[2];
			this.data[3] = raw[3];

			if ((raw[4] & 0x01) != 0)
			{
				this.data[0] |= 0x80;
			}
			if ((raw[4] & 0x02) != 0)
			{
				this.data[1] |= 0x80;
			}
			if ((raw[4] & 0x04) != 0)
			{
				this.data[2] |= 0x80;
			}
			if ((raw[4] & 0x08) != 0)
			{
				this.data[3] |= 0x80;
			}

			byte crc = (byte) ~(raw[0] + raw[1] + raw[2] + raw[3] + raw[4]);

			if ((crc & 0x7f) != raw[5])
			{
				throw new System.Exception ("Corrupted frame data, checksum error");
			}
		}

		public byte this[int index]
		{
			get
			{
				return this.data[index];
			}
		}

		private byte[] data;
	}
}
