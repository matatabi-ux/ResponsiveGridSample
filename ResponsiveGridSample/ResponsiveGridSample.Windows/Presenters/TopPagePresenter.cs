#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveGridSample.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ResponsiveGridSample.ViewModels;
    using ResponsiveGridSample.Views;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;
    using Windows.UI.Xaml;

    /// <summary>
    /// トップ画面の Presenter
    /// </summary>
    public class TopPagePresenter : PresenterBase, IPresenter<TopPage, TopPageViewModel>
    {
        #region IPresenter<View, ViewModel>

        /// <summary>
        /// View
        /// </summary>
        public TopPage View
        {
            get { return this.PresenterView as TopPage; }
            set { this.PresenterView = value; }
        }

        /// <summary>
        /// ViewModel
        /// </summary>
        public TopPageViewModel ViewModel
        {
            get { return this.PresenterViewModel as TopPageViewModel; }
            set { this.PresenterViewModel = value; }
        }

        #endregion //IPresenter<View, ViewModel>

        /// <summary>
        /// 画面に遷移したときの処理
        /// </summary>
        /// <param name="navigationParameter">遷移パラメータ</param>
        /// <param name="navigationMode">遷移モード</param>
        /// <param name="viewModelState">画面状態データ</param>
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            if (this.ViewModel.Items.Count > 0)
            {
                return;
            }

            foreach (var photo in App.AppSettings.Settings.Items)
            {
                var item = PhotoViewModel.Convert(photo);

                if (this.ViewModel.Items.Count > 0)
                {

                    this.ViewModel.Items.Add(
                        new ItemContainerViewModel()
                        {
                            UniqueId = Guid.NewGuid().ToString(),
                            ContentId = item.UniqueId,
                            Content = item,
                            ColumnSpan = 1,
                            RowSpan = 1,
                            IsActive = true,
                        });
                }
                else
                {

                    this.ViewModel.Items.Add(
                        new ItemContainerViewModel()
                        {
                            UniqueId = Guid.NewGuid().ToString(),
                            ContentId = item.UniqueId,
                            Content = item,
                            ColumnSpan = 1,
                            RowSpan = 2,
                            IsActive = true,
                        });
                }
            }
        }

        /// <summary>
        /// GridView 内部の VariableSizedWrapGrid のサイズ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        public void OnVariableSizedWrapGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var wrapGrid = sender as VariableSizedWrapGrid;
            if (wrapGrid == null)
            {
                return;
            }

            switch (wrapGrid.Orientation)
            {
                // Z字型の並び順の場合
                case Orientation.Horizontal:
                    var maxColumn = (int)Math.Floor(e.NewSize.Width / 350);

                    // 列数が3列未満になる場合はタイルを小さくする
                    if (maxColumn < 3)
                    {
                        wrapGrid.ItemWidth = 250;
                        wrapGrid.ItemHeight = 300;
                        maxColumn = (int)Math.Floor(e.NewSize.Width / wrapGrid.ItemWidth);
                    }
                    else
                    {
                        wrapGrid.ItemWidth = 350;
                        wrapGrid.ItemHeight = 400;
                    }
                    wrapGrid.MaximumRowsOrColumns = maxColumn;
                    break;

                // И字型の並び順の場合
                case Orientation.Vertical:
                    var maxRow = (int)Math.Floor(e.NewSize.Height / 400);

                    // 行数が2列未満になる場合はタイルを小さくする
                    if (maxRow < 2)
                    {
                        wrapGrid.ItemWidth = 250;
                        wrapGrid.ItemHeight = 300;
                        maxRow = (int)Math.Floor(e.NewSize.Height / wrapGrid.ItemHeight);
                    }
                    else
                    {
                        wrapGrid.ItemWidth = 350;
                        wrapGrid.ItemHeight = 400;
                    }
                    wrapGrid.MaximumRowsOrColumns = maxRow;
                    break;
            }
        }
    }
}