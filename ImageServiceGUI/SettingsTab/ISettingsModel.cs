using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.SettingsTab
{
    interface ISettingsModel
    {
        string model_OPD {get; set;}
        string model_logName { get; set; }
        string model_source { get; set; }
        string model_thumbSize { get; set; }
        ObservableCollection<string> model_handlers { get; set; }
    }
}
