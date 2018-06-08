using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Windows.Data;
using GUICommunication.Client;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImageServiceWeb.Models
{
    public class LogModel
    {
		private IGUIClient client;
        //event for databinding
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// on change
        /// </summary>
        /// <param name="propName"></param>
		public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        /// <summary>
        /// new log revieved need to be written to data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="entry"></param>
		public void OnLogRecieved(object sender, EventLogEntry entry)
        {
            model_entries.Add(entry);
            NotifyPropertyChanged("entries");
        }
        //entries collection
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
        //lock object
        private static object _lock = new object();
        public LogModel()
        {
            client = GUIClient.Instance();
            model_entries = new ObservableCollection<EventLogEntry>();
            //to enable non UI thread to update the collection
            BindingOperations.EnableCollectionSynchronization(model_entries, _lock);
            //enlisting for new log message event case
            client.NewMessage += UpdateLog;
            //sending request for log base
            JObject response = new JObject();
            response["commandID"] = (int)CommandEnum.LogCommand;
            client.SendMessage(response.ToString());
        }
        /// <summary>
        /// init of log on connection start using json or just log update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
		public void UpdateLog(object sender, string message)
        {
            JObject command = JObject.Parse(message);
            int commandID = (int)command["commandID"];
            // check the commandID for a matching response
            if (commandID == (int)CommandEnum.LogCommand)
            {
                //case init of log
                EventLogEntry[] fromService =
                    JsonConvert.DeserializeObject<EventLogEntry[]>((string)command["LogCollection"]);
                foreach (EventLogEntry entry in fromService)
                {
                    model_entries.Add(entry);
                }
            }
            else if (commandID == (int)CommandEnum.LogUpdate)
            {
                //case log update
                EventLogEntry newLog =
                    JsonConvert.DeserializeObject<EventLogEntry>((string)command["Log"]);
                model_entries.Add(newLog);
            }
        }
    }
}
