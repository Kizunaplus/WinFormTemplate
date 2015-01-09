using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.EventArg
{
    /// <summary>
    /// アクションイベント引数
    /// </summary>
    class ActionEventArgs : AbstractEventArgs
    {
        #region プロパティ
        /// <summary>
        /// コントローラ
        /// </summary>
        public string Controller
        {
            get;
            set;
        }

        /// <summary>
        /// アクション名
        /// </summary>
        public string ActionName
        {
            get;
            set;
        }

        /// <summary>
        /// 引数
        /// </summary>
        public object[] Parameters
        {
            get;
            set;
        }
        #endregion
    }
}
