using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageServiceGUI.SettingsTab
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private ISettingsModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public string OPD
        {
            get { return model.OPD; }
            set
            {
                if (model.OPD != value)
                {
                    model.OPD = value;
                    OnPropertyChanged("OPD");
                }
            }
        }
        public string thumbSize
        {
            get { return model.thumbSize; }
            set
            {
                if (model.thumbSize != value)
                {
                    model.thumbSize = value;
                    OnPropertyChanged("thumbSize");
                }
            }
        }
        public string source
        {
            get { return model.source; }
            set
            {
                if (model.source != value)
                {
                    model.source = value;
                    OnPropertyChanged("source");
                }
            }
        }
        public string logName
        {
            get { return model.logName; }
            set
            {
                if (model.logName != value)
                {
                    model.logName = value;
                    OnPropertyChanged("logName");
                }
            }
        }
        public ObservableCollection<string> handlers
        {
            get
            {
                return model.handlers;
            }

            set
            {
                model.handlers = value;
                OnPropertyChanged("handlers");
            }
        }
        private string _selectedModel;
        public string SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                OnPropertyChanged("SelectedModel");
                //taken from the simple MVVM microsoft example
                var command = this.removeCommand as DelegateCommand<object>;
                command.RaiseCanExecuteChanged();
            }
        }

        public ICommand removeCommand { get; private set; }
       
        private void remove_click(object sender)
        {
            handlers.Remove(SelectedModel);
        }
        private bool canRemove(object o)
        {
            if (SelectedModel != null)
                return true;
            return false;
        }
        public SettingsViewModel()
        {
            model = new SettingsModel();
            this.removeCommand = new DelegateCommand<object>(this.remove_click, canRemove);
        }

    }
}
