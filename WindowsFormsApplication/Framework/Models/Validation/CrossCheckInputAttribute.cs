using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    class CrossChecksInputAttribute : ModelValidationAttribute
    {
        #region Delegate
        /// <summary>
        /// クロスチェック処理関数
        /// </summary>
        /// <param name="controls">チェック対象のコントロール</param>
        /// <returns></returns>
        public delegate bool CrossCheckDelegate(IList<Control> controls);
        #endregion

        #region メンバー変数
        /// <summary>
        /// 判定メソッド
        /// </summary>
        private CrossCheckDelegate delegateMethod;

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        private String message;
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="delegateMethod">判定メソッド</param>
        public CrossChecksInputAttribute(CrossCheckDelegate delegateMethod, String message)
        {
            this.delegateMethod = delegateMethod;
            this.message = message;
        }
        #endregion

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public override bool Valid(Control control, ref String message)
        {
            bool valid = true;

            if (delegateMethod == null)
            {
                return true;
            }

            IList<Control> controls = (IList<Control>)control.Parent.Controls;
            if (this.delegateMethod.Invoke(controls) == false)
            {
                valid = false;
                message = this.message;
            }

            return valid;
        }
    }
}
