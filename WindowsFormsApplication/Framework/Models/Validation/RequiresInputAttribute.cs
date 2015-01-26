using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication.Framework.Message;
using System.Reflection;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    /// <summary>
    /// 必須チェッククラス
    /// </summary>
    class RequiresInputAttribute : ModelValidationAttribute
    {
        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public override bool Valid(Control control, ref String message)
        {
            bool valid = true;

            if (control.Text == string.Empty)
            {
                valid = false;
                message = String.Format(FrameworkValidationMessage.RequiresInputMessage, control.Name, control.Text);
            }

            return valid;
        }

        /// <summary>
        /// プロパティの入力チェック
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="property">プロパティ</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public virtual bool Valid(object target, PropertyInfo property, ref String message)
        {
            bool valid = true;

            object value = property.GetValue(target, null);
            if (value == null)
            {
                valid = false;
                message = String.Format(FrameworkValidationMessage.RequiresInputMessage, property.Name, value);
            }

            return valid;
        }
    }
}
