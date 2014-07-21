#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveGridSample.Views
{
    using Microsoft.Practices.Prism.StoreApps;
    using ResponsiveGridSample.Presenters;

    /// <summary>
    /// トップ画面
    /// </summary>
    [PresenterView(typeof(TopPagePresenter))]
    public sealed partial class TopPage : VisualStateAwarePage, IPresenterView<TopPagePresenter>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TopPage()
        {
            this.InitializeComponent();
        }

        #region IPresenterView<TPresenter>

        /// <summary>
        /// この画面の Presenter
        /// </summary>
        public TopPagePresenter Presenter
        {
            get { return this.GetPresenter<TopPagePresenter>(); }
        }

        #endregion //IPresenterView<TPresenter>
    }
}
