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
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;
    using Windows.Storage;

    /// <summary>
    /// アプリケーション設定情報リポジトリ
    /// </summary>
    public class ApplicationSettingsRepository
    {
        #region Privates

        /// <summary>
        /// データの参照先フォルダ
        /// </summary>
        private static readonly IStorageFolder StoreFolder = ApplicationData.Current.LocalFolder;

        /// <summary>
        /// アプリケーション設定情報のファイル名
        /// </summary>
        private static readonly string FileName = @"app-settings.xml";

        /// <summary>
        /// アプリケーション設定情報のデフォルト設定ファイル名
        /// </summary>
        private static readonly string DefaultFilePath = @"ms-appx:///Assets/Data/default-app-settings.xml";

        #endregion //Privates

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ApplicationSettingsRepository()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ApplicationSettingsRepository()
        {
        }

        /// <summary>
        /// アプリケーション設定情報
        /// </summary>
        public ApplicationSettings Settings { get; private set; }

        /// <summary>
        /// アプリケーション設定情報を読み込む
        /// </summary>
        /// <returns>成功した場合は true, 失敗した場合は false</returns>
        public async Task<bool> LoadAsync()
        {
            try
            {
                var file = await StoreFolder.GetFileAsync(FileName);
                using (var stream = await file.OpenSequentialReadAsync())
                {
                    var serializer = new XmlSerializer(typeof(ApplicationSettings));
                    this.Settings = serializer.Deserialize(stream.AsStreamForRead()) as ApplicationSettings;
                    ApplicationSettings.IsSynchronize = true;
                }

                return true;
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine(string.Format("{0} がないのでデフォルトの設定情報を読み込みます。", FileName));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            // 読み込みに失敗した場合はデフォルト値を読み込む
            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(DefaultFilePath, UriKind.Absolute));
                using (var stream = await file.OpenSequentialReadAsync())
                {
                    var serializer = new XmlSerializer(typeof(ApplicationSettings));
                    this.Settings = serializer.Deserialize(stream.AsStreamForRead()) as ApplicationSettings;
                    ApplicationSettings.IsSynchronize = true;
                }

                await this.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// アプリケーション設定情報を書き込む
        /// </summary>
        /// <returns>成功した場合は true, 失敗した場合は false</returns>
        public async Task<bool> SaveAsync()
        {
            try
            {
                ApplicationSettings.LockObject.WaitOne();

                var file = await StoreFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    var writer = XmlWriter.Create(
                         stream,
                         new XmlWriterSettings()
                         {
                             Encoding = Encoding.UTF8,
                             Indent = false,
                             NewLineChars = string.Empty,
                         });

                    var serializer = new XmlSerializer(typeof(ApplicationSettings));
                    serializer.Serialize(writer, this.Settings);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                ApplicationSettings.LockObject.ReleaseMutex();
            }

            return false;
        }
    }
}
