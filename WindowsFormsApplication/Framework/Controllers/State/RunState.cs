using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers.State
{
    /// <summary>
    /// 実行状態
    /// </summary>
    class RunState : AbstractState
    {
        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="source">呼び出し元</param>
        public RunState(object source)
            : base(source)
        {
            this.mode = StateMode.Process;
        }
        #endregion
    }
}
