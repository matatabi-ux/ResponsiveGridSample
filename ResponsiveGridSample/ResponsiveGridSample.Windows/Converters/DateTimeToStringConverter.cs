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
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// DateTime を指定書式の文字列に変換するコンバータ
    /// </summary>
    public class DateTimeToStringConverter : IValueConverter
    {
        /// <summary>
        /// DateTime から string に変換します
        /// </summary>
        /// <param name="value">DateTime値</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">変換書式</param>
        /// <param name="language">言語カルチャ</param>
        /// <returns>日時文字列</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime? datetime = null;

            if (value is DateTime)
            {
                datetime = (DateTime)value;
            }
            else if (value is DateTime?)
            {
                datetime = (DateTime?)value;
            }

            if (datetime.HasValue)
            {
                if (parameter is string)
                {
                    return datetime.Value.ToString((string)parameter);
                }
                else
                {
                    return datetime.Value.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// string から DateTime に変換します
        /// </summary>
        /// <param name="value">string値</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">反転フラグ</param>
        /// <param name="language">言語カルチャ</param>
        /// <returns>DateTime値</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
