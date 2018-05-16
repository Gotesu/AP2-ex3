using System;
using System.Linq;
using ImageService.Logging;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Model;
using System.Configuration;
using ImageService.Infrastructure.Enums;

namespace ImageService.Server
{
	/// <summary>
	/// ImageServer is the server that sends handlers to handle directories given in appconfig (for the time being)
	/// the server communicates with the handlers via eventHandlers
	/// </summary>
	public class DirectoryManager
	{
		#region Members
		private IImageController m_controller;
		private ILoggingService m_logging;
		#endregion

		#region Properties
		public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
		#endregion

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="log"></param>
		public DirectoryManager(ILoggingService log, IImageController controller)
		{
			//taking paths given in the config
			string[] dest = ConfigurationManager.AppSettings["Handler"].Split(';');
			for (int i = 0; i < dest.Count(); i++)
			{
				IDirectoryHandler dH = new DirectoryHandler(controller, log);
				CommandRecieved += dH.OnCommandRecieved;
				dH.DirectoryClose += OnDirClosed;
				try
				{
					dH.StartHandleDirectory(dest[i]);
				}
				catch (Exception e)
				{
					m_logging.Log("directory" + dest[i] + "couldn't be handeled", MessageTypeEnum.FAIL);
				}
			}
		}
		/// <summary>
		/// method to close the manager by commanding the handlers to close first
		/// </summary>
		public void CloseManager()
		{   // invoke close all directories CommandRecieved Event
			CommandRecievedEventArgs args = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, "*");
			CommandRecieved.Invoke(this, args);
			// wait for all handlers to close
			while ((CommandRecieved != null) && (CommandRecieved.GetInvocationList().Length > 0))
				System.Threading.Thread.Sleep(1000);
			// update logger
			m_logging.Log("All Directories are Closed", MessageTypeEnum.INFO);
		}
		/// <summary>
		/// OnDirClosed is summoned by the DirClose event and the method gets the directory from the event handler list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnDirClosed(object sender, DirectoryCloseEventArgs e)
		{
			IDirectoryHandler d = (IDirectoryHandler)sender;
			d.DirectoryClose -= OnDirClosed;
		}

		public string HandlerList()
		{
			String list = "";
			if (CommandRecieved == null)
				return list;
			Delegate[] handlers = CommandRecieved.GetInvocationList();
			for (int i = 0; i < handlers.Length; i++)
				list = list + ";" + ((DirectoryHandler)handlers[i].Target).m_path;
			return list;
		}
	}
}