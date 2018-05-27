using System;
namespace GUICommunication.Client
{
	public interface IGUIClient
	{
		event EventHandler<string> NewMessage;
		void Connect(int port);
		void Disconnect();
		void SendMessage(string message);
	}
}