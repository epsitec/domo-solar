namespace DomoSolarInterface
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
            var service = new AcquisitionService ();

            service.Start ();

            try
            {
                while (true)
                {
                    service.Acquire();
                    System.Threading.Thread.Sleep(5 * 1000);
                }
            }
            catch
            {

            }

            service.Stop ();
		}
	}
}