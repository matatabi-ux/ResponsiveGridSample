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

    /// <summary>
    /// トップ画面の ViewModel
    /// </summary>
    public partial class TopPageViewModel : ViewModelBase, ITopPageViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TopPageViewModel()
        {
            this.items = new ObservableCollection<ItemContainerViewModel>();
            this.horizontalScrollOffset = 0;
        }
    }
}
