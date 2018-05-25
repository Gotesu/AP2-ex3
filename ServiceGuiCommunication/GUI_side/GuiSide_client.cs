using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServiceGuiCommunication.GUI_side
{
    class GuiSide_client : IGuiSide_client
    {
        private IPEndPoint ep { get; }
        private TcpClient client { get; }

        public GuiSide_client()
        {
            ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            client = new TcpClient();
            client.Connect(ep);
        }

        public void closeClient()
        {
            client.Close();
        }

        public ImageServiceConfig getConfig()
        {
            throw new NotImplementedException();
        }

        public EventLogEntryCollection getEntries()
        {
            throw new NotImplementedException();
        }

        public EventLogEntry getEntry()
        {
            throw new NotImplementedException();
        }

        public EventLog getLog()
        {
            throw new NotImplementedException();
        }
    }
}
