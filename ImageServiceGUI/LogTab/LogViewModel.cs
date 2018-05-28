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
    class LogViewModel
    {
        //model instance
        private ILogModel model;
        //property change for binding
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        //entries 
        public ObservableCollection<EventLogEntry> entries
        {
            get
            {
                return model.entries;
            }

            set
            {
                model.entries = value;
                OnPropertyChanged("entries");
            }
        }
        public LogViewModel()
        {
            model = new LogModel();
        }
    }
}
