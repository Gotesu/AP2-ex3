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
        /// <summary>
        /// event handler event of a new log entry, adding the entry and raising the notify event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="entry"></param>
        public void OnLogRecieved(object sender, EventLogEntry entry)
        {
            model_entries.Add(entry);
            NotifyPropertyChanged("entries");
        }
        /// <summary>
        /// data transfer from model to viewmodel and to view
        /// </summary>
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
        /// <summary>
        /// ctor, using the connection client to get data from image service.
        /// </summary>
        public LogModel()
        {
            
            client = GuiSide_client.get_instance();
            entries = new ObservableCollection<EventLogEntry>();
            buildLog(entries, client.getEntries());
            EventLog log = new EventLog();
            
        }
        /// <summary>
        /// build observable log entry list from event log given by the client from the image service.
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="fromService"></param>
        public void buildLog(ObservableCollection<EventLogEntry> modelList,EventLogEntryCollection fromService)
        {
            foreach (EventLogEntry entry in fromService)
            {
                modelList.Add(entry);
            }
        }
    }
}
