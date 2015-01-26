using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    /// <summary>
    /// 正規表現にて入力チェッククラス
    /// </summary>
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

        #region 検証
        /// <summary>
        /// プロパティの入力チェック
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="message">エラーメッセージ</param>
        /// <returns></returns>
        public override bool Valid(Object target, ref String message, String typeName = "")
        {
            bool valid = true;

            if (Regex.IsMatch(target.ToString(), regexPattern) == this.isMatch)
            {
                valid = false;
                message = String.Format(errorMessage + " {0}={1}", typeName, target);
            }

            return valid;
        }
        #endregion

    }
}
