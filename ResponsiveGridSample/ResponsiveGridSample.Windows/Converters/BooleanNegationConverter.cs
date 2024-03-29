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
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// true を false に、および false を true に変換する値コンバーター。
    /// </summary>
    public sealed class BooleanNegationConverter : IValueConverter
    {
        /// <summary>
        /// bool を反転します
        /// </summary>
        /// <param name="value">bool値</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">パラメータ</param>
        /// <param name="language">言語カルチャ</param>
        /// <returns>bool値</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }

        /// <summary>
        /// bool を反転します
        /// </summary>
        /// <param name="value">bool値</param>
        /// <param name="targetType">変換後の型</param>
        /// <param name="parameter">パラメータ</param>
        /// <param name="language">言語カルチャ</param>
        /// <returns>bool値</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }
    }
}
