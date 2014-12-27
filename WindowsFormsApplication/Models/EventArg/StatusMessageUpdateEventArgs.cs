using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication.Models.EventArg
{
    /// <summary>
    /// ステータスメッセージ更新イベント引数
    /// </summary>
    class StatusMessageUpdateEventArgs : EventArgs
    {
        #region プロパティ
        /// <summary>
        /// ステータスメッセージ
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 優先順位
        /// </summary>
        public int Priority
        {
            get;
            set;
        }

        /// <summary>
        /// メッセージID
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }
        #endregion
    }
}
