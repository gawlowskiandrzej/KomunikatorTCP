using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClientWPF.Models
{
    public class IndexToAlignmentConverter : IValueConverter
    {
        Repository repo = new Repository();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as string == repo.GetLoggedUser().Name as string)
            {
                return HorizontalAlignment.Right;
            }
            return HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
