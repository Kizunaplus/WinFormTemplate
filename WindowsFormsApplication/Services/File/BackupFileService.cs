using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication.Models;

namespace WindowsFormsApplication.Services.File
{
    /// <summary>
    /// ファイルバックアップサービス
    /// </summary>
    class BackupFileService : IFileService
    {
        #region メンバー変数
        /// <summary>
        /// 一括バックアップで使用するフォルダ一覧
        /// </summary>
        private IList<string> batchBackupFolderList;

        /// <summary>
        /// 一括バックアップ対象リスト
        /// </summary>
        private IList<Tuple<string, IModel>> batchBackupTargetFileList;
        #endregion

        #region プロパティ
        /// <summary>
        /// 一括バックアップ対象リスト
        /// </summary>
        private IList<Tuple<string, IModel>> BatchBackupTargetFileList
        {
            get
            {
                if (batchBackupTargetFileList == null)
                {
                    batchBackupTargetFileList = new List<Tuple<string, IModel>>();
                }

                return batchBackupTargetFileList;
            }
        }
        /// <summary>
        /// 一括バックアップ対象リスト
        /// </summary>
        private IList<string> BatchBackupFolderList
        {
            get
            {
                if (BatchBackupFolderList == null)
                {
                    // 初回から一時フォルダ+実行ファイル名のフォルダを追加
                    batchBackupFolderList = new List<string>();
                    batchBackupFolderList.Add(Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Application.ExecutablePath)));
                }

