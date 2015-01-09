using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using WindowsFormsApplication.Framework.Message;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
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
