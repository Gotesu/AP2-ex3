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

namespace ImageServiceGUI.SettingsTab
{
    /// <summary>
    /// model of the MVVM settings tab, used to maintain connection with the image service and store data passed to GUI
    /// </summary>
    class SettingsModel : ISettingsModel
    {
		private IGUIClient client;
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
            client = GUIClient.Instance();
			client.NewMessage += UpdateSettings;
			model_OPD = "";
			model_handlers =new ObservableCollection<string>();
            model_logName = "";
            model_source = "";
            model_thumbSize = "";
			JObject response = new JObject();
			response["commandID"] = (int)CommandEnum.GetConfigCommand;
			client.SendMessage(response.ToString());
            Task.Delay(50).Wait();
		}

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
                    try
                    {
                        model_handlers.Add(handler);
                    }
                    catch (Exception)
                    {
                    }
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
    }
}