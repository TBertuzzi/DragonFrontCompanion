﻿using DragonFrontDb;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DragonFrontCompanion.Helpers
{
    public class HealthIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var card = value as Card;
            if (card != null && 
                card.Traits != null && 
                card.Traits.Contains(DragonFrontDb.Enums.Traits.ARMOR.ToString()))
            {
                return "IconArmor.png";
            }
            return "IconHealth.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
