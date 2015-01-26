using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using WindowsFormsApplication.Framework.Message;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    /// <summary>
    /// アルファベットのみの入力値チェッククラス
    /// </summary>
    class AlphabetValueCheckAttribute : ValueCheckAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AlphabetValueCheckAttribute()
            : base("^[a-zA-Z]+$", FrameworkValidationMessage.AlphabetValueCheckMessage)
        {

        }
    }
}
