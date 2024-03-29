using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

#if !MONO
namespace Server
{
    class WindowsService : ServiceBase
    {
	    private Thread server;
	    //private static Thread restart;
        /// <summary>

        /// Public Constructor for WindowsService.

        /// - Put all of your Initialization code here.

        /// </summary>

        public WindowsService()
        {
            this.ServiceName = "RunUO Service";
            this.EventLog.Log = "Application";
            
            // These Flags set whether or not to handle that specific

            //  type of event. Set to true if you need it, false otherwise.

            this.CanHandlePowerEvent = false;
            this.CanHandleSessionChangeEvent = false;
            this.CanPauseAndContinue = false;
            this.CanShutdown = false;
            this.CanStop = true;
        }

        /// <summary>

        /// The Main Thread: This is where your Service is Run.

        /// </summary>

        /*static void Main()
        {
            ServiceBase.Run(new WindowsService());
        }*/

        /// <summary>

        /// Dispose of objects that need it here.

        /// </summary>

        /// <param name="disposing">Whether

        ///    or not disposing is going on.</param>

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>

        /// OnStart(): Put startup code here

        ///  - Start threads, get inital data, etc.

        /// </summary>

        /// <param name="args"></param>

        protected override void OnStart(string[] args)
        {
		//RequestAdditionalTime(120000);
		server = new Thread(new ThreadStart(Server.Core.StartService));
		server.Start();
		//Server.Core.Main(args2);
		//if(Server.Core.Process != null && !Server.Core.Process.HasExited)
		//	Stop();
            //base.OnStart(args);
        }
	
	/*private static void tRestartServer()
	{	
		//server.Abort();
		server.Join();
		Thread.Sleep(5000);
		server = new Thread(new ThreadStart(Server.Core.StartService));
		server.Start();
	}
	
	public static void Restart()
	{
		restart = new Thread(new ThreadStart(tRestartServer));
		restart.Start();
	}*/
        /// <summary>

        /// OnStop(): Put your stop code here

        /// - Stop threads, set final data, etc.

        /// </summary>

        protected override void OnStop()
        {
		//if(Server.Core.Process != null && !Server.Core.Process.HasExited)
		if(server.IsAlive)
		{
			RequestAdditionalTime(15000);
			Server.World.Broadcast(0x35, true, "RunUO Service is stopping in 5 seconds.");
			Thread.Sleep(5000);
			Server.World.Save(true);
			server.Abort();
			server.Join();
		}
            //base.OnStop();
        }

        /// <summary>

        /// OnPause: Put your pause code here

        /// - Pause working threads, etc.

        /// </summary>

        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>

        /// OnContinue(): Put your continue code here

        /// - Un-pause working threads, etc.

        /// </summary>

        protected override void OnContinue()
        {
            base.OnContinue();
        }

        /// <summary>

        /// OnShutdown(): Called when the System is shutting down

        /// - Put code here when you need special handling

        ///   of code that deals with a system shutdown, such

        ///   as saving special data before shutdown.

        /// </summary>

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        /// <summary>

        /// OnCustomCommand(): If you need to send a command to your

        ///   service without the need for Remoting or Sockets, use

        ///   this method to do custom methods.

        /// </summary>

        /// <param name="command">Arbitrary Integer between 128 & 256</param>

        protected override void OnCustomCommand(int command)
        {
            //  A custom command can be sent to a service by using this method:

            //#  int command = 128; //Some Arbitrary number between 128 & 256

            //#  ServiceController sc = new ServiceController("NameOfService");

            //#  sc.ExecuteCommand(command);


            base.OnCustomCommand(command);
        }

        /// <summary>

        /// OnPowerEvent(): Useful for detecting power status changes,

        ///   such as going into Suspend mode or Low Battery for laptops.

        /// </summary>

        /// <param name="powerStatus">The Power Broadcast Status

        /// (BatteryLow, Suspend, etc.)</param>

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        /// <summary>

        /// OnSessionChange(): To handle a change event

        ///   from a Terminal Server session.

        ///   Useful if you need to determine

        ///   when a user logs in remotely or logs off,

        ///   or when someone logs into the console.

        /// </summary>

        /// <param name="changeDescription">The Session Change

        /// Event that occured.</param>

        protected override void OnSessionChange(
                  SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }
    }
}
#endif