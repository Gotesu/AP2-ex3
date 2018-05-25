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
    interface IGuiSide_client
    {
        ImageServiceConfig getConfig();
        EventLog getLog();
        EventLogEntryCollection getEntries();
        EventLogEntry getEntry();
        void closeClient();
    }
}
