using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication.Models.EventArg
{
    /// <summary>
    /// ファイル操作イベント引数
    /// </summary>
    class FileEventArgs : EventArgs
    {
        #region プロパティ
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }
        #endregion
    }
}
