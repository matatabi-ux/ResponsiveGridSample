﻿#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveGridSample.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Practices.Prism.StoreApps;
    using Microsoft.Practices.Prism.StoreApps.Interfaces;
    using ResponsiveGridSample.Services;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.ApplicationModel.Resources;
#if WINDOWS_APP
    using Windows.UI.ApplicationSettings;
#endif
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Animation;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// MVPVM アプリケーション抽象クラス
    /// </summary>
    public abstract class MvpvmAppBase : Application
    {
        #region Privates

        /// <summary>
        /// Terminated からの復帰フラグ
        /// </summary>
        private bool isRestoringFromTermination;

#if WINDOWS_PHONE_APP
        /// <summary>
        /// 遷移効果
        /// </summary>
        private TransitionCollection transitions;
#endif
        #endregion //Privates

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected MvpvmAppBase()
        {
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// セッション状態
        /// </summary>
        protected ISessionStateService SessionStateService { get; set; }

        /// <summary>
        /// 画面遷移サービス
        /// </summary>
        public static INavigationService NavigationService { get; set; }

        /// <summary>
        /// 拡張スプラッシュ画面生成クラス
        /// </summary>
        protected Func<SplashScreen, Page> ExtendedSplashScreenFactory { get; set; }

        /// <summary>
        /// 中断状態フラグ
        /// </summary>
        public bool IsSuspending { get; private set; }

        /// <summary>
        /// アプリケーション起動処理
        /// </summary>
        /// <param name="args"><see cref="LaunchActivatedEventArgs"/> の起動イベント引数</param>
        /// <returns>Task</returns>
        protected abstract Task OnLaunchApplication(LaunchActivatedEventArgs args);

        /// <summary>
        /// 画面トークンから画面クラスを取得する
        /// </summary>
        /// <param name="pageToken">画面トークン</param>
        /// <returns>トークンが示す画面クラスの型</returns>
        protected virtual Type GetPageType(string pageToken)
        {
            var assemblyQualifiedAppType = this.GetType().GetTypeInfo().AssemblyQualifiedName;

            var pageNameWithParameter = assemblyQualifiedAppType.Replace(this.GetType().FullName, this.GetType().Namespace + ".Views.{0}Page");

            var viewFullName = string.Format(CultureInfo.InvariantCulture, pageNameWithParameter, pageToken);
            var viewType = Type.GetType(viewFullName);

            if (viewType == null)
            {
                var resourceLoader = ResourceLoader.GetForCurrentView(Constants.StoreAppsInfrastructureResourceMapId);
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, resourceLoader.GetString("DefaultPageTypeLookupErrorMessage"), pageToken, this.GetType().Namespace + ".Views"),
                    "pageToken");
            }

            return viewType;
        }

        /// <summary>
        /// シリアライズ用の KnownTypes の登録処理
        /// </summary>
        protected virtual void OnRegisterKnownTypesForSerialization()
        { 
        }

        /// <summary>
        /// アプリケーション初期化処理
        /// </summary>
        /// <param name="args">The <see cref="IActivatedEventArgs"/> のイベント引数</param>
        /// <returns>Task</returns>
        protected abstract Task OnInitializeAsync(IActivatedEventArgs args);

        /// <summary>
        /// 指定したクラスをインスタンス化する
        /// </summary>
        /// <param name="type">クラスの型</param>
        /// <returns>インスタンス化した指定クラス</returns>
        protected virtual object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// ユーザーによって通常起動された場合のアプリケーション起動処理
        /// ファイルアクティベーションやプロトコルアクティベーションなどは含まない
        /// </summary>
        /// <param name="args">起動要求と処理の詳細情報</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            var rootFrame = await this.InitializeFrameAsync(args);

            // プライマリタイルから起動された場合、TileId と アプリケーションIDが一致するので確認する
            // 参考 http://go.microsoft.com/fwlink/?LinkID=288842
            string tileId = AppManifestHelper.GetApplicationId();

            if (rootFrame != null && (!this.isRestoringFromTermination || (args != null && args.TileId != tileId)))
            {
                await this.OnLaunchApplication(args);
            }

            // 現在のウィンドウがアクティブであることを確認
            Window.Current.Activate();
        }

        /// <summary>
        /// Frame と Content を初期化する
        /// </summary>
        /// <param name="args"><see cref="IActivatedEventArgs"/> のイベント引数</param>
        /// <returns>Task</returns>
        protected async Task<Frame> InitializeFrameAsync(IActivatedEventArgs args)
        {
            var rootFrame = Window.Current.Content as Frame;

            // ウィンドウアクティブ化の際にすでに Content が含まれる場合は初期化しない
            if (rootFrame == null)
            {
                // 初期画面の生成と遷移のため Frame を生成する
                rootFrame = new Frame();

                if (this.ExtendedSplashScreenFactory != null)
                {
                    Page extendedSplashScreen = this.ExtendedSplashScreenFactory.Invoke(args.SplashScreen);
                    rootFrame.Content = extendedSplashScreen;
                }

                var frameFacade = new FrameFacadeAdapter(rootFrame);

                // セッション状態サービスを初期化する
                this.SessionStateService = new SessionStateService();

                // VisualStateAwarePage がセッション状態を取得できるように設定
                VisualStateAwarePage.GetSessionStateForFrame =
                    frame => this.SessionStateService.GetSessionStateForFrame(frameFacade);

                // Frame にキーを関連付ける
                this.SessionStateService.RegisterFrame(frameFacade, "AppFrame");

                NavigationService = this.CreateNavigationService(frameFacade, this.SessionStateService);
#if WINDOWS_APP
                SettingsPane.GetForCurrentView().CommandsRequested += this.OnCommandsRequested;
#endif
                // ViewModelLocator の名前解決メソッドを設定する
                ViewModelLocator.SetDefaultViewModelFactory(this.Resolve);

                this.OnRegisterKnownTypesForSerialization();
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    await this.SessionStateService.RestoreSessionStateAsync();
                }

                await this.OnInitializeAsync(args);
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // 必要な場合のみ、保存されたセッション状態を復元
                    try
                    {
                        this.SessionStateService.RestoreFrameState();
                        NavigationService.RestoreSavedNavigation();
                        this.isRestoringFromTermination = true;
                    }
                    catch (SessionStateServiceException)
                    {
                        //状態の復元に何か問題があった場合
                        //状態がないものとして続行する
                    }
                }

