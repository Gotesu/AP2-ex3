
using Prism.Commands;
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
    /// <summary>
    /// ViewModel of the settings tab, inherets Inotify because of data binding
    /// </summary>
    class SettingsViewModel : INotifyPropertyChanged
    {
        private ISettingsModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        //output dir directory path
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
        //thumbnail size
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
        //log source
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
        //log name
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
        //handlers list
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
        /// <summary>
        /// this code is for enabling the remove button on select of the handler.
        /// selected model is the item in the item list that is selected by the user using the GUI
        /// </summary>
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
                //raising the can execute to enable execution of the remove button
                command.RaiseCanExecuteChanged();
            }
        }

        public ICommand removeCommand { get; private set; }
       /// <summary>
       /// remove button click removes handler
       /// </summary>
       /// <param name="sender"></param>
        private void remove_click(object sender)
        {
            handlers.Remove(SelectedModel);
        }
        /// <summary>
        /// canRemove is the enabling can execute function of the remove button, condition is that an item was selected from the list.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool canRemove(object o)
        {
            if (SelectedModel != null)
                return true;
            return false;
        }
        /// <summary>
        /// ctor
        /// the viewModel holds a model instance
        /// </summary>
        public SettingsViewModel()
        {
            model = new SettingsModel();
            //making the button's command the remove command with canRemove condition
            this.removeCommand = new DelegateCommand<object>(this.remove_click, canRemove);
        }

    }
}
