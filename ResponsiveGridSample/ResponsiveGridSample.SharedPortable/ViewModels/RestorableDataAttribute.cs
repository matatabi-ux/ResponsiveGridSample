#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveGridSample.ViewModels
{
    using System;

    /// <summary>
    /// Terminate から復帰時に復元されるプロパティに付加する属性
    /// </summary>
    [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class RestorableDataAttribute : Attribute
    {
    }
}
