using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KTouch.Units {
    public class SumConverter : IValueConverter {
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture ) {
            double convertedParameter;
            if ( Double.TryParse ( parameter.ToString ( ), out convertedParameter ) )
                return ( double ) value + convertedParameter;
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotImplementedException ( );
        }
    }
}
