﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.SettingsTab
{
    class SettingsModel : ISettingsModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName) {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        private ObservableCollection<string> model_handlers;
        public ObservableCollection<string> handlers
        {
            get
            {
                return model_handlers;
            }

            set
            {
                model_handlers = value;
                NotifyPropertyChanged("handlers");
            }
        }
        private string model_logName;
        public string logName
        {
            get
            {
                return model_logName;
            }

            set
            {
                model_logName = value;
                NotifyPropertyChanged("logName");
            }
        }

        private string model_OPD;
        public string OPD
        {
            get
            {
                return model_OPD;
            }

            set
            {
                model_OPD = value;
                NotifyPropertyChanged("OPD");
            }
        }
        private string model_source;
        public string source
        {
            get
            {
                return model_source;
            }

            set
            {
                model_source = value;
                NotifyPropertyChanged("source");
            }
        }
        public string model_thumbSize;
        public string thumbSize
        {
            get
            {
                return model_thumbSize;
            }

            set
            {
                model_thumbSize = value;
                NotifyPropertyChanged("thumbSize");
            }
        }
        public SettingsModel()
        {
            model_OPD = "ldfjglsdkjglksljd";
            handlers = new ObservableCollection<string>();
            handlers.Add("wow");
        }
    }
}
