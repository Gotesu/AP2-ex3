using System;
using System.Collections.Generic;
using System.Linq;
using ImageService.Logging;
using ImageService.Model;
using System.Configuration;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure;
using GUICommunication.Server;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using ImageService.Controller.Handlers;
using ImageService.Logging.Modal;

namespace ImageService.Server
{
    /// <summary>
    /// ImageServer is the server that sends handlers to handle directories given in appconfig (for the time being)
    /// the server communicates with the handlers via eventHandlers
    /// </summary>
    public class ImageServer
	{
		#region Members
		private EventLog m_eventLogger;
		private ImageServiceConfig m_config;
		private DirectoryManager dm;
		private IGUIServer guis;
		#endregion
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="log"></param>
        public ImageServer(ILoggingService log, EventLog eventLogger)
        {
			m_eventLogger = eventLogger;
			//taking info given in the config
			List<string> dest = ConfigurationManager.AppSettings["Handler"].Split(';').ToList();
			int thumbSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
			string sourceName = ConfigurationManager.AppSettings["SourceName"];
			string logName = ConfigurationManager.AppSettings["LogName"];
			string outputDir = ConfigurationManager.AppSettings["OutputDir"];
			// build the config object
			m_config = new ImageServiceConfig(dest, thumbSize, sourceName, logName, outputDir);
			dm = new DirectoryManager(log, m_config, this.OnDirClosed);
			guis = new GUIServer(9999, log, this.OnNewMessage);
			eventLogger.EntryWritten += WhenEntryWritten;
			eventLogger.EnableRaisingEvents = true;
			guis.Start();
		}

        /// <summary>
        /// method to close the server
        /// </summary>
		public void CloseServer()
		{
			dm.CloseServer();
			guis.Close();
			m_eventLogger.EntryWritten -= WhenEntryWritten;
		}

		/// <summary>
		/// OnNewMessage is summoned by the NewMessage event and the method
		/// excute the NewMessage command.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message">the message string</param>
		public void OnNewMessage(object sender, string message)
        {
			JObject command = JObject.Parse(message);
			int commandID = (int)command["commandID"];
			// check the commandID for a matching response
			if (commandID == (int)CommandEnum.GetConfigCommand)
			{
				// send config info
				JObject response = new JObject();
				response["commandID"] = commandID;
				response["config"] = m_config.ToJSON();
				((IClientHandler)sender).SendMessage(response.ToString());
			}
			else if (commandID == (int)CommandEnum.LogCommand)
			{
				// send log info
				JObject response = new JObject();
				response["commandID"] = commandID;
				response["LogCollection"] = JsonConvert.SerializeObject(m_eventLogger.Entries);
				((IClientHandler)sender).SendMessage(response.ToString());
			}
			else
			{
				// excute command
				CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs(commandID,
					((string)command["args"]).Split(';'), (string)command["path"]);
				dm.WhenCommandRecieved(eventArgs);
			}
		}

		/// <summary>
		/// OnDirClosed is summoned by the DirClose event and the method
		/// gets the directory out from the event handlers list, and updates all clients.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnDirClosed(object sender, DirectoryCloseEventArgs e)
		{
			IDirectoryHandler d = (IDirectoryHandler)sender;
			d.DirectoryClose -= OnDirClosed;
			// updte config object
			lock (d.m_path)
			{
				m_config.handlers.Remove(d.m_path);
			}
			// update clients
			JObject response = new JObject();
			response["commandID"] = ((int)CommandEnum.CloseCommand).ToString();
			response["path"] = d.m_path;
			guis.Send(response.ToString());
		}

		/// <summary>
		/// WhenEntryWritten is summoned by the EntryWritten event.
		/// The method updates all the clients.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void WhenEntryWritten(object sender,
			System.Diagnostics.EntryWrittenEventArgs args)
		{
			// update clients
			JObject response = new JObject();
			response["commandID"] = ((int)CommandEnum.LogUpdate).ToString();
			response["Log"] = JsonConvert.SerializeObject(args.Entry);
			guis.Send(response.ToString());
		}
	}
}
