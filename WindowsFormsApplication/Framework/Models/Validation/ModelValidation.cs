using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using System.Text.RegularExpressions;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.Validation
{
    /// <summary>
    /// 入力チェックサービス
    /// </summary>
    class ModelValidation
    {
        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="delegateMethod">計測するメソッド</param>
        /// <param name="parameters">メソッド引数</param>
        public static bool Valid(Control control, IList<ModelValidationAttribute> attributes, out String message)
        {
            bool valid = true;
            message = "";

            DataGridView dataGridView = control as DataGridView;

            // 入力チェック処理
            foreach (ModelValidationAttribute attr in attributes)
            {
                if (dataGridView != null)
                {
                    // DataGridView用のチェック
                    if (attr.Valid(dataGridView, ref message) == false)
                    {
                        valid = false;
                        break;
                    }
                }
                else
                {
                    // 標準コントロールのチェック
                    if (attr.Valid(control, ref message) == false)
                    {
                        valid = false;
                        break;
                    }
                }
            }

            return valid;
        }
    }
}
