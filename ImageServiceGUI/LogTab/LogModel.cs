using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.LogTab
{
    class LogModel:ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
            entries = new ObservableCollection<EventLogEntry>();
            EventLog log = new EventLog();
            log.Source = "source";
            log.WriteEntry("wowow");
            log.WriteEntry("wowow1");
            log.WriteEntry("wowow2");
            entries.Add(log.Entries[0]);
            entries.Add(log.Entries[1]);
            entries.Add(log.Entries[2]);
        }
    }
}
