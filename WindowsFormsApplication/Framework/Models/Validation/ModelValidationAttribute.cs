using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    /// <summary>
    /// モデルバリデーション属性
    /// </summary>
    class ModelValidationAttribute : Attribute
    {
        /// <summary>
        /// データグリッドビューの入力チェック
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public virtual bool Valid(DataGridView control, ref String message) { return true; }

        /// <summary>
        /// 標準コントロールの入力チェック
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public virtual bool Valid(Control control, ref String message) { return true; }

    }
}
