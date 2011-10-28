using System;
using System.Globalization;
using System.Windows.Data;

namespace KTouch.Units {
    public class MultiplicationConverter : IValueConverter {
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture ) {
            double convertedParameter;
            //if (Double.TryParse(parameter as string, out convertedParameter))
            //    return (double)value * convertedParameter;
            //return DependencyProperty.UnsetValue;
            var frac = ( ( string ) parameter ).Split ( '/' );
            if ( frac.Length == 2 )
                return ( double ) value * double.Parse ( frac [ 0 ] ) / double.Parse ( frac [ 1 ] );
            convertedParameter = double.Parse ( ( ( string ) parameter ).Replace ( ".", "," ) );
            return ( double ) value * convertedParameter;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotImplementedException ( );
        }
    }
}
