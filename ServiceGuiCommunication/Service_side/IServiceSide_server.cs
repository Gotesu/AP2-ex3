using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServiceGuiCommunication.Service_side
{
    interface IServiceSide_server
    {
        IServiceSide_server getServer();
        int Port { get; set; }
        IClientHandler ClHandler { get; set; }
    }
}
