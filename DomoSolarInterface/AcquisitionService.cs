using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace DomoSolarInterface
{
	public partial class AcquisitionService : ServiceBase
	{
		public AcquisitionService()
		{
			this.InitializeComponent ();
			this.resol = new AcquireResol (Settings.Default.ResolPort);
			this.maxComm = new AcquireMaxComm (Settings.Default.MaxCommPort);
			this.logger = new DataLogger (this.maxComm, this.resol);
			this.journal = new DataJournal (this.logger);
		}

		protected override void OnStart(string[] args)
		{
			this.resolThread = new System.Threading.Thread (this.resol.RunThread);
			this.maxCommThread = new System.Threading.Thread (this.maxComm.RunThread);
			this.loggerThread =  new System.Threading.Thread (this.logger.RunThread);
			this.journalThread =  new System.Threading.Thread (this.journal.RunThread);

			this.resolThread.Start ();
			this.maxCommThread.Start ();
			this.loggerThread.Start ();
			this.journalThread.Start ();
		}

		protected override void OnStop()
		{
			this.journalThread.Interrupt ();
			this.loggerThread.Interrupt ();
			this.resolThread.Interrupt ();
			this.maxCommThread.Interrupt ();

			this.resolThread.Join ();
			this.maxCommThread.Join ();
			this.loggerThread.Join ();
			this.journalThread.Join ();

			this.resolThread = null;
			this.maxCommThread = null;
			this.loggerThread = null;
			this.journalThread = null;
		}

		protected override void OnShutdown()
		{
			base.OnShutdown ();
			this.Stop ();
		}

		private AcquireResol resol;
		private AcquireMaxComm maxComm;
		private DataLogger logger;
		private DataJournal journal;

		private System.Threading.Thread resolThread;
		private System.Threading.Thread maxCommThread;
		private System.Threading.Thread loggerThread;
		private System.Threading.Thread journalThread;
	}
}
