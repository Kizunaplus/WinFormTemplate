using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication.Models.EventArg
{
    /// <summary>
    /// 例外イベント引数
    /// </summary>
    class ExceptionEventArgs : EventArgs
    {
        #region プロパティ
        /// <summary>
        /// 例外
        /// </summary>
        public Exception Exception
        {
            get;
            set;
        }
        #endregion
    }
}
