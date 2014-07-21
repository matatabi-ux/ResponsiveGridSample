#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveGridSample
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Practices.Prism.StoreApps;
    using ResponsiveGridSample.Common;
    using ResponsiveGridSample.Models;
    using ResponsiveGridSample.ViewModels;
    using Windows.ApplicationModel.Activation;
    
    /// <summary>
    /// アプリケーション
    /// </summary>
    public sealed partial class App : MvpvmAppBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// アプリケーション設定情報リポジトリ
        /// </summary>
        public static ApplicationSettingsRepository AppSettings { get; set; }

        /// <summary>
        /// アプリケーション起動処理
        /// </summary>
        /// <param name="args"><see cref="LaunchActivatedEventArgs"/> の起動イベント引数</param>
        /// <returns>Task</returns>
        protected override Task OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Top", null);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 中断時に復元用に退避する ViewModel を登録する
        /// </summary>
        protected override void OnRegisterKnownTypesForSerialization()
        {
            // セッションデータに保存する可能性のある ViewModel をすべて登録する
            this.SessionStateService.RegisterKnownType(typeof(PhotoViewModel));
            this.SessionStateService.RegisterKnownType(typeof(ItemContainerViewModel));
            this.SessionStateService.RegisterKnownType(typeof(GroupContainerViewModel));
            this.SessionStateService.RegisterKnownType(typeof(ObservableCollection<ItemContainerViewModel>));
            this.SessionStateService.RegisterKnownType(typeof(ObservableCollection<GroupContainerViewModel>));
        }

        /// <summary>
        /// アプリケーション初期化処理
        /// </summary>
        /// <param name="args"><see cref="IActivatedEventArgs"/> のイベント引数</param>
        /// <returns>Task</returns>
        protected async override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            AppSettings = new ApplicationSettingsRepository();
            await AppSettings.LoadAsync();

            // View から対応する ViewModel を取得するロジックを設定する
            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = typeof(ViewModelBase).GetTypeInfo().Assembly.FullName;
                var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);
                return Type.GetType(viewModelName);
            });

            // 明示的に ViewModel の生成ロジックを指定する場合はここに記載する
        }
    }
}