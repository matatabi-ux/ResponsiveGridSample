#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveGridSample.Behaviors
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Xaml.Interactivity;
    using ResponsiveGridSample.ViewModels;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// 内部の ScrollViewer にバインド可能にするビヘイビア
    /// </summary>
    public class ScrollViewerBindableBehavior : DependencyObject, IBehavior
    {
        #region HorizonalOffset 依存関係プロパティ
        /// <summary>
        /// 水平スクロール位置 依存関係プロパティ
        /// </summary>
        private static readonly DependencyProperty HorizontalOffsetOffsetProperty
            = DependencyProperty.Register(
            "HorizontalOffset",
            typeof(double),
            typeof(ScrollViewerBindableBehavior),
            new PropertyMetadata(
                0d,
                (s, e) =>
                {
                    var control = s as ScrollViewerBindableBehavior;
                    if (control != null)
                    {
                        control.OnHorizonalOffsetChanged();
                    }
                }));

        /// <summary>
        /// 水平スクロール位置 変更イベントハンドラ
        /// </summary>
        private void OnHorizonalOffsetChanged()
        {
            if (this.ScrollViewer != null)
            {
                this.ScrollViewer.ChangeView(this.HorizontalOffset, null, null, false);
            }
        }

        /// <summary>
        /// 水平スクロール位置
        /// </summary>
        public double HorizontalOffset
        {
            get { return (double)this.GetValue(HorizontalOffsetOffsetProperty); }
            set { this.SetValue(HorizontalOffsetOffsetProperty, value); }
        }
        #endregion //HorizonalOffset 依存関係プロパティ

        #region VerticalOffset 依存関係プロパティ
        /// <summary>
        /// 垂直スクロール位置 依存関係プロパティ
        /// </summary>
        private static readonly DependencyProperty VerticalOffsetProperty
            = DependencyProperty.Register(
            "VerticalOffset",
            typeof(double),
            typeof(ScrollViewerBindableBehavior),
            new PropertyMetadata(
                0d,
                (s, e) =>
                {
                    var control = s as ScrollViewerBindableBehavior;
                    if (control != null)
                    {
                        control.OnVerticalOffsetChanged();
                    }
                }));

        /// <summary>
        /// 垂直スクロール位置 変更イベントハンドラ
        /// </summary>
        private void OnVerticalOffsetChanged()
        {
            if (this.ScrollViewer != null)
            {
                this.ScrollViewer.ChangeView(null, this.VerticalOffset, null, false);
            }
        }

        /// <summary>
        /// 垂直スクロール位置
        /// </summary>
        public double VerticalOffset
        {
            get { return (double)this.GetValue(VerticalOffsetProperty); }
            set { this.SetValue(VerticalOffsetProperty, value); }
        }
        #endregion //VerticalOffset 依存関係プロパティ

        #region ZoomFactor 依存関係プロパティ
        /// <summary>
        /// 拡大率 依存関係プロパティ
        /// </summary>
        private static readonly DependencyProperty ZoomFactorProperty
            = DependencyProperty.Register(
            "ZoomFactor",
            typeof(float),
            typeof(ScrollViewerBindableBehavior),
            new PropertyMetadata(
                1f,
                (s, e) =>
                {
                    var control = s as ScrollViewerBindableBehavior;
                    if (control != null)
                    {
                        control.OnZoomFactorChanged();
                    }
                }));

        /// <summary>
        /// 拡大率 変更イベントハンドラ
        /// </summary>
        private void OnZoomFactorChanged()
        {
            if (this.ScrollViewer != null)
            {
                this.ScrollViewer.ChangeView(null, null, this.ZoomFactor, false);
            }
        }

        /// <summary>
        /// 拡大率
        /// </summary>
        public float ZoomFactor
        {
            get { return (float)this.GetValue(ZoomFactorProperty); }
            set { this.SetValue(ZoomFactorProperty, value); }
        }
        #endregion //ZoomFactor 依存関係プロパティ

        #region OffsetItem 依存関係プロパティ
        /// <summary>
        /// スクロール位置アイテム 依存関係プロパティ
        /// </summary>
        private static readonly DependencyProperty OffsetItemProperty
            = DependencyProperty.Register(
            "OffsetItem",
            typeof(ViewModelBase),
            typeof(ScrollViewerBindableBehavior),
            new PropertyMetadata(
                null,
                (s, e) =>
                {
                    var control = s as ScrollViewerBindableBehavior;
                    if (control != null)
                    {
                        control.OnOffsetItemChanged();
                    }
                }));

        /// <summary>
        /// スクロール位置アイテム 変更イベントハンドラ
        /// </summary>
        private void OnOffsetItemChanged()
        {
        }

        /// <summary>
        /// スクロール位置アイテム
        /// </summary>
        public ViewModelBase OffsetItem
        {
            get { return (ViewModelBase)this.GetValue(OffsetItemProperty); }
            set { this.SetValue(OffsetItemProperty, value); }
        }
        #endregion //OffsetItem 依存関係プロパティ

        /// <summary>
        /// アタッチ対象のオブジェクト
        /// </summary>
        public DependencyObject AssociatedObject { get; set; }

        /// <summary>
        /// ScrollViewer
        /// </summary>
        public ScrollViewer ScrollViewer { get; set; }

        /// <summary>
        /// アタッチする
        /// </summary>
        /// <param name="associatedObject">アタッチ対象オブジェクト</param>
        public void Attach(DependencyObject associatedObject)
        {
            this.AssociatedObject = associatedObject;
            if (associatedObject is FrameworkElement)
            {
                ((FrameworkElement)associatedObject).SizeChanged += this.OnSizeChanged;
            }
        }

        /// <summary>
        /// 表示完了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.OffsetItem != null && Window.Current.Content is Frame && ((Frame)Window.Current.Content).Content is Page)
            {
                ((Page)((Frame)Window.Current.Content).Content).Loaded += this.OnPageLoaded;
                return;
            }

            this.ScrollViewer = FindChild<ScrollViewer>(this.AssociatedObject);
            if (this.ScrollViewer != null)
            {
                this.ScrollViewer.ChangeView(this.HorizontalOffset, this.VerticalOffset, this.ZoomFactor, false);
                this.ScrollViewer.ViewChanged += this.OnViewChanged;
            }
        }

        /// <summary>
        /// 画面読み込み完了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            ((Page)((Frame)Window.Current.Content).Content).Loaded -= this.OnPageLoaded;

            /*
            if (this.OffsetItem != null)
            {
                var listViewBase = this.AssociatedObject as ListViewBase;
                if (listViewBase != null)
                {
                    var item = listViewBase.Items.FirstOrDefault(
                        i => ((ItemContainerViewModel)this.OffsetItem).ContentId.Equals(
                            ((ItemContainerViewModel)i).ContentId));
                    if (item == null)
                    {
                        return;
                    }
                    listViewBase.ScrollIntoView(item, ScrollIntoViewAlignment.Leading);
                }
            }/* */
        }

        /// <summary>
        /// スクロールおよびズーム変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (this.ScrollViewer != null)
            {
                this.HorizontalOffset = this.ScrollViewer.HorizontalOffset;
                this.VerticalOffset = this.ScrollViewer.VerticalOffset;
                this.ZoomFactor = this.ScrollViewer.ZoomFactor;
            }
        }

        /// <summary>
        /// デタッチする
        /// </summary>
        public void Detach()
        {
            if (this.AssociatedObject is FrameworkElement)
            {
                ((FrameworkElement)this.AssociatedObject).SizeChanged -= this.OnSizeChanged;
            }
            if (Window.Current.Content is Frame && ((Frame)Window.Current.Content).Content is Page)
            {
                ((Page)((Frame)Window.Current.Content).Content).Loaded -= this.OnPageLoaded;
            }
            if (this.ScrollViewer != null)
            {
                this.ScrollViewer.ViewChanged -= this.OnViewChanged;
            }
            this.AssociatedObject = null;
            this.ScrollViewer = null;
        }

        /// <summary>
        /// 指定した型の最初に見つかったビジュアル要素を返す
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="root">探索対象のビジュアル要素</param>
        /// <param name="name">対象のビジュアル要素名</param>
        /// <returns>見つかった場合はその要素</returns>
        private static T FindChild<T>(DependencyObject root, string name = null) where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                if (child != null && child is T)
                {
                    if (child is FrameworkElement && (string.IsNullOrEmpty(name) || ((FrameworkElement)child).Name == name))
                    {
                        return (T)child;
                    }
                    else
                    {
                        T childOfChild = FindChild<T>(child, name);

                        if (childOfChild != null)
                        {
                            return childOfChild;
                        }
                    }
                }
                else
                {
                    T childOfChild = FindChild<T>(child, name);

                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
    }
}