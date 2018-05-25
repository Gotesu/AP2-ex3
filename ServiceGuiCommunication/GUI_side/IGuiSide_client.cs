using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;

namespace ServiceGuiCommunication.GUI_side
{
    public interface IGuiSide_client
    {
        ImageServiceConfig getConfig();
        EventLogEntryCollection getEntries();
        void closeClient();
        event EventHandler<EventLogEntry> newLog;

    }
}
