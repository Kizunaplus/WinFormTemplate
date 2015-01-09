using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers.State
{
    /// <summary>
    /// 情報状態
    /// </summary>
    class InformationState : IState
    {
        #region メンバー変数
        /// <summary>
        /// 呼び出し元
        /// </summary>
        private object source;
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="source">呼び出し元</param>
        public InformationState(object source)
        {
            if (source != null)
            {
                this.source = source;
            }
        }
        #endregion

        #region 取得
        /// <summary>
        /// 呼び出し元の取得
        /// </summary>
        /// <returns>呼び出し元のクラス</returns>
        public object GetSource()
        {
            return source;
        }

        /// <summary>
        /// 処理状態の取得
        /// </summary>
        /// <returns>処理状態</returns>
        public StateMode GetMode()
        {
            return StateMode.Error;
        }
        #endregion
    }
}
