using System.Net.Sockets;

namespace ServiceGuiCommunication.Service_side
{
    public interface IClientHandler
    {
        void HandleClient(TcpClient client);
    }
}