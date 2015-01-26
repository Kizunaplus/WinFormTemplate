using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers.State
{
    /// <summary>
    /// 状態なし
    /// ステータスがない場合は、このクラスを使用する。
    /// </summary>
    class NonState : AbstractState
    {
        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="source">呼び出し元</param>
        public NonState(object source)
            : base(source)
        {
            this.mode = StateMode.None;
        }
        #endregion
    }
}
