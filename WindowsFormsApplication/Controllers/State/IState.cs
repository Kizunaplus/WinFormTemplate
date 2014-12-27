using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication.Controllers.State
{
    /// <summary>
    /// 処理状態
    /// </summary>
    public enum StateMode
    {
        /// <summary>
        /// 初期化
        /// </summary>
        Initialize,

        /// <summary>
        /// 開始
        /// </summary>
        Start,

        /// <summary>
        /// 処理中
        /// </summary>
        Process,

        /// <summary>
        /// 終了
        /// </summary>
        End,

        /// <summary>
        /// エラー
        /// </summary>
        Error,

        /// <summary>
        /// ログ
        /// </summary>
        Log,

        /// <summary>
        /// 無し
        /// </summary>
        None
    }

    /// <summary>
    /// ステートクラスインターフェース
    /// デザインパターン:ステート
    /// </summary>
    interface IState
    {
        /// <summary>
        /// 呼び出し元の取得
        /// </summary>
        /// <returns>呼び出し元のクラス</returns>
        object GetSource();

        /// <summary>
        /// 処理状態の取得
        /// </summary>
        /// <returns>処理状態</returns>
        StateMode GetMode();
    }
}
