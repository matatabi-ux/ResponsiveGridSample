﻿//<auto-generated />
#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveGridSample.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
	using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    /// <summary>
    /// Photo のインタフェース
    /// </summary>
    public partial interface IPhoto
    {
        string UniqueId { get; set; }
        string ImageUri { get; set; }
        string Title { get; set; }
        string Owner { get; set; }
    }

    /// <summary>
    /// Photo
    /// </summary>
    [XmlRoot("entity")]
    public partial class Photo : IPhoto
    {
        #region マルチスレッド排他制御用

        /// <summary>
        /// 排他制御フラグ
        /// </summary>
        private static bool isSynchronize = false;

        /// <summary>
        /// 排他制御フラグ
        /// </summary>
        [XmlIgnore]
        public static bool IsSynchronize
        {
            get { return isSynchronize; }
            set { isSynchronize = value; }
        }

        /// <summary>
        /// 排他制御オブジェクト
        /// </summary>
        public static readonly Mutex LockObject = new Mutex();

        #endregion //マルチスレッド排他制御用

        #region UniqueId:ID プロパティ
        /// <summary>
        /// ID
        /// </summary>
        private string uniqueId; 

        /// <summary>
        /// ID の取得および設定
        /// </summary>
        [XmlAttribute("id")]
        public string UniqueId
        {
            get { return this.uniqueId; }
            set 
            {
                try
                {
                    if (isSynchronize)
                    {
                        LockObject.WaitOne();
                    }

                    this.uniqueId = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (isSynchronize)
                    {
                        LockObject.ReleaseMutex();
                    }
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
        /// 画像Uri の取得および設定
        /// </summary>
        [XmlAttribute("image")]
        public string ImageUri
        {
            get { return this.imageUri; }
            set 
            {
                try
                {
                    if (isSynchronize)
                    {
                        LockObject.WaitOne();
                    }

                    this.imageUri = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (isSynchronize)
                    {
                        LockObject.ReleaseMutex();
                    }
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
        /// タイトル の取得および設定
        /// </summary>
        [XmlAttribute("title")]
        public string Title
        {
            get { return this.title; }
            set 
            {
                try
                {
                    if (isSynchronize)
                    {
                        LockObject.WaitOne();
                    }

                    this.title = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (isSynchronize)
                    {
                        LockObject.ReleaseMutex();
                    }
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
        /// 撮影者 の取得および設定
        /// </summary>
        [XmlAttribute("owner")]
        public string Owner
        {
            get { return this.owner; }
            set 
            {
                try
                {
                    if (isSynchronize)
                    {
                        LockObject.WaitOne();
                    }

                    this.owner = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (isSynchronize)
                    {
                        LockObject.ReleaseMutex();
                    }
                }
            }
        }
        #endregion //Owner:撮影者 プロパティ

    }

}