                return batchBackupFolderList;
            }
        }
        #endregion

        #region バックアップ
        /// <summary>
        /// 一括バックアップファイルに指定されたファイルのバックアップ
        /// </summary>
        public void BackupBatchFile()
        {
            lock (BatchBackupTargetFileList)
            {
                lock (BatchBackupFolderList)
                {
                    foreach (var backupFileData in BatchBackupTargetFileList)
                    {
                        foreach (var folderPath in BatchBackupFolderList)
                        {
                            if (string.IsNullOrEmpty(folderPath) == true)
                            {
                                // パスが不正
                                continue;
                            }
                            if (Directory.Exists(folderPath) == false)
                            {
                                // フォルダの作成
                                try
                                {
                                    Directory.CreateDirectory(folderPath);
                                }
                                catch
                                {
                                    continue;
                                }
                            }

                            backupFileData.Item2.Save(Path.Combine(folderPath, Path.GetFileName(backupFileData.Item1)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 一時バックアップを行いながらファイルへ書き込みます。
        /// </summary>
        public bool BackupWriteFile(string filePath, IModel model, int retry = 0)
        {
            string movedFilePath = Path.GetTempFileName();
            try
            {
                if (System.IO.File.Exists(filePath) == true)
                {
                    System.IO.File.Copy(filePath, movedFilePath, true);
                }
            }
            catch (Exception)
            {
                return false;
            }

            // ファイルの書き込み
            bool isSuccess = false;
            int retryCount = 0;
            while (retryCount <= retry)
            {
                if (model.Save(filePath) == false)
                {
                    // 500ms待つ
                    Thread.Sleep(500);
                    retryCount++;
                    continue;
                }
                isSuccess = true;
                break;
            }

            if (isSuccess == false)
            {
                // 元に戻す。
                try
                {
                    System.IO.File.Copy(movedFilePath, filePath, true);
                }
                catch
                {
                }
            }

            System.IO.File.Delete(movedFilePath);

            return isSuccess;
        }
        #endregion

        #region 読み込み
        /// <summary>
        /// バックアップファイルが存在するかチェック
        /// </summary>
        /// <returns></returns>
        public bool IsBackupBatchFileExist(string filePath)
        {
            lock (BatchBackupFolderList)
            {
                foreach (var folderPath in BatchBackupFolderList)
                {
                    if (string.IsNullOrEmpty(folderPath) == true)
                    {
                        // パスが不正
                        continue;
                    }
                    if (Directory.Exists(folderPath) == false)
                    {
                        // フォルダの作成
                        try
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (System.IO.File.Exists(Path.Combine(folderPath, Path.GetFileName(filePath))) == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// バックアップファイルが元のファイルよりも新しいかチェック
        /// </summary>
        /// <returns></returns>
        public bool IsNewBackupBatchFile(string filePath)
        {
            DateTime fileDateTime = DateTime.MinValue;
            try
            {
                fileDateTime = System.IO.File.GetLastWriteTime(filePath);
            }
            catch
            {

            }

            lock (BatchBackupFolderList)
            {
                foreach (var folderPath in BatchBackupFolderList)
                {
                    if (string.IsNullOrEmpty(folderPath) == true)
                    {
                        // パスが不正
                        continue;
                    }
                    if (Directory.Exists(folderPath) == false)
                    {
                        // フォルダの作成
                        try
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    DateTime backupfileDateTime = DateTime.MinValue;
                    try
                    {
                        backupfileDateTime = System.IO.File.GetLastWriteTime(Path.Combine(folderPath, Path.GetFileName(filePath)));
                    }
                    catch
                    {

                    }
                    if (fileDateTime < backupfileDateTime)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 最も新しいファイルを読み込み
        /// </summary>
        /// <returns></returns>
        public bool LoadNewBackupBatchFile(string filePath, ref IModel data)
        {
            string newFilePath = filePath;
            DateTime newFileDateTime = DateTime.MinValue;
            try
            {
                newFileDateTime = System.IO.File.GetLastWriteTime(newFilePath);
            }
            catch
            {

            }

            lock (BatchBackupFolderList)
            {
                foreach (var folderPath in BatchBackupFolderList)
                {
                    if (string.IsNullOrEmpty(folderPath) == true)
                    {
                        // パスが不正
                        continue;
                    }
                    if (Directory.Exists(folderPath) == false)
                    {
                        // フォルダの作成
                        try
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    DateTime backupfileDateTime = DateTime.MinValue;
                    try
                    {
                        backupfileDateTime = System.IO.File.GetLastWriteTime(Path.Combine(folderPath, Path.GetFileName(filePath)));
                    }
                    catch
                    {

                    }
                    if (newFileDateTime < backupfileDateTime)
                    {
                        newFilePath = Path.Combine(folderPath, Path.GetFileName(filePath));
                        newFileDateTime = backupfileDateTime;
                    }
                }
            }
            bool isSuccess = false;
            if (newFileDateTime != DateTime.MinValue)
            {
                data = data.Load(newFilePath);
                isSuccess = true;
            }

            return isSuccess;
        }
        #endregion

        #region 一括対象
        /// <summary>
        /// 一括バックアップ対象に追加
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="model">保存する内容</param>
        public void AddBatchBackup(string filePath, IModel model)
        {
            lock (BatchBackupTargetFileList)
            {
                BatchBackupTargetFileList.Add(new Tuple<string, IModel>(filePath, model));
            }
        }

        /// <summary>
        /// 一括バックアップ対象から削除
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public void RemoveBatchBackup(string filePath)
        {
            lock (BatchBackupTargetFileList)
            {
                // ファイルパスが一致する情報を削除
                for (int fileIndex = BatchBackupTargetFileList.Count - 1; 0 <= fileIndex; fileIndex--)
                {
                    if (BatchBackupTargetFileList[fileIndex].Item1 == filePath)
                    {
                        BatchBackupTargetFileList.RemoveAt(fileIndex);
                    }
                }
            }
        }

        /// <summary>
        /// 一括バックアップ対象に追加
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="model">保存する内容</param>
        public void AddBatchBackupFolder(string filePath, IModel model)
        {
            lock (BatchBackupFolderList)
            {
                if (string.IsNullOrEmpty(filePath) == true)
                {
                    // 不正なパス
                    return;
                }

                BatchBackupFolderList.Add(filePath);
            }
        }

        /// <summary>
        /// 一括バックアップ対象から削除
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public void RemoveBatchBackupFolder(string filePath)
        {
            lock (BatchBackupFolderList)
            {
                // ファイルパスが一致する情報を削除
                for (int fileIndex = BatchBackupFolderList.Count - 1; 0 <= fileIndex; fileIndex--)
                {
                    if (BatchBackupFolderList[fileIndex] == filePath)
                    {
                        BatchBackupFolderList.RemoveAt(fileIndex);
                    }
                }
            }
        }
        #endregion
    }
}
