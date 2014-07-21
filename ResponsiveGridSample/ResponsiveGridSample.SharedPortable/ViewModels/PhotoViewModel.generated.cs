﻿//<auto-generated>
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
    /// 写真情報の ViewModel のインタフェース
    /// </summary>
    public partial interface IPhotoViewModel
    {
        string UniqueId { get; set; }
        string ImageUri { get; set; }
        string Title { get; set; }
        string Owner { get; set; }
    }

    /// <summary>
    /// 写真情報の ViewModel
    /// </summary>
    
    public partial class PhotoViewModel : ViewModelBase, IPhotoViewModel
    {
        #region UniqueId:ID プロパティ
        /// <summary>
        /// ID
        /// </summary>
        private string uniqueId; 

        /// <summary>
        /// ID の変更前の処理
        /// </summary>
        partial void OnUniqueIdChanging(ref string value);

        /// <summary>
        /// ID の変更後の処理
        /// </summary>
        partial void OnUniqueIdChanged();

        /// <summary>
        /// ID の取得および設定
        /// </summary>
        [RestorableData]
        public string UniqueId
        {
            get
            {
                return this.uniqueId;
            }

            set
            {
                if (this.uniqueId != value)
                {
                    this.OnUniqueIdChanging(ref value);
                    this.SetProperty<string>(ref this.uniqueId, value);
                    this.OnUniqueIdChanged();
                }
            }
        }
        #endregion //UniqueId:ID プロパティ

        #region ImageUri:画像Uri プロパティ
        /// <summary>
        /// 画像Uri
        /// </summary>
        private string imageUri; 

        /// <summary>
        /// 画像Uri の変更前の処理
        /// </summary>
        partial void OnImageUriChanging(ref string value);

        /// <summary>
        /// 画像Uri の変更後の処理
        /// </summary>
        partial void OnImageUriChanged();

        /// <summary>
        /// 画像Uri の取得および設定
        /// </summary>
        [RestorableData]
        public string ImageUri
        {
            get
            {
                return this.imageUri;
            }

            set
            {
                if (this.imageUri != value)
                {
                    this.OnImageUriChanging(ref value);
                    this.SetProperty<string>(ref this.imageUri, value);
                    this.OnImageUriChanged();
                }
            }
        }
        #endregion //ImageUri:画像Uri プロパティ

        #region Title:タイトル プロパティ
        /// <summary>
        /// タイトル
        /// </summary>
        private string title; 

        /// <summary>
        /// タイトル の変更前の処理
        /// </summary>
        partial void OnTitleChanging(ref string value);

        /// <summary>
        /// タイトル の変更後の処理
        /// </summary>
        partial void OnTitleChanged();

        /// <summary>
        /// タイトル の取得および設定
        /// </summary>
        [RestorableData]
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title != value)
                {
                    this.OnTitleChanging(ref value);
                    this.SetProperty<string>(ref this.title, value);
                    this.OnTitleChanged();
                }
            }
        }
        #endregion //Title:タイトル プロパティ

        #region Owner:撮影者 プロパティ
        /// <summary>
        /// 撮影者
        /// </summary>
        private string owner; 

        /// <summary>
        /// 撮影者 の変更前の処理
        /// </summary>
        partial void OnOwnerChanging(ref string value);

        /// <summary>
        /// 撮影者 の変更後の処理
        /// </summary>
        partial void OnOwnerChanged();

        /// <summary>
        /// 撮影者 の取得および設定
        /// </summary>
        [RestorableData]
        public string Owner
        {
            get
            {
                return this.owner;
            }

            set
            {
                if (this.owner != value)
                {
                    this.OnOwnerChanging(ref value);
                    this.SetProperty<string>(ref this.owner, value);
                    this.OnOwnerChanged();
                }
            }
        }
        #endregion //Owner:撮影者 プロパティ

    }

}