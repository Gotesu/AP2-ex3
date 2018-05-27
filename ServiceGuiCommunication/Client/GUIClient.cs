using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GUICommunication.Client
{
	public class GUIClient : IGUIClient
	{
		private TcpClient m_client;
		// A list of messages that wait for sending
		private List<string> m_messages = new List<string>();
		private static GUIClient instance = null;

		#region Properties
		// The event that notifies about a new message being recieved
		public event EventHandler<string> NewMessage;
		#endregion

		private GUIClient() {}

		public static GUIClient Instance()
		{
			lock (instance)
			{
				if (instance == null)
				{
					instance = new GUIClient();

				}
				return instance;
			}
		}

		/// <summary>
		/// The method makes the GUIClient starts handle
		/// the communication with the server.
		/// </summary>
		/// <param name="port">the port number</param>
		public void Connect(int port)
		{
			// check if client is already connected
			lock (m_client)
			{
				if ((m_client != null) && (m_client.Connected))
					return;
			}
			// open communication
			IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
			m_client = new TcpClient();
			m_client.Connect(ep);
			// a communication task
			Task task = new Task(() =>
			{
				using (NetworkStream stream = m_client.GetStream())
				using (StreamReader reader = new StreamReader(stream))
				using (StreamWriter writer = new StreamWriter(stream))
				{
					// a loop that continue wile communication open
					while (m_client.Connected)
					{
						try
						{
							lock (m_messages)
							{
								// check if there is a message to send
								if ((m_messages == null) || (m_messages.Count <= 0))
									writer.WriteLine("ping"); // send ping for feedback
								else
								{
									// send the message
									writer.WriteLine(m_messages[0]);
									m_messages.RemoveAt(0); // remove the message from the list
								}
							}
							writer.Flush();
							// read incomming message or feedback
							string message = reader.ReadLine();
							// check if got a new message, or just feedback
							if (message != "ping")
							{
								// invoke NewMessage event
								NewMessage.Invoke(this, message);
							}
						}
						catch (Exception e)
						{
							break;
						}
					}
				}
				// close communication (if still open)
				m_client.Close();
			});
			task.Start();
		}

		public bool IsConnected()
		{
			if (m_client != null)
				return m_client.Connected;
			return false;
		}

		/// <summary>
		/// The method disconnects the client from the server,
		/// and clear the NewMessage event handlers list.
		/// </summary>
		public void Disconnect()
		{
			// close communication
			m_client.Close();
			// clear NewMessage event invocation list
			NewMessage = null;
		}

		/// <summary>
		/// The method add the given message to the list of messages to send.
		/// </summary>
		/// <param name="message">the message string to send</param>
		public void SendMessage(string message)
		{
			// check if the communication open
			if (!m_client.Connected)
				return;
			lock (m_messages)
			{
				// add the new message for the list
				m_messages.Add(message);
			}
		}
	}
}