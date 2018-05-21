using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.SettingsTab
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private string _OPD;
        public string OPD
        {
            get { return _OPD; }
            set
            {
                if (_OPD != value)
                {
                    _OPD = value;
                    OnPropertyChanged("OPD");
                }
            }
        }
        private string _thumbSize;
        public string thumbSize
        {
            get { return _thumbSize; }
            set
            {
                if (_thumbSize != value)
                {
                    _thumbSize = value;
                    OnPropertyChanged("thumbSize");
                }
            }
        }
        private string _source { get; set; }
        public string source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    OnPropertyChanged("source");
                }
            }
        }
        private string _logName { get; set; }
        public string logName
        {
            get { return _logName; }
            set
            {
                if (_logName != value)
                {
                    _logName = value;
                    OnPropertyChanged("logName");
                }
            }
        }

        public SettingsViewModel()
        {
            _OPD = "blablab/kljlsfjdf";
            thumbSize = "120";
            logName = "log";
            source = "source";
        }
        
        public void btnRemove_Click(object sender, EventArgs e)
        {
            return;
        }

    }
}
