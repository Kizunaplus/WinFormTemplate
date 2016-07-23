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
        /// 標準型の入力チェック
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public override bool Valid(object target, ref String message, String typeName = "")
        {
            bool valid = true;

            if (target == null)
            {
                valid = false;
                message = String.Format(FrameworkValidationMessage.RequiresInputMessage, typeName, target);
            }

            if (valid == true 
                && target.GetType() == typeof(String)
                && String.IsNullOrEmpty(target as String) == true)
            {
                valid = false;
                message = String.Format(FrameworkValidationMessage.RequiresInputMessage, typeName, target);
            }
            return valid;
        }
    }
}
