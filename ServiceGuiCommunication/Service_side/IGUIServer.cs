using System;
namespace GUICommunication.Server
{
	public interface IGUIServer
	{
		event EventHandler<string> SendAll;
		void Start();
		void Send(string message);
		void Stop();
		void OnClientClose(object sender, EventArgs args);
	}
}