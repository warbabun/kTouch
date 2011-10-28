using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KTouch.Units {
    public class SizeToPointConverter : IValueConverter {
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture ) {
            Size s = ( Size ) value;
            return new Point ( s.Width / 2.0, s.Height );
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotImplementedException ( );
        }
    }
}
