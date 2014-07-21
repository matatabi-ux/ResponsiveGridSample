using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ResponsiveGridSample.Controls
{
    /// <summary>
    /// 繰り返し画像を表示する Canvas
    /// </summary>
    public class TiledCanvas : Canvas
    {
        /// <summary>
        /// 画像ソースの依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource",
            typeof(ImageSource),
            typeof(TiledCanvas),
            new PropertyMetadata(null, OnImageSourceChanged));

        /// <summary>
        /// 画像ソース変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="args">イベント引数</param>
        private static void OnImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var panel = (TiledCanvas)sender;
            if (panel.ImageSource == null)
            {
                return;
            }

            panel.image = new Image
            {
                Source = panel.ImageSource,
                UseLayoutRounding = false,
                Stretch = Stretch.None
            };
            panel.image.ImageOpened += panel.OnImageOpened;
            panel.image.ImageFailed += panel.OnImageFailed;

            // ImageOpend イベントを起こすため、ビジュアルツリーに画像を追加する
            panel.Children.Add(panel.image);
        }

        /// <summary>
        /// 画像ソース
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)this.GetValue(ImageSourceProperty); }
            set { this.SetValue(ImageSourceProperty, value); }
        }

        /// <summary>
        /// 直近のサイズ
        /// </summary>
        private Size lastActualSize;

        /// <summary>
        /// 画像
        /// </summary>
        private Image image;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TiledCanvas()
        {
            this.LayoutUpdated += this.OnLayoutUpdated;
            this.Unloaded += this.OnUnloaded;
        }

        /// <summary>
        /// 破棄イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.LayoutUpdated -= this.OnLayoutUpdated;
            this.Unloaded -= this.OnUnloaded;
            if (this.image == null)
            {
                return;
            }
            this.image.ImageFailed -= this.OnImageFailed;
            this.image.ImageOpened -= this.OnImageOpened;
            this.image = null;
        }

        /// <summary>
        /// レイアウト更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="args">イベント引数</param>
        private void OnLayoutUpdated(object sender, object args)
        {
            var newSize = new Size(this.ActualWidth, this.ActualHeight);
            if (this.lastActualSize.Equals(newSize))
            {
                return;
            }

            this.lastActualSize = newSize;
            this.Render();
        }

        /// <summary>
        /// 画像読み込み失敗イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="args">イベント引数</param>
        private void OnImageFailed(object sender, ExceptionRoutedEventArgs args)
        {
            this.image.ImageOpened -= this.OnImageOpened;
            this.image.ImageFailed -= this.OnImageFailed;
            this.image = null;
            this.Children.Clear();
        }

        /// <summary>
        /// 画像読み込み完了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="args">イベント引数</param>
        private void OnImageOpened(object sender, RoutedEventArgs args)
        {
            this.image.ImageOpened -= this.OnImageOpened;
            this.image.ImageFailed -= this.OnImageFailed;
            this.image = null;
            this.Render();
        }

        /// <summary>
        /// 繰り返し画像をレンダリングする
        /// </summary>
        private void Render()
        {
            var bmp = ImageSource as BitmapSource;
            if (bmp == null)
            {
                return;
            }

            var width = bmp.PixelWidth;
            var height = bmp.PixelHeight;

            if (width == 0 || height == 0)
            {
                return;
            }

            this.Children.Clear();

            // スケーリング状態に応じて縦横の幅を調整
            var scaleing = 1.0d;
            switch (DisplayInformation.GetForCurrentView().ResolutionScale)
            {
                case ResolutionScale.Scale100Percent:
                case ResolutionScale.Scale120Percent:
                    scaleing = 1.0d;
                    break;

                case ResolutionScale.Scale140Percent:
                case ResolutionScale.Scale150Percent:
                case ResolutionScale.Scale160Percent:
                    scaleing = 1.4d;
                    break;

                case ResolutionScale.Scale180Percent:
                case ResolutionScale.Scale225Percent:
                    scaleing = 1.8d;
                    break;
            }

            width = (int)Math.Floor(width / scaleing);
            height = (int)Math.Floor(height / scaleing);

            // 画像を敷き詰める
            for (double x = 0; x < this.ActualWidth; x += width)
            {
                for (double y = 0; y < this.ActualHeight; y += height)
                {
                    var image = new Image
                    {
                        Source = this.ImageSource,
                        UseLayoutRounding = false,
                        Stretch = Stretch.None
                    };
                    Canvas.SetLeft(image, x);
                    Canvas.SetTop(image, y);
                    this.Children.Add(image);
                }
            }

            // はみ出した部分をクリッピングする
            this.Clip = new RectangleGeometry 
            {
                Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight)
            };
        }

    }
}
