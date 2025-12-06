using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using MedicalCharting.Models;

namespace MedicalCharting.Converters
{
    public class AgeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Patient p)
            {
                int age = DateTime.Now.Year - p.BirthDate.Year;
                if (p.BirthDate > DateTime.Today.AddYears(-age))
                    age--;

                if (age < 18)
                    return Colors.Gold;   // highlight minors
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
