using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BOMBS.Client.Communicator.Server.Controls
{
//    [ValueConversion(typeof(object), typeof(string))]    

    [ValueConversion(typeof(bool), typeof(string))]
    public class ServerConfigurationStatusImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = null;
            switch ((ServerConfigurationStatus)value)
            {
                case ServerConfigurationStatus.Active:
                    result = @"../../../Resources/tick.png";
                    break;
                case ServerConfigurationStatus.NoConnection:
                    result = @"../../../Resources/error.png";
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
