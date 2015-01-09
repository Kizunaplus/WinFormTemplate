using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    class ValueCheckAttribute : ModelValidationAttribute
    {
        #region メンバー変数
        /// <summary>
        /// チェックパターン（正規表現）
        /// </summary>
        private String regexPattern;

        /// <summary>
        /// チェックエラーメッセージ
        /// </summary>
        private String errorMessage;

        /// <summary>
        /// チェック　マッチフラグ
        /// </summary>
        private bool isMatch;
        #endregion

        #region 初期化
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regexPattern">チェックパターン（正規表現）</param>
        /// <param name="errorMessage">チェックエラーメッセージ</param>
        /// <param name="isMatch">マッチフラグ</param>
        public ValueCheckAttribute(String regexPattern = "", String errorMessage = "", bool isMatch = false)
        {
            this.regexPattern = regexPattern;
            this.errorMessage = errorMessage;
            this.isMatch = isMatch;
        }
        #endregion

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="control">対象コントロール</param>
        /// <param name="message">エラー内容メッセージ</param>
        /// <returns></returns>
        public override bool Valid(Control control, ref String message)
        {
            bool valid = true;

            if (Regex.IsMatch(control.Text, regexPattern) == this.isMatch)
            {
                valid = false;
                message = String.Format(errorMessage + " {0}={1}", control.Name, control.Text);
            }

            return valid;
        }

    }
}
