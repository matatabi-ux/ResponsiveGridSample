﻿#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveGridSample.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// bool から Visibility に変換するコンバータ
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// bool から Visibility に変換します
        /// </summary>
        /// <param name="value">bool値</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">反転フラグ</param>
        /// <param name="language">言語カルチャ</param>
        /// <returns>Visibility値</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var flag = false;
            bool.TryParse(parameter as string, out flag);

            if (!flag)
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// Visibility から bool に変換します
        /// </summary>
        /// <param name="value">Visibility値</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">反転フラグ</param>
        /// <param name="language">言語カルチャ</param>
        /// <returns>Visibility値</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var flag = false;
            bool.TryParse(parameter as string, out flag);

            var visibility = (Visibility)value;

            if (!flag)
            {
                return visibility.Equals(Visibility.Visible);
            }
            else
            {
                return visibility.Equals(Visibility.Collapsed);
            }
        }
    }
}
