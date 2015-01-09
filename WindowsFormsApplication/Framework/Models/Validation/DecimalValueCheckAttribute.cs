using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using WindowsFormsApplication.Framework.Message;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    class DecimalValueCheckAttribute : ValueCheckAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DecimalValueCheckAttribute()
            : base("^[0-9]+\\.*[0-9]*$", FrameworkValidationMessage.DecimalValueCheckMessage)
        {

        }
    }
}
