using System;
using System.ComponentModel;

namespace GUICommunication.Client
{
	public interface IGUIClient : INotifyPropertyChanged
	{
		event EventHandler<string> NewMessage;
		void Connect(int port);
		void Disconnect();
		void SendMessage(string message);
	}
}