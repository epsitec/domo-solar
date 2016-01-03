using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace DomoSolarInterface
{
	public class AcquisitionService
	{
		public AcquisitionService()
		{
			this.resol = new AcquireResol (Settings.Default.ResolPort);
			this.maxComm = new AcquireMaxComm (Settings.Default.MaxCommPort);
			this.logger = new DataLogger (this.maxComm, this.resol);
			this.journal = new DataJournal (this.logger);
		}

		public void Start()
		{
			this.loggerThread =  new System.Threading.Thread (this.logger.RunThread);
			this.journalThread =  new System.Threading.Thread (this.journal.RunThread);

			this.loggerThread.Start ();
			this.journalThread.Start ();
		}

        public void Acquire()
        {
            this.resol.Acquire ();
            this.maxComm.Acquire ();
        }

		public void Stop()
		{
			this.journalThread.Interrupt ();
			this.loggerThread.Interrupt ();

			this.loggerThread.Join ();
			this.journalThread.Join ();

			this.loggerThread = null;
			this.journalThread = null;
		}

		private AcquireResol resol;
		private AcquireMaxComm maxComm;
		private DataLogger logger;
		private DataJournal journal;

		private System.Threading.Thread loggerThread;
		private System.Threading.Thread journalThread;
	}
}
