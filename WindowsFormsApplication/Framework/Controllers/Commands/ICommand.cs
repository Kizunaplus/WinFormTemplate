using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands
{
    /// <summary>
    /// コマンドクラスインターフェース
    /// デザインパターン:コマンド
    /// 
    /// Viewから様々な機能を呼び出す場合や
    /// ログやサービスを呼び出す場合にコマンドを用います。
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="source">呼び出し元</param>
        /// <param name="args">イベントハンドラ引数</param>
        /// <returns>true: 成功 ,false: 失敗</returns>
        bool Execute(object source, EventArgs args);

        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="state">状態</param>
        /// <param name="args">イベントハンドラ引数</param>
        /// <returns>true: 成功 ,false: 失敗</returns>
        bool Execute(IState state, EventArgs args);
    }
}
