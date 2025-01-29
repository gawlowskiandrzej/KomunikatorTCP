using ClientWPF.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClientWPF.Models
{
    public class IndexToAlignmentConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as string == MainVM.Repository.GetLoggedUser().Name)
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
