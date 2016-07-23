using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    /// <summary>
    /// モデルバリデーション属性
    /// </summary>
    class ModelValidationAttribute : Attribute
    {
        #region 検証
        /// <summary>
        /// データグリッドビューの入力チェック
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public virtual bool Valid(DataGridView control, ref String message) {
            AbstractModel model = control.DataSource as AbstractModel;
            if (model == null)
            {
                return true;
            }

            return model.Valid(out message);
        }

        /// <summary>
        /// 標準コントロールの入力チェック
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public virtual bool Valid(Control control, ref String message)
        {
            return Valid(control.Text, ref message, control.Name);
        }

        /// <summary>
        /// 標準モデルの入力チェック
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="property">プロパティ</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public bool Valid(IModel target, PropertyInfo property, ref String message)
        {
            object value = property.GetValue(target, null);
            return Valid(value, ref message, property.Name);
        }


        /// <summary>
        /// 標準型の入力チェック
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public virtual bool Valid(object target, ref String message, String typeName = "") { return true; }
        #endregion
    }
}
