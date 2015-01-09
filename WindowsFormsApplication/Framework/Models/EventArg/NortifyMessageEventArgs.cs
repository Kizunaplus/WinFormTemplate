using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.EventArg
{
    /// <summary>
    /// 通知エリアのメッセージイベント
    /// </summary>
    class NortifyMessageEventArgs : AbstractEventArgs
    {
        #region プロパティ
        /// <summary>
        /// 通知アイコン
        /// </summary>
        public ToolTipIcon Icon
        {
            get;
            set;
        }

        /// <summary>
        /// 通知タイトル
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 通知メッセージ
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 通知元
        /// </summary>
        public object Source
        {
            get;
            set;
        }
        #endregion
    }
}
