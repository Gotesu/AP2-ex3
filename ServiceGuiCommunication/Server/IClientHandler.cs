using System;
namespace GUICommunication.Server
{
	public interface IClientHandler
	{
		event EventHandler<string> NewMessage;
		event EventHandler ClientClose;
		void HandleClient();
		void SendMessage(string message);
		void OnSendAll(object sender, string message);
		void OnCloseAll(object sender, EventArgs args);
	}
}