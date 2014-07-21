#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveGridSample.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ResponsiveGridSample.Presenters;
    using ResponsiveGridSample.ViewModels;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Presenter つき View のインタフェース
    /// </summary>
    /// <typeparam name="TPresenter">Presenter の型</typeparam>
    public interface IPresenterView<TPresenter> where TPresenter : IPresenterBase
    {
        /// <summary>
        /// Presenter
        /// </summary>
        TPresenter Presenter { get; }
    }
}
