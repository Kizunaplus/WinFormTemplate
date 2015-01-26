using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers.State
{
    /// <summary>
    /// 情報出力状態
    /// </summary>
    class InformationState : AbstractState
    {
        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="source">呼び出し元</param>
        public InformationState(object source)
            : base(source)
        {
            this.mode = StateMode.Log;
        }
        #endregion
    }
}
