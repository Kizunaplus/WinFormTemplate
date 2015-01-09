using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Enums
{
    #region 通知エリア
    /// <summary>
    /// 通知エリア表示スタイル
    /// </summary>
    public enum NortifyStyles
    {
        /// <summary>
        /// 表示しない
        /// </summary>
        None,

        /// <summary>
        /// 常に表示
        /// </summary>
        Alway,

        /// <summary>
        /// 最小化時のみ
        /// </summary>
        MinimumWindow,

        /// <summary>
        /// 最小化時のみ(タスク非表示)
        /// </summary>
        MinimumWindowAndHiddenTask
    }
    #endregion

    #region メニュー
    /// <summary>
    /// メニューエリア表示スタイル
    /// </summary>
    public enum MenuStyles
    {
        /// <summary>
        /// 表示しない
        /// </summary>
        None,

        /// <summary>
        /// 常に表示
        /// </summary>
        Alway,

        /// <summary>
        /// Alt時のみ
        /// </summary>
        AltKeyDown
    }
    #endregion

    #region ステータス
    /// <summary>
    /// ステータスエリア表示スタイル
    /// </summary>
    public enum StatusStyles
    {
        /// <summary>
        /// 表示しない
        /// </summary>
        None,

        /// <summary>
        /// イベント中のみ
        /// </summary>
        EventOnly
    }
    #endregion
}
