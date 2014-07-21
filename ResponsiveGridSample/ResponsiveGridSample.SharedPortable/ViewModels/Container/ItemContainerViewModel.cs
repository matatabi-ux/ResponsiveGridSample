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
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// ViewModel
    /// </summary>
    public partial class ItemContainerViewModel : ViewModelBase, IItemContainerViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ItemContainerViewModel()
        {
        }

        /// <summary>
        /// コンテンツの ViewModel の型
        /// </summary>
        public Type ContentType
        {
            get { return this.content.GetType(); }
        }
    }
}
