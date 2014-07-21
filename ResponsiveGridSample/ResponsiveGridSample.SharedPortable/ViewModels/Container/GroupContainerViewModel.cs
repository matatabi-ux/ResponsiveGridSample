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
    /// グループコンテナ ViewModel
    /// </summary>
    public partial class GroupContainerViewModel : ViewModelBase, IGroupContainerViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GroupContainerViewModel()
        {
            this.Items = new ObservableCollection<ItemContainerViewModel>();
        }

        /// <summary>
        /// コンテンツの ViewModel の型
        /// </summary>
        public Type ContentType
        {
            get { return this.content.GetType(); }
        }

        #region Count:件数 プロパティ

        /// <summary>
        /// 件数
        /// </summary>
        public int Count
        {
            get
            {
                return this.items == null ? 0 : this.items.Count;
            }
        }

        #endregion //Count:件数 プロパティ
    }
}
