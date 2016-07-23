using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands
{
    /// <summary>
    /// コマンド抽象クラス
    /// </summary>
    class AbstractCommand : ICommand
    {
        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="source">呼び出し元</param>
        /// <param name="args">イベントハンドラ引数</param>
        /// <returns>true: 成功 ,false: 失敗</returns>
        public bool Execute(object source, EventArgs args)
        {
            IState state = new NonState(source);

            return Execute(state, args);
        }

        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="state">状態</param>
        /// <param name="args">イベントハンドラ引数</param>
        /// <returns>true: 成功 ,false: 失敗</returns>
        public bool Execute(IState state, EventArgs args)
        {
            CommandRegister register = CommandRegister.Current;

            // コマンドの実行
            return register.Execute(this, state, args);
        }
    }
}
