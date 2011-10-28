using System;
using System.Globalization;
using System.Windows.Data;

namespace KTouch.Units {
    public class CountConverter : IValueConverter {
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture ) {
            bool newValue = ( int ) value > 0;
            return newValue;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotImplementedException ( );
        }
    }
}
