using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.SettingsTab
{
    class ISettingsModel
    {
        public string model_OPD {get; set;}
        public string model_logName { get; set; }
        public string model_source { get; set; }
        public string model_thumbSize { get; set; }
        public ObservableCollection<string> model_handlers { get; set; }
    }
}
