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
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// 色コードを Color に変換するコンバータ
    /// </summary>
    public class CodeToColorConverter : IValueConverter
    {
        /// <summary>
        /// 色コード から Color に変換します
        /// </summary>
        /// <param name="value">色コード値</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">変換書式</param>
        /// <param name="language">言語カルチャ</param>
        /// <returns>Color</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Color color = Colors.Transparent;

            if (value is string)
            {
                try
                {
                    var colorString = (string)value;
                    color = ColorHelper.FromArgb(
                        byte.Parse(colorString.Substring(1, 2), NumberStyles.HexNumber),
                        byte.Parse(colorString.Substring(3, 2), NumberStyles.HexNumber),
                        byte.Parse(colorString.Substring(5, 2), NumberStyles.HexNumber),
                        byte.Parse(colorString.Substring(7, 2), NumberStyles.HexNumber));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return color;
        }

        /// <summary>
        /// Color から 色コード に変換します
        /// </summary>
        /// <param name="value">Color</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">変換書式</param>
        /// <param name="language">言語カルチャ</param>
        /// <returns>色コード値</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var color = Colors.Transparent;

            if (value is Color)
            {
                color = (Color)value;
            }
            var colorString = string.Format("#{0:X2}{1:X2}{2:X2}", color.A, color.R, color.G, color.B);

            return colorString;
        }
    }
}
