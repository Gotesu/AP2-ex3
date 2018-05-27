using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GUICommunication.Server
{
	class ClientHandler : IClientHandler
	{
		private TcpClient m_client;
		// A list of messages that wait for sending
		private List<string> m_messages;

		#region Properties
		// The event that notifies about a new message being recieved
		public event EventHandler<string> NewMessage;
		// The Event That Notifies that the Client is being closed
		public event EventHandler ClientClose;
		#endregion

		/// <summary>
		/// A constructor method.
		/// </summary>
		/// <param name="client">the TcpClient</param>
		public ClientHandler(TcpClient client)
		{
			m_client = client;
			m_messages = new List<string>();
		}

		/// <summary>
		/// The method makes the HandleClient starts handle
		/// the communication with the client.
		/// </summary>
		public void HandleClient()
		{
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
							// read incomming message or feedback
							string commandLine = reader.ReadLine();
							// check if got a new message, or just feedback
							if (commandLine != "ping")
							{
								// invoke NewMessage event
								NewMessage.Invoke(this, commandLine);
							}
							lock (m_messages)
							{
								// check if there is a message to send
								if ((m_messages == null) || (m_messages.Count <= 0))
									writer.WriteLine("ping"); // send ping for feedback
								else
								{
									// send the message
									writer.WriteLine(m_messages[0]);
									m_messages.RemoveAt(0);
								}
							}
							writer.Flush();
						}
						catch (Exception e)
						{
							break;
						}
					}
				}
				// close communication (if still open)
				m_client.Close();
				ClientClose.Invoke(this, null);
			});
			task.Start();
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
				// add the new message to the list
				m_messages.Add(message);
			}
		}

		/// <summary>
		/// OnSendAll is summoned by the SendAll event.
		/// The method sends the given message using the SendMessage method.
		/// </summary>
		/// <param name="sender">the object that invoke the event</param>
		/// <param name="message">the message string to send</param>
		public void OnSendAll(object sender, string message)
		{
			SendMessage(message);
		}

		/// <summary>
		/// OnCloseAll is summoned by the CloseAll event.
		/// The method close the client handler.
		/// </summary>
		/// <param name="sender">the object that invoke the event</param>
		public void OnCloseAll(object sender, EventArgs args)
		{
			m_client.Close();
			// clear NewMessage event invocation list
			NewMessage = null;
			ClientClose.Invoke(this, null);
		}
	}
}