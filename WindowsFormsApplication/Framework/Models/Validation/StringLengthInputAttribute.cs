using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication.Framework.Message;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    class StringLengthInputAttribute : ModelValidationAttribute
    {
        /// <summary>
        /// 最小文字数
        /// </summary>
        private int min_length;

        /// <summary>
        /// 最大文字数
        /// </summary>
        private int max_length;

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="max_length">最大文字長</param>
        public StringLengthInputAttribute(int max_length)
        {
            this.min_length = 0;
            this.max_length = max_length;
        }

        /// <summary>
        /// コンストラクタ
        /// 
        /// max_lengthに-1を指定した場合は、最大値のチェックを行いません。
        /// </summary>
        /// <param name="min_length">最小文字長</param>
        /// <param name="max_length">最大文字長</param>
        public StringLengthInputAttribute(int min_length, int max_length)
        {
            this.min_length = min_length;
            this.max_length = max_length;
        }
        #endregion

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="control"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Valid(Control control, ref String message)
        {
            bool valid = true;

            String text = control.Text;
            if (text.Length < min_length)
            {
                valid = false;
                message = String.Format(FrameworkValidationMessage.StringLengthInput1ArgsMaxMessage, control.Name, control.Text, min_length);
            }

            if (0 <= max_length && max_length < text.Length)
            {
                valid = false;
                message = String.Format(FrameworkValidationMessage.StringLengthInput1ArgsMinMessage, control.Name, control.Text, min_length, max_length);
            }

            return valid;
        }
    }
}
