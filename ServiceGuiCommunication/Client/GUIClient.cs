using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;

namespace GUICommunication.Client
{
    public class GUIClient : IGUIClient
    {
        private TcpClient m_client;
        // A list of messages that wait for sending
        private List<string> m_messages = new List<string>();
        private static GUIClient instance = null;
        //locking obj
        private static object obj = new object();
        /// <summary>
        /// connected bool databinding protocol
        /// </summary>
        private bool _connected;
        public bool connected
        {
            get
            {
                return _connected;
            }
            set
            {
                if(_connected != value)
                {
                    _connected = value;
                    NotifyPropertyChanged("connected");
                }
            }
        }

		#region Properties
		// The event that notifies about a new message being recieved
		public event EventHandler<string> NewMessage;
        //property changed for connected bool
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
        /// <summary>
        /// ctor init port
        /// </summary>
        private GUIClient() {
            int port = 9999;
            Connect(port);
        }
        /// <summary>
        /// made GuiClient singelton because the severak models use him
        /// </summary>
        /// <returns></returns>
		public static GUIClient Instance()
		{
            if (instance == null)
            {
                lock (obj)
                {
                    if (instance == null)
                        instance = new GUIClient();
                }
            }
            return instance;
        }

		/// <summary>
		/// The method makes the GUIClient starts handle
		/// the communication with the server.
		/// </summary>
		/// <param name="port">the port number</param>
		public void Connect(int port)
		{
            // check if client is already connected
            if (m_client == null)
            {
                lock (obj)
                {
                    if ((m_client != null) && (m_client.Connected))
                        return;
                }
            }
			// open communication
			IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
			m_client = new TcpClient();
            try
            {
                m_client.Connect(ep);
                connected = true;
            }
            catch (Exception)
            {
                connected = false;
                return;
            }
			// a communication task
			Task task = new Task(() => 
			{
				using (NetworkStream stream = m_client.GetStream())
				using (BinaryReader reader = new BinaryReader(stream))
				using (BinaryWriter writer = new BinaryWriter(stream))
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
                                    try
                                    {
                                        writer.Write("ping"); // send ping for feedback
                                    }
                                    catch (Exception)
                                    {
                                        connected = false;
                                    } 
								else
								{
									// send the message
									writer.Write(m_messages[0]);
									m_messages.RemoveAt(0); // remove the message from the list
								}
							}
							writer.Flush();
							// read incomming message or feedback
							string message = reader.ReadString();
							// check if got a new message, or just feedback
							if (message != "ping")
							{
                                // invoke NewMessage event
								NewMessage.Invoke(this, message);
							}
                            connected = true;
                            if (message == null)
                                connected = false;
						}
						catch (Exception)
						{
                            connected = false;
                        }
					}
				}
				// close communication (if still open)
				m_client.Close();
			});
			task.Start();
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

        public bool isConnected()
        {
           return connected;
        }
    }
}