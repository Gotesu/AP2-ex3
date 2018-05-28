using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.SettingsTab
{
    /// <summary>
    /// this interface is used for the settings tab model (MVVM)
    /// </summary>
    interface ISettingsModel : INotifyPropertyChanged
    {
        string OPD {get; set;}
        string logName { get; set; }
        string source { get; set; }
        string thumbSize { get; set; }
        ObservableCollection<string> handlers { get; set; }
		void RemoveHandler(string path);
	}
}