#if WINDOWS_PHONE_APP
                // スタートアップのターンスタイル ナビゲーションを削除します。
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.OnPhoneFirstNavigated;
#endif
                // Frame を現在のウィンドウに配置
                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// アプリを起動した後のコンテンツの移行を復元します。
        /// </summary>
        /// <param name="sender">ハンドラーがアタッチされたオブジェクト。</param>
        /// <param name="e">ナビゲーション イベントの詳細。</param>
        private void OnPhoneFirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.OnPhoneFirstNavigated;
        }
#endif
        /// <summary>
        /// 画面遷移サービスを生成する
        /// </summary>
        /// <param name="rootFrame">Frame</param>
        /// <param name="sessionStateService">セッション状態サービス</param>
        /// <returns>初期化後の画面遷移サービス</returns>
        private INavigationService CreateNavigationService(IFrameFacade rootFrame, ISessionStateService sessionStateService)
        {
            var navigationService = new PageNavigationService(rootFrame, this.GetPageType, sessionStateService);
            return navigationService;
        }

        /// <summary>
        /// アプリケーションの実行が中断されたときに呼び出されます。アプリケーションの状態は、
        /// アプリケーションが終了されるのか、メモリの内容がそのままで再開されるのか
        /// わからない状態で保存されます。
        /// </summary>
        /// <param name="sender">中断要求の送信元。</param>
        /// <param name="e">中断要求の詳細。</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            this.IsSuspending = true;
            try
            {
                var deferral = e.SuspendingOperation.GetDeferral();

                //Bootstrap inform navigation service that app is suspending.
                NavigationService.Suspending();

                // アプリケーションの状態を保存
                await this.SessionStateService.SaveAsync();

                deferral.Complete();
            }
            finally
            {
                this.IsSuspending = false;
            }
        }

#if WINDOWS_APP
        /// <summary>
        /// 設定チャームのメニュー生成
        /// </summary>
        /// <returns>設定チャームに表示するコマンドのリスト</returns>
        protected virtual IList<SettingsCommand> GetSettingsCommands()
        {
            return new List<SettingsCommand>();
        }

        /// <summary>
        /// チャーム表示要求イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="args"><see cref="SettingsPaneCommandsRequestedEventArgs"/> のイベント引数</param>
        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            if (args == null || args.Request == null || args.Request.ApplicationCommands == null)
            {
                return;
            }

            var applicationCommands = args.Request.ApplicationCommands;
            var settingsCommands = this.GetSettingsCommands();

            foreach (var settingsCommand in settingsCommands)
            {
                applicationCommands.Add(settingsCommand);
            }
        }
#endif
    }
}
