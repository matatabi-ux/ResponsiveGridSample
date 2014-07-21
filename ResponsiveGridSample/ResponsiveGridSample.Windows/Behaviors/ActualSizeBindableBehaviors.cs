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
    using Microsoft.Xaml.Interactivity;
    using ResponsiveGridSample.Common;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// Actual Width/height をバインド可能にするビヘイビア
    /// </summary>
    public class ActualSizeBindableBehaviors : DependencyObject, IBehavior
    {
        #region ActualWidth 依存関係プロパティ
        /// <summary>
        /// ActualWidth 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ActualWidthProperty
            = DependencyProperty.Register(
            "ActualWidth",
            typeof(double),
            typeof(ActualSizeBindableBehaviors),
            new PropertyMetadata(
                0d,
                (s, e) =>
                {
                    var control = s as ActualSizeBindableBehaviors;
                    if (control != null)
                    {
                        control.OnActualWidthChanged();
                    }
                }));

        /// <summary>
        /// ActualWidth 変更イベントハンドラ
        /// </summary>
        private void OnActualWidthChanged()
        {
        }

        /// <summary>
        /// ActualWidth
        /// </summary>
        public double ActualWidth
        {
            get { return (double)this.GetValue(ActualWidthProperty); }
            set { this.SetValue(ActualWidthProperty, value); }
        }
        #endregion //ActualWidth 依存関係プロパティ

        #region ActualHeight 依存関係プロパティ
        /// <summary>
        /// ActualHeight 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ActualHeightProperty
            = DependencyProperty.Register(
            "ActualHeight",
            typeof(double),
            typeof(ActualSizeBindableBehaviors),
            new PropertyMetadata(
                0d,
                (s, e) =>
                {
                    var control = s as ActualSizeBindableBehaviors;
                    if (control != null)
                    {
                        control.OnActualHeightChanged();
                    }
                }));

        /// <summary>
        /// ActualHeight 変更イベントハンドラ
        /// </summary>
        private void OnActualHeightChanged()
        {
        }

        /// <summary>
        /// ActualHeight
        /// </summary>
        public double ActualHeight
        {
            get { return (double)this.GetValue(ActualHeightProperty); }
            set { this.SetValue(ActualHeightProperty, value); }
        }
        #endregion //ActualHeight 依存関係プロパティ

        /// <summary>
        /// アタッチ対象のオブジェクト
        /// </summary>
        public DependencyObject AssociatedObject { get; set; }

        /// <summary>
        /// Visibility 変更監視オブジェクト
        /// </summary>
        private DependencyPropertyChangedHelper visibilityWatcher;

        /// <summary>
        /// アタッチする
        /// </summary>
        /// <param name="associatedObject">アタッチ対象オブジェクト</param>
        public void Attach(DependencyObject associatedObject)
        {
            this.AssociatedObject = associatedObject as FrameworkElement;
            if (this.AssociatedObject == null)
            {
                return;
            }
            ((FrameworkElement)associatedObject).SizeChanged += this.OnSizeChanged;
            this.visibilityWatcher = new DependencyPropertyChangedHelper(associatedObject as FrameworkElement, "Visibility");
            this.visibilityWatcher.PropertyChanged += this.OnVisibilityChanged;
        }

        /// <summary>
        /// 表示完了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.ActualWidth = ((FrameworkElement)this.AssociatedObject).ActualWidth;
            this.ActualHeight = ((FrameworkElement)this.AssociatedObject).ActualHeight;
        }

        /// <summary>
        /// Visibility 変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(this.AssociatedObject is FrameworkElement))
            {
                return;
            }

            if (((FrameworkElement)this.AssociatedObject).Visibility.Equals(Visibility.Collapsed))
            {
                this.ActualWidth = 0;
                this.ActualHeight = 0;
            }
            else
            {
                this.ActualWidth = ((FrameworkElement)this.AssociatedObject).ActualWidth;
                this.ActualHeight = ((FrameworkElement)this.AssociatedObject).ActualHeight;
            }
        }

        /// <summary>
        /// デタッチする
        /// </summary>
        public void Detach()
        {
            if (this.AssociatedObject is FrameworkElement)
            {
                ((FrameworkElement)this.AssociatedObject).SizeChanged += this.OnSizeChanged;
            }
            this.AssociatedObject = null;
        }
    }
}
