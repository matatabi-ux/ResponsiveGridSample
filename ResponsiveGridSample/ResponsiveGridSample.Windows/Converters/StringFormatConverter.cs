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
    /// 指定書式の文字列に変換するコンバーター
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        /// <summary>
        /// string に変換します
        /// </summary>
        /// <param name="value">変換元パラメータ値</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">変換書式</param>
        /// <param name="language">言語カルチャ</param>
        /// <returns>文字列</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter is string)
            {
                return string.Format((string)parameter, value);
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// string から 指定書式 に変換します
        /// </summary>
        /// <param name="value">string値</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">反転フラグ</param>
        /// <param name="language">言語カルチャ</param>
        /// <returns>指定書式</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
