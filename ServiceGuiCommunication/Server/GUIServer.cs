using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ImageService.Logging;

namespace GUICommunication.Server
{
	public class GUIServer : IGUIServer
	{
		private int port;
		private TcpListener listener;
		// The function that execute when a new Command being recieved
		private EventHandler<string> ExecuteCommand;

		#region Properties
		// The event that notifies about a new Command being to send
		public event EventHandler<string> SendAll;
		public event EventHandler CloseAll;
		private ILoggingService m_logging;
		#endregion

		/// <summary>
		/// A constructor method.
		/// </summary>
		/// <param name="port">the port number</param>
		/// <param name="log"></param>
		/// <param name="execute">the function that execute
		///			when a new Command being recieved</param>
		public GUIServer(int port, ILoggingService log, EventHandler<string> execute)
		{
			m_logging = log;
			this.port = port;
			IPEndPoint ep = new
			IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
			listener = new TcpListener(ep);
			ExecuteCommand = execute;
		}

		/// <summary>
		/// The method makes the server starts getting new clients.
		/// </summary>
		public void Start()
		{
			// start listening for new clients
			listener.Start();
			m_logging.Log("Waiting for connections...", MessageTypeEnum.INFO);
			Task task = new Task(() =>
			{
				// a loop for recieving new clients
				while (true)
				{
					try
					{
						// get new client
						TcpClient client = listener.AcceptTcpClient();
						// create a client handler
						IClientHandler ch = new ClientHandler(client);
						m_logging.Log("Got new connection", MessageTypeEnum.INFO);
						// set all the required evethandlers
						SendAll += ch.OnSendAll;
						CloseAll += ch.OnCloseAll;
						ch.ClientClose += OnClientClose;
						ch.NewMessage += ExecuteCommand;
						// start handle the client
						ch.HandleClient();
					}
					catch (SocketException e)
					{
						m_logging.Log(e.Message, MessageTypeEnum.FAIL);
						break;
					}
				}
				m_logging.Log("Server stopped", MessageTypeEnum.INFO);
			});
			task.Start();
		}

		/// <summary>
		/// The method send the new message to all the open clients,
		/// by invoking the SendAll event.
		/// </summary>
		/// <param name="message">the message string to send</param>
		public void Send(string message)
		{
			m_logging.Log("Update all clients", MessageTypeEnum.INFO);
			SendAll.Invoke(this, message);
		}

		/// <summary>
		/// The method stops the server from listening for new clients.
		/// Note: the method does not close the open clients.
		/// </summary>
		public void Stop()
		{
			// stop listening for new clients
			listener.Stop();
		}

		/// <summary>
		/// method to close the server by stops the server from listening for new clients,
		/// and commanding the open handlers to close.
		/// </summary>
		public void Close()
		{
			listener.Stop();
			// invoke close all directories CommandRecieved Event
			CloseAll.Invoke(this, null);
			// wait for all handlers to close
			while ((CloseAll != null) && (CloseAll.GetInvocationList().Length > 0))
				System.Threading.Thread.Sleep(1000);
			m_logging.Log("Server closed", MessageTypeEnum.INFO);
		}

		/// <summary>
		/// OnClientClose is summoned by the ClientClose event.
		/// The method gets the ClientHandler out from the SendAll event handlers list,
		/// and gets the ExecuteCommand out from the ClientHandler's NewMessage event handlers list.
		/// </summary>
		/// <param name="sender">the ClientHandler that closed</param>
		public void OnClientClose(object sender, EventArgs args)
		{
			m_logging.Log("Client closed", MessageTypeEnum.INFO);
			IClientHandler ch = (IClientHandler)sender;
			SendAll -= ch.OnSendAll;
			ch.NewMessage -= ExecuteCommand;
		}
	}
}