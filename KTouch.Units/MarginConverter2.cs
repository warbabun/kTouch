using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KTouch.Units {
    public class MarginConverter2 : IValueConverter {
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture ) {
            double height = ( double ) value / 20;
            double width = height / Math.Sqrt ( 2.0 );
            return new Thickness ( width, height, width, height );
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotImplementedException ( );
        }
    }
}
