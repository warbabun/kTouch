using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KTouch.Units {
    public class MarginConverter1 : IValueConverter {
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture ) {
            double height = ( double ) value;
            double convertedParameter = double.Parse ( ( ( string ) parameter ).Replace ( ".", "," ) );
            if ( convertedParameter != 0 )
                return new Thickness ( height / convertedParameter );
            else
                return new Thickness ( );
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotImplementedException ( );
        }
    }
}
