using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication.Framework.Message;
using System.Reflection;
using System.IO;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    /// <summary>
    /// ファイルパスの妥当性チェッククラス
    /// </summary>
    class FilePathCheckAttribute : ModelValidationAttribute
    {
        /// <summary>
        /// プロパティの入力チェック
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public override bool Valid(Object target, ref String message, String typeName = "")
        {
            bool valid = true;

            String text = target.ToString();
            char[] invalidChars = System.IO.Path.GetInvalidPathChars();

            int index = text.IndexOfAny(invalidChars);
            if (0 <= index)
            {
                valid = false;
                message = String.Format(FrameworkValidationMessage.FilePathCheckAttributeMessageIllegalChar, typeName, text, text[index]);

                return valid;
            }
            String fileName = Path.GetFileName(text);
            char[] invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();

            index = fileName.IndexOfAny(invalidFileNameChars);
            if (0 <= index)
            {
                valid = false;
                message = String.Format(FrameworkValidationMessage.FilePathCheckAttributeMessageIllegalChar, typeName, text, text[index]);
            }


            return valid;
        }

    }
}
