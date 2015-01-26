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
    /// フォルダの存在チェッククラス
    /// </summary>
    class DirectoryExistCheckAttribute : ModelValidationAttribute
    {
        /// <summary>
        /// ファイルの存在チェック
        /// </summary>
        private bool isExist;

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="isExist">true: ファイルが存在しないとエラー、 false: ファイルが存在するとエラー</param>
        public DirectoryExistCheckAttribute(bool isExist)
        {
            this.isExist = isExist;
        }
        #endregion
        
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
            if (Directory.Exists(text) != isExist)
            {
                valid = false;
                if (isExist == true)
                {
                    message = String.Format(FrameworkValidationMessage.FileExistCheckMessageNotExist, typeName, text);
                }
                else
                {
                    message = String.Format(FrameworkValidationMessage.FileExistCheckMessageExist, typeName, text);
                }
            }

            return valid;
        }

    }
}
