using ImageService.Infrastructure;
using GUICommunication.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using ImageService.Infrastructure.Enums;
using System;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageServiceGUI.SettingsTab
{
    /// <summary>
    /// model of the MVVM settings tab, used to maintain connection with the image service and store data passed to GUI
    /// </summary>
    class SettingsModel : ISettingsModel
    {
		private IGUIClient client;
        //data binding property change event
		public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// notify on property change
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName) {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        //colection of handlers
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
        //again data binding protocol
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
        //same
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
        //same
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
        //same
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
        //lock objext
        private static object _lock = new object();
        /// <summary>
        /// ctor init as blank and than updating using event (different func)
        /// </summary>
        public SettingsModel()
        {
            client = GUIClient.Instance();
            //update setting on new message event
			client.NewMessage += UpdateSettings;
            //blank properties
			model_OPD = "";
			model_handlers =new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(model_handlers, _lock);
            model_logName = "";
            model_source = "";
            model_thumbSize = "";
            //requesting config data
			JObject response = new JObject();
			response["commandID"] = (int)CommandEnum.GetConfigCommand;
			client.SendMessage(response.ToString());
            //waiting for update with delay
            Task.Delay(500).Wait();
		}
        /// <summary>
        /// on new message, updates the config
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void UpdateSettings(object sender, string message)
        {
            JObject command = JObject.Parse(message);
            int commandID = (int)command["commandID"];
            // check the commandID for a matching response
            if (commandID == (int)CommandEnum.GetConfigCommand)
            {
                // in case of GetConfigCommand
                ImageServiceConfig fromService =
                    ImageServiceConfig.FromJSON((string)command["config"]);
                foreach (string handler in fromService.handlers)
                {
                    model_handlers.Add(handler);
                }
                  
                model_thumbSize = fromService.thumbSize.ToString();
                model_logName = fromService.logName;
                model_OPD = fromService.OPD;
                model_source = fromService.source;
            }
            else if (commandID == (int)CommandEnum.CloseCommand)
            {
                // in case of CloseCommand update
                string dir = (string)command["path"];
                if (model_handlers != null)
                    model_handlers.Remove(dir);
            }
        }
        /// <summary>
        /// removing handler on remove click
        /// </summary>
        /// <param name="path"></param>
		public void RemoveHandler (string path)
		{
			JObject response = new JObject();
			response["commandID"] = ((int)CommandEnum.CloseCommand).ToString();
			response["args"] = "";
			response["path"] = path;
			client.SendMessage(response.ToString());
		}
    }
}