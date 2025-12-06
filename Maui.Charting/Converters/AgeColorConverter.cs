using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using MedicalCharting.Models;

namespace Maui.Charting.Converters
{
    public class AgeColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Patient p)
            {
                int age = DateTime.Today.Year - p.BirthDate.Year;
                if (p.BirthDate > DateTime.Today.AddYears(-age)) age--;

                return age < 18 ? Colors.LightPink : Colors.Transparent;
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
