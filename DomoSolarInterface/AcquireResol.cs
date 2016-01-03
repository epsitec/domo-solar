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

        public void Acquire()
        {
            try
            {
                ResolVBus.Frame[] frames = this.port.ReceiveFrames();

                if ((frames != null) &&
                    (frames.Length > 0))
                {
                    ResolVBus.DeltaSolData data = new ResolVBus.DeltaSolData(frames);
                    this.Publish(data);
                }
            }
            catch
            {
                this.deltaSolData = null;
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
