using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.LogTab
{
    interface ILogModel
    {
        ObservableCollection<EventLogEntry> entries { get; set; }
        void OnLogRecieved(object sender, EventLogEntry entry);
    }
}
