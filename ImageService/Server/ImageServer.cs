using System;
using ImageService.Logging;
using ImageService.Controller;
using ImageService.Model;
using System.Configuration;

namespace ImageService.Server
{
	/// <summary>
	/// ImageServer is the server that sends handlers to handle directories given in appconfig (for the time being)
	/// the server communicates with the handlers via eventHandlers
	/// </summary>
	public class ImageServer
	{
		#region Members
		private ILoggingService m_logging;
		private DirectoryManager m_handlers;
		private int m_thumbSize;
		private string m_outputDir;
		private string m_sourceName;
		private string m_logName;
		#endregion

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="log"></param>
		public ImageServer(ILoggingService log)
		{
			m_logging = log;
			// taking info from config
			m_outputDir = ConfigurationManager.AppSettings["OutputDir"];
			m_sourceName = ConfigurationManager.AppSettings["SourceName"];
			m_logName = ConfigurationManager.AppSettings["LogName"];
			m_thumbSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
			//one controller to rule them all
			IImageController controller = new ImageController(
				new ImageModel(m_outputDir, m_thumbSize));
			m_handlers = new DirectoryManager(m_logging, controller);
		}
		/// <summary>
		/// method to close the server by commanding the handlers to close first
		/// </summary>
		public void CloseServer()
		{
			m_handlers.CloseManager();
			// update logger
			m_logging.Log("Server is Closed", MessageTypeEnum.INFO);
		}

	}
}