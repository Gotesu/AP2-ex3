using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageServiceGUI.LogTab
{
    /// <summary>
    /// convertor from type to type name for event log entries
    /// </summary>
    public class TypeToMemo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value.ToString();
            switch (val)
            {
                case "Information":
                    return "INFO";
                case "Error":
                    return "ERROR";
                case "Warning":
                    return "WARNING";
                default:
                    //made info the default
                    return "INFO";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
