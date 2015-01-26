using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers.State
{
    /// <summary>
    /// 例外発生状態
    /// </summary>
    class ExceptionState : AbstractState
    {
        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="source">呼び出し元</param>
        public ExceptionState(object source)
            : base(source)
        {
            this.mode = StateMode.Error;
        }
        #endregion
    }
}
