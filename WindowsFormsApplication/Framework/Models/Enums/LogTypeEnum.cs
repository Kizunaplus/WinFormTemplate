using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Enums
{
    #region 列挙体
    /// <summary>
    /// ログタイプ
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 例外
        /// </summary>
        Exception,

        /// <summary>
        /// エラー
        /// </summary>
        Error,

        /// <summary>
        /// 警告
        /// </summary>
        Warn,

        /// <summary>
        /// 情報
        /// </summary>
        Info,

        /// <summary>
        /// デバッグ
        /// </summary>
        Debug
    }
    #endregion

}
