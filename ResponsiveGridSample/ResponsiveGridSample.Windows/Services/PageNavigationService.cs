﻿#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveGridSample.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Practices.Prism.StoreApps.Interfaces;
    using Microsoft.Practices.Unity;
    using ResponsiveGridSample.Presenters;
    using ResponsiveGridSample.ViewModels;
    using ResponsiveGridSample.Views;
    using Windows.ApplicationModel.Resources;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;
    using Microsoft.Practices.Prism.StoreApps;

    /// <summary>
    /// 画面遷移サービス
    /// </summary>
    public class PageNavigationService : INavigationService
    {
        #region Privates

        /// <summary>
        /// 最終遷移先パラメータキー
        /// </summary>
        private const string LastNavigationParameterKey = "LastNavigationParameter";

        /// <summary>
        /// 最終遷移先画面キー
        /// </summary>
        private const string LastNavigationPageKey = "LastNavigationPageKey";

        /// <summary>
        /// Frame ファサード
        /// </summary>
        private readonly IFrameFacade frame;

        /// <summary>
        /// 遷移先解決処理
        /// </summary>
        private readonly Func<string, Type> navigationResolver;

        /// <summary>
        /// セッション管理サービス
        /// </summary>
        private readonly ISessionStateService sessionStateService;

        #endregion //Privates

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="frame">Frame</param>
        /// <param name="navigationResolver">遷移先解決処理</param>
        /// <param name="sessionStateService">セッション管理サービス</param>
        public PageNavigationService(IFrameFacade frame, Func<string, Type> navigationResolver, ISessionStateService sessionStateService)
        {
            this.frame = frame;
            this.navigationResolver = navigationResolver;
            this.sessionStateService = sessionStateService;

            if (frame == null)
            {
                return;
            }

#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += this.OnHardwareButtonsBackPressed;
#endif
            this.frame.Navigating += this.OnNavigating;
            this.frame.Navigated += this.OnNavigated;
        }

        /// <summary>
        /// 指定画面に遷移する
        /// </summary>
        /// <param name="pageToken">画面トークン</param>
        /// <param name="parameter">遷移パラメータ</param>
        /// <returns>成功した場合 <c>true</c>、それ以外は <c>false</c></returns>
        public bool Navigate(string pageToken, object parameter)
        {
            Type pageType = this.navigationResolver(pageToken);

            if (pageType == null)
            {
                var resourceLoader = ResourceLoader.GetForCurrentView(Constants.StoreAppsInfrastructureResourceMapId);
                var error = string.Format(CultureInfo.CurrentCulture, resourceLoader.GetString("FrameNavigationServiceUnableResolveMessage"), pageToken);
                throw new ArgumentException(error, "pageToken");
            }

            // 全く同じ遷移でないか確認するため画面の型とパラメータを取得する
            var lastNavigationParameter = this.sessionStateService.SessionState.ContainsKey(LastNavigationParameterKey) ? this.sessionStateService.SessionState[LastNavigationParameterKey] : null;
            var lastPageTypeFullName = this.sessionStateService.SessionState.ContainsKey(LastNavigationPageKey) ? this.sessionStateService.SessionState[LastNavigationPageKey] as string : string.Empty;

            if (lastPageTypeFullName != pageType.FullName || !AreEquals(lastNavigationParameter, parameter))
            {
                return this.frame.Navigate(pageType, parameter);
            }

            return false;
        }

        /// <summary>
        /// 戻り遷移する
        /// </summary>
        public void GoBack()
        {
            this.frame.GoBack();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// 戻るボタン押下イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnHardwareButtonsBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (this.CanGoBack())
            {
                e.Handled = true;
                this.GoBack();
            }
        }
#endif

        /// <summary>
        /// 戻り遷移できるかどうか取得する
        /// </summary>
        /// <returns>遷移できる場合 <c>true</c>、それ以外は <c>false</c></returns>
        public bool CanGoBack()
        {
            return this.frame.CanGoBack;
        }

        /// <summary>
        /// 遷移履歴を削除する
        /// </summary>
        public void ClearHistory()
        {
            this.frame.SetNavigationState("1,0");
        }

        /// <summary>
        /// 遷移状態を復元する
        /// </summary>
        public void RestoreSavedNavigation()
        {
            var parameter = this.sessionStateService.SessionState[LastNavigationParameterKey];
            this.NavigateToCurrentPresenter(NavigationMode.Refresh, parameter);
        }

        /// <summary>
        /// 中断状態に遷移する
        /// </summary>
        public void Suspending()
        {
            this.NavigateFromCurrentPresenter(true);
        }

        /// <summary>
        /// 現在の Presenter に遷移する
        /// </summary>
        /// <param name="navigationMode">遷移モード</param>
        /// <param name="parameter">遷移パラメータ</param>
        private void NavigateToCurrentPresenter(NavigationMode navigationMode, object parameter)
        {
            var frameState = this.sessionStateService.GetSessionStateForFrame(this.frame);
            var viewModelKey = "ViewModel-" + this.frame.BackStackDepth;

            if (navigationMode == NavigationMode.New)
            {
                // 画面遷移履歴に新しい履歴が追加されたら現在位置から進行方向に向かう履歴を削除する
                var nextViewModelKey = viewModelKey;
                int nextViewModelIndex = this.frame.BackStackDepth;
                while (frameState.Remove(nextViewModelKey))
                {
                    nextViewModelIndex++;
                    nextViewModelKey = "ViewModel-" + nextViewModelIndex;
                }
            }

            var newView = this.frame.Content as FrameworkElement;
            if (newView == null)
            {
                return;
            }

            var presenter = newView.GetPresenter() as IPresenterBase;

            presenter.PresenterView = newView;
            presenter.PresenterViewModel = newView.DataContext as ViewModelBase;

            if (presenter == null)
            {
                return;
            }
            Dictionary<string, object> viewModelState;
            if (frameState.ContainsKey(viewModelKey))
            {
                viewModelState = frameState[viewModelKey] as Dictionary<string, object>;
            }
            else
            {
                viewModelState = new Dictionary<string, object>();
            }
            presenter.OnNavigatedTo(parameter, navigationMode, viewModelState);
            frameState[viewModelKey] = viewModelState;
        }

        /// <summary>
        /// 現在の Presenter から遷移する
        /// </summary>
        /// <param name="suspending">中断フラグ</param>
        private void NavigateFromCurrentPresenter(bool suspending)
        {
            var departingView = this.frame.Content as Page;
            if (departingView == null)
            {
                return;
            }
            var frameState = this.sessionStateService.GetSessionStateForFrame(this.frame);

            var presenter = departingView.GetPresenter() as INavigationAware;
            if (presenter == null)
            {
                return;
            }

            var viewModelKey = "ViewModel-" + this.frame.BackStackDepth;
            var viewModelState = frameState.ContainsKey(viewModelKey)
                                        ? frameState[viewModelKey] as Dictionary<string, object>
                                        : null;

            presenter.OnNavigatedFrom(viewModelState, suspending);
        }

        /// <summary>
        /// 遷移中イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e"><see cref="EventArgs"/> のイベント引数</param>
        private void OnNavigating(object sender, EventArgs e)
        {
            this.NavigateFromCurrentPresenter(false);
        }

        /// <summary>
        /// 遷移後イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e"><see cref="MvvmNavigatedEventArgs"/> のイベント引数</param>
        private void OnNavigated(object sender, Microsoft.Practices.Prism.StoreApps.MvvmNavigatedEventArgs e)
        {
            // 最後の遷移履歴の名称とパラメータを更新する
            this.sessionStateService.SessionState[LastNavigationPageKey] = this.frame.Content.GetType().FullName;
            this.sessionStateService.SessionState[LastNavigationParameterKey] = e.Parameter;

            this.NavigateToCurrentPresenter(e.NavigationMode, e.Parameter);
        }

        /// <summary>
        /// Null 許容値を比較する
        /// </summary>
        /// <param name="obj1">比較する値１</param>
        /// <param name="obj2">比較する値２</param>
        /// <returns>等しい場合 <c>true</c>、それ以外は <c>false</c></returns>
        private static bool AreEquals(object obj1, object obj2)
        {
            if (obj1 != null)
            {
                return obj1.Equals(obj2);
            }
            return obj2 == null;
        }
    }
}
