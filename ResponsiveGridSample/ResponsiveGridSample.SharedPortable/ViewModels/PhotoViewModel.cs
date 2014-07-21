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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using ResponsiveGridSample.Models;

    /// <summary>
    /// 写真情報の ViewModel
    /// </summary>
    public partial class PhotoViewModel : ViewModelBase, IPhotoViewModel
    {
        /// <summary>
        /// Model を ViewModel に変換する
        /// </summary>
        /// <param name="model">変換元の Model</param>
        /// <returns>ViewModel</returns>
        public static PhotoViewModel Convert(Photo model)
        {
            return new PhotoViewModel()
            {
                UniqueId = model.UniqueId,
                ImageUri = model.ImageUri,
                Title = model.Title,
                Owner = model.Owner,
            };
        }
    }
}
