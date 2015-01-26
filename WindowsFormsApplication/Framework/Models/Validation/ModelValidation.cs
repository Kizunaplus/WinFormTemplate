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
        #region 検証
        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="control">チェック対象コントロール</param>
        /// <param name="attributes">チェック属性</param>
        /// <param name="message">エラーメッセージ</param>
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

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="obj">チェック対象</param>
        /// <param name="dataName">チェック対象名称</param>
        /// <param name="attributes">チェック属性</param>
        /// <param name="message">エラーメッセージ</param>
        public static bool Valid(Object obj, String dataName, IList<ModelValidationAttribute> attributes, out String message)
        {
            bool valid = true;
            message = "";

            // 入力チェック処理
            foreach (ModelValidationAttribute attr in attributes)
            {

                // DataGridView用のチェック
                if (attr.Valid(obj, ref message, dataName) == false)
                {
                    valid = false;
                    break;
                }
            }

            return valid;
        }
        #endregion

    }
}
