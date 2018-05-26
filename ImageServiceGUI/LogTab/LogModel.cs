using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceGuiCommunication.GUI_side;

namespace ImageServiceGUI.LogTab
{
    class LogModel:ILogModel
    {
        private IGuiSide_client client;
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void OnLogRecieved(object sender, EventLogEntry entry)
        {
            model_entries.Add(entry);
            NotifyPropertyChanged("entries");
        }

        private ObservableCollection<EventLogEntry> model_entries;
        public ObservableCollection<EventLogEntry> entries
        {
            get
            {
                return model_entries;
            }

            set
            {
                model_entries = value;
                NotifyPropertyChanged("entries");
            }
        }
        public LogModel()
        {
            
            client = GuiSide_client.get_instance();
            entries = new ObservableCollection<EventLogEntry>();
            buildLog(entries, client.getEntries());
            EventLog log = new EventLog();
            
        }

        public void buildLog(ObservableCollection<EventLogEntry> modelList,EventLogEntryCollection fromService)
        {
            foreach (EventLogEntry entry in fromService)
            {
                modelList.Add(entry);
            }
        }
    }
}
