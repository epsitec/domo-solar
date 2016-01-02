using System;
using System.Collections.Generic;
using System.Text;

namespace DomoSolarInterface
{
	class AcquireResol
	{
		public AcquireResol(string portName)
		{
			this.port = new ResolVBus.Port (portName);
		}

		public ResolVBus.DeltaSolData DeltaSolData
		{
			get
			{
				return this.deltaSolData;
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
						ResolVBus.Frame[] frames = this.port.ReceiveFrames ();

						if (frames != null)
						{
							ResolVBus.DeltaSolData data = new ResolVBus.DeltaSolData (frames);
							this.Publish (data);
						}
					}
					catch
					{
						this.deltaSolData = null;
					}

					System.Threading.Thread.Sleep (System.Math.Max (Settings.Default.PollInterval, 1) * 1000);
				}
			}
			catch (System.Threading.ThreadInterruptedException)
			{
			}
		}

		private void Publish(ResolVBus.DeltaSolData data)
		{
			this.deltaSolData = data;
		}

		private ResolVBus.Port port;
		private volatile ResolVBus.DeltaSolData deltaSolData;
	}
}
