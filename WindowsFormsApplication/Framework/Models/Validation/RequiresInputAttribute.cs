using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication.Framework.Message;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
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
    }
}